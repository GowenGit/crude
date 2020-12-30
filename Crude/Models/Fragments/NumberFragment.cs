using Microsoft.AspNetCore.Components;

namespace Crude.Models.Fragments
{
    internal class NumberFragment : ICrudeFragment
    {
        private readonly double _number;

        internal NumberFragment(double doubleValue)
        {
            _number = doubleValue;
        }

        public RenderFragment Render(RenderContext context) => builder =>
        {
            builder.AddContent(0, _number.ToString(context.Formatter));
        };
    }
}