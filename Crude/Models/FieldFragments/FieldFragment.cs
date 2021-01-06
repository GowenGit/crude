﻿using Crude.Models.Fragments;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components.Forms;

namespace Crude.Models.FieldFragments
{
    internal abstract class FieldFragment : IFragment
    {
        protected CrudeProperty Property { get; }

        public string Identifier { get; }

        protected FieldFragment(CrudeProperty property)
        {
            Property = property;
            Identifier = $"{Guid.NewGuid()}";
        }

        public abstract RenderFragment Render(RenderContext context);

        /// <summary>
        /// We use this to handle both nullable and regular value types
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
            var expression = Expression.Lambda<Func<T>>(property);

            var valueChanged = EventCallback.Factory.Create<T>(this, value => Property.SetValue(value));

            //var identifier = FieldIdentifier.Create(expression);

            builder.AddAttribute(seq++, "Placeholder", Property.Name);
            builder.AddAttribute(seq++, "Value", Property.GetValue());
            builder.AddAttribute(seq++, "ValueChanged", valueChanged);
            builder.AddAttribute(seq++, "ValueExpression", expression);
            builder.AddAttribute(seq++, "id", Identifier);

            // Close input
            builder.CloseComponent();

            var type = typeof(ValidationMessage<>).MakeGenericType(Property.Info.PropertyType);

            builder.OpenComponent(seq++, type);
            builder.AddAttribute(seq++, "For", expression);
            builder.CloseComponent();
        }
    }
}