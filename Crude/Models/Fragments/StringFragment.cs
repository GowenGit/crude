using Microsoft.AspNetCore.Components;

namespace Crude.Models.Fragments
{
    internal class StringFragment : ICrudeFragment
    {
        private readonly string _value;

        internal StringFragment(string value)
        {
            _value = value;
        }

        public RenderFragment Render(RenderOptions options) => builder =>
        {
            builder.AddContent(0, _value.ToString(options.Formatter));
        };
    }
}