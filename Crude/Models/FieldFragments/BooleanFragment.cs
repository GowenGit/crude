using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Crude.Models.FieldFragments
{
    internal class BooleanFragment : FieldFragment
    {
        private readonly bool _value;

        internal BooleanFragment(bool value)
        {
            _value = value;
        }

        public override RenderFragment Render(RenderContext context) => builder =>
        {
            var seq = 0;

            builder.OpenComponent<InputText>(seq++);
            builder.AddAttribute(1, "Placeholder", _value.ToString(context.Formatter));
            builder.CloseComponent();
        };
    }
}