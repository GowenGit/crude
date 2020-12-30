using Microsoft.AspNetCore.Components;

namespace Crude.Models.Fragments
{
    internal class BooleanFragment : ICrudeValueFragment
    {
        private readonly bool _value;

        internal BooleanFragment(bool value)
        {
            _value = value;
        }

        public RenderFragment RenderForm(RenderContext context)
        {
            throw new System.NotImplementedException();
        }

        public RenderFragment RenderValue(RenderContext context) => builder =>
        {
            builder.AddContent(0, _value.ToString(context.Formatter));
        };
    }
}