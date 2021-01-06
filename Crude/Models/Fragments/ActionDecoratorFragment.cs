using System;
using Microsoft.AspNetCore.Components;

namespace Crude.Models.Fragments
{
    internal class ActionDecoratorFragment : IFragment
    {
        private readonly string _value;

        private readonly Action? _action;

        internal ActionDecoratorFragment(string value, Action? action)
        {
            _value = value;
            _action = action;
        }

        public RenderFragment Render(RenderContext context) => builder =>
        {
            var seq = 0;

            if (_action == null)
            {
                builder.AddContent(seq++, _value);

                return;
            }

            builder.OpenElement(seq++, "a");

            void ActionWithStateChange()
            {
                _action();
                context.StateHasChanged();
            }

            builder.AddAttribute(seq++, "onclick", (Action)ActionWithStateChange);
            builder.AddAttribute(seq++, "onclick:preventDefault", "true");
            builder.AddAttribute(seq++, "onclick:stopPropagation", "true");

            builder.AddContent(seq++, _value);

            builder.CloseElement();
        };
    }
}
