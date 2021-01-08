using Microsoft.AspNetCore.Components;

namespace Crude.Core.Fragments
{
    internal class ButtonFragment : IFragment
    {
        private readonly string _value;

        private readonly EventCallback _event;

        private readonly string _cssClass;

        private readonly bool _disabled;

        internal ButtonFragment(string value, EventCallback crudeEvent, string cssClass, bool disabled)
        {
            _value = value;
            _event = crudeEvent;
            _cssClass = cssClass;
            _disabled = disabled;
        }

        public RenderFragment Render(RenderContext context) => builder =>
        {
            var seq = 0;

            builder.OpenElement(seq++, "button");
            builder.AddAttribute(seq++, "onclick", _event);
            builder.AddAttribute(seq++, "disabled", _disabled);
            builder.AddAttribute(seq++, "onclick:preventDefault", "true");
            builder.AddAttribute(seq++, "onclick:stopPropagation", "true");

            if (!string.IsNullOrWhiteSpace(_cssClass))
            {
                builder.AddAttribute(seq++, "class", _cssClass);
            }

            builder.AddContent(seq++, _value);
            builder.CloseElement();
        };
    }
}