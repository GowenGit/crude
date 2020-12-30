using Microsoft.AspNetCore.Components;
using System;

namespace Crude.Models.Fragments
{
    internal class ActionFragment : ICrudeValueFragment
    {
        private readonly ICrudeValueFragment _fragment;
        private readonly Action _action;

        internal ActionFragment(ICrudeValueFragment fragment, Action action)
        {
            _fragment = fragment;
            _action = action;
        }

        public RenderFragment RenderForm(RenderContext context) => builder =>
        {
            throw new NotImplementedException();
        };

        public RenderFragment RenderValue(RenderContext context) => builder =>
        {
            var seq = 0;

            builder.OpenElement(seq++, "a");

            void ActionWithStateChange()
            {
                _action();
                context.StateHasChanged();
            }

            builder.AddAttribute(seq++, "onclick", (Action)ActionWithStateChange);
            builder.AddAttribute(seq++, "onclick:preventDefault", "true");
            builder.AddAttribute(seq++, "onclick:stopPropagation", "true");

            builder.AddContent(seq++, _fragment.RenderValue(context));

            builder.CloseElement();
        };
    }
}