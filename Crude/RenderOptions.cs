using Crude.Models.Formatters;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Globalization;

namespace Crude
{
    internal class RenderContext
    {
        public const string EmptyPlaceholder = "N/A";

        public const string NotRenderedPlaceholder = "No value resolver";

        internal BaseRenderFormatter Formatter { get; }

        public int TablePageSize { get; }

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

    public class CrudeOptions
    {
        internal BaseRenderFormatter Formatter { get; }

        public int TablePageSize { get; } = 10;

        public CrudeOptions(BaseRenderFormatter formatter, int tablePageSize)
        {
            if (tablePageSize < 1)
            {
                throw new ArgumentException("Page size should be greater than 0", nameof(tablePageSize));
            }

            TablePageSize = tablePageSize;
            Formatter = formatter;
        }

        public CrudeOptions(CultureInfo culture)
        {
            Formatter = new DefaultRenderFormatter(culture);
        }
    }
}
