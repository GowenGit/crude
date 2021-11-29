using System;
using System.Linq.Expressions;
using System.Reflection;
using Crude.Core.Fragments;
using Crude.Core.Parsers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace Crude.Core.FieldFragments
{
    internal abstract class FieldFragment : IFragment
    {
        protected CrudeProperty Property { get; }

        public string Identifier { get; }

        protected bool ValueIsSet { get; set; }

        protected object? Value { get; set; }

        protected object? ValueExpression { get; set; }

        protected object? ValueChanged { get; set; }

        protected Type? ValidationObjectType { get; set; }

        public abstract string FragmentType { get; }

        protected FieldFragment(CrudeProperty property)
        {
            Property = property;
            Identifier = $"{Guid.NewGuid()}";
        }

        public abstract RenderFragment Render(RenderContext context);

        /// <summary>
        /// We use this to handle both nullable and regular value types.
        /// </summary>
        protected void AddFieldAttributesByType(ref int seq, RenderTreeBuilder builder, Type? type = null)
        {
            if (type == null)
            {
                type = Property.Info.PropertyType;
            }

            var method = typeof(FieldFragment).GetMethod("AddFieldAttributes", BindingFlags.Instance | BindingFlags.NonPublic)!;
            var genericMethod = method.MakeGenericMethod(type);
            var args = new object[] { seq, builder };

            genericMethod.Invoke(this, args);

            seq = (int)args[0];
        }

        private void AddFieldAttributes<T>(ref int seq, RenderTreeBuilder builder)
        {
            var constant = Expression.Constant(Property.ViewModel);
            var property = Expression.Property(constant, Property.Info);

            var expression = ValueExpression ?? Expression.Lambda<Func<T>>(property);

            var valueChanged = ValueChanged ?? EventCallback.Factory.Create<T>(this, value => Property.SetValue(value));

            if (Property.Placeholder)
            {
                builder.AddAttribute(seq++, "Placeholder", Property.Name);
            }

            if (!string.IsNullOrWhiteSpace(Property.AutocompleteLabel))
            {
                builder.AddAttribute(seq++, "autocomplete", Property.AutocompleteLabel);
            }

            builder.AddAttribute(seq++, "Value", ValueIsSet ? Value : Property.GetValue());
            builder.AddAttribute(seq++, "ValueChanged", valueChanged);
            builder.AddAttribute(seq++, "ValueExpression", expression);
            builder.AddAttribute(seq++, "name", Property.Name);
            builder.AddAttribute(seq++, "id", Identifier);
            builder.AddAttribute(seq++, "disabled", Property.Disabled);

            if (Property.Password && typeof(T) == typeof(string))
            {
                builder.AddAttribute(seq++, "type", "password");
            }

            // Close input
            builder.CloseComponent();

            var type = typeof(ValidationMessage<>).MakeGenericType(ValidationObjectType ?? Property.Info.PropertyType);

            builder.OpenComponent(seq++, type);
            builder.AddAttribute(seq++, "For", expression);
            builder.CloseComponent();
        }
    }
}