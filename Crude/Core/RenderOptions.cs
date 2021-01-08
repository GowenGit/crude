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

        public uint TablePageSize { get; }

        public uint TablePageLookahead { get; }

        public IHandleEvent Receiver { get; }

        public object ViewModel { get; }

        public EditContext EditContext { get; }

        public RenderContext(
            IHandleEvent receiver,
            object viewModel,
            CrudeOptions userOptions)
        {
            Receiver = receiver;
            ViewModel = viewModel;

            TablePageSize = userOptions.TablePageSize;
            TablePageLookahead = userOptions.TablePageLookahead;
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
