using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Crude.Models.FieldFragments
{
    internal class EnumFragment : FieldFragment
    {
        private readonly Enum _value;

        internal EnumFragment(Enum value)
        {
            _value = value;
        }

        public override RenderFragment Render(RenderContext context) => builder =>
        {
            var seq = 0;

            builder.OpenComponent<InputText>(seq++);
            builder.AddAttribute(1, "Placeholder", _value.ToString());
            builder.CloseComponent();
        };
    }
}