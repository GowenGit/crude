using Crude.Models.Formatters;
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

        public object ViewModel { get; }

        public Action StateHasChanged { get; }

        public EditContext EditContext { get; }

        public RenderContext(
            object viewModel,
            Action stateHasChanged,
            CrudeOptions userOptions)
        {
            ViewModel = viewModel;
            StateHasChanged = stateHasChanged;

            TablePageSize = userOptions.TablePageSize;
            Formatter = userOptions.Formatter;

            EditContext = new EditContext(viewModel);
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
