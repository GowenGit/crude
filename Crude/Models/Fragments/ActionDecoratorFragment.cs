using System;
using Microsoft.AspNetCore.Components;

namespace Crude.Models.Fragments
{
    internal class ActionDecoratorFragment : IFragment
    {
        private readonly string _value;

        private readonly CrudeEvent? _event;

        internal ActionDecoratorFragment(string value, CrudeEvent? crudeEvent)
        {
            _value = value;
            _event = crudeEvent;
        }

        public RenderFragment Render(RenderContext context) => builder =>
        {
            var seq = 0;

            if (_event == null)
            {
                builder.AddContent(seq++, _value);

                return;
            }

            builder.OpenElement(seq++, "a");

            builder.AddAttribute(seq++, "onclick", context.CreateEvent(_event.Callback));
            builder.AddAttribute(seq++, "onclick:preventDefault", "true");
            builder.AddAttribute(seq++, "onclick:stopPropagation", "true");

            builder.AddContent(seq++, _value);

            builder.CloseElement();
        };
    }
}
