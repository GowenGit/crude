using Crude.Core.Models;
using Crude.Core.Parsers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Linq.Expressions;

namespace Crude.Core.FieldFragments
{
    internal class CrudeDropdownFragment : FieldFragment
    {
        public override string FragmentType => "select";

        internal CrudeDropdownFragment(CrudeProperty property) : base(property) { }

        public override RenderFragment Render(RenderContext context) => builder =>
        {
            var seq = 0;

            if (Property.GetValue() is not CrudeDropdown dropdown)
            {
                return;
            }

            var type = typeof(InputSelect<string>);

            builder.OpenComponent(seq++, type);

            builder.AddAttribute(seq++, "ChildContent", RenderOptions(dropdown));

            Expression<Func<string?>> expression = () => dropdown.Value;

            ValueChanged = EventCallback.Factory.Create<string>(this, value => dropdown.Value = value);
            ValueExpression = expression;
            Value = dropdown.Value;
            ValueIsSet = true;
            ValidationObjectType = typeof(string);

            AddFieldAttributesByType(ref seq, builder, typeof(CrudeDropdown));
        };

        private static RenderFragment RenderOptions(CrudeDropdown dropdown) => builder =>
        {
            var seq = 0;

            foreach (var (key, value) in dropdown.GetDropdownPairs())
            {
                builder.OpenElement(seq++, "option");
                builder.AddAttribute(seq++, "value", key);
                builder.AddContent(seq++, value);
                builder.CloseElement();
            }
        };
    }
}