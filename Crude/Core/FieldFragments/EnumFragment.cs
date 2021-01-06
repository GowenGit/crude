using System;
using Crude.Core.Parsers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Crude.Core.FieldFragments
{
    internal class EnumFragment : FieldFragment
    {
        internal EnumFragment(CrudeProperty property) : base(property) { }

        public override RenderFragment Render(RenderContext context) => builder =>
        {
            var seq = 0;

            var enumType = Property.Info.PropertyType;

            var type = typeof(InputSelect<>).MakeGenericType(enumType);

            builder.OpenComponent(seq++, type);

            builder.AddAttribute(seq++, "ChildContent", RenderOptions(enumType));

            AddFieldAttributesByType(ref seq, builder, enumType);
        };

        private static RenderFragment RenderOptions(Type enumType) => builder =>
        {
            var seq = 0;

            foreach (var value in Enum.GetValues(enumType.UnwrapNullable()))
            {
                builder.OpenElement(seq++, "option");
                builder.AddContent(seq++, value.ToString());
                builder.CloseElement();
            }
        };
    }
}