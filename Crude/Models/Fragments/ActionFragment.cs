using System;
using Microsoft.AspNetCore.Components;

namespace Crude.Models.Fragments
{
    internal class ActionFragment : ICrudeFragment
    {
        private readonly ICrudeFragment _fragment;
        private readonly Action _action;

        internal ActionFragment(ICrudeFragment fragment, Action action)
        {
            _fragment = fragment;
            _action = action;
        }

        public RenderFragment Render(RenderContext context) => builder =>
        {
            var seq = 0;

            builder.OpenElement(seq++, "a");

            void ActionWithStateChange()
            {
                _action();
                context.StateHasChanged();
            }

            builder.AddAttribute(seq++, "onclick", (Action) ActionWithStateChange);
            builder.AddAttribute(seq++, "onclick:preventDefault", "true");
            builder.AddAttribute(seq++, "onclick:stopPropagation", "true");

            builder.AddContent(seq++, _fragment.Render(context));

            builder.CloseElement();
        };
    }
}