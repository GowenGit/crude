using Crude.Core.Parsers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Crude.Core.FieldFragments
{
    internal class EnumFragment : FieldFragment
    {
        public override string FragmentType => "select";

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
                builder.AddAttribute(seq++, "value", value.ToString());
                builder.AddContent(seq++, GetDisplayName(value));
                builder.CloseElement();
            }
        };

        private static string GetDisplayName(object? value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var member = value
                .GetType()
                .GetMember(value.ToString() ?? string.Empty)
                .FirstOrDefault();

            var displayAttribute = member?.GetCustomAttribute<DisplayAttribute>();

            if (displayAttribute != null)
            {
                return displayAttribute.GetName() ?? value.ToString() ?? string.Empty;
            }

            return value.ToString() ?? string.Empty;
        }
    }
}