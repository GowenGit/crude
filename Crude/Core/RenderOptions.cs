using System;
using Crude.Core.Formatters;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Crude.Core
{
    internal class RenderContext
    {
        public const string EmptyPlaceholder = "N/A";

        public const string NotRenderedPlaceholder = "No value resolver";

        internal BaseRenderFormatter Formatter { get; }

        public ComponentBase Receiver { get; }

        public object ViewModel { get; }

        public EditContext EditContext { get; }

        public Action StateHasChanged { get; }

        public RenderContext(
            ComponentBase receiver,
            Action stateHasChanged,
            object viewModel,
            CrudeOptions userOptions)
        {
            Receiver = receiver;
            StateHasChanged = stateHasChanged;
            ViewModel = viewModel;

            Formatter = userOptions.Formatter;

            EditContext = new EditContext(viewModel);
        }

        public EventCallback<T> CreateEvent<T>(Action<T> action)
        {
            return EventCallback.Factory.Create(Receiver, action);
        }

        public EventCallback CreateEvent(Action action)
        {
            return EventCallback.Factory.Create(Receiver, action);
        }
    }
}
