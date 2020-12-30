using Microsoft.AspNetCore.Components;

namespace Crude.Models.Fragments
{
    internal class BooleanFragment : ICrudeFragment
    {
        private readonly bool _value;

        internal BooleanFragment(bool value)
        {
            _value = value;
        }

        public RenderFragment Render(RenderContext context) => builder =>
        {
            builder.AddContent(0, _value.ToString(context.Formatter));
        };
    }
}