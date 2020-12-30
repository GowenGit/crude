using Crude.Models.Formatters;
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

        public Action StateHasChanged { get; }

        public RenderContext(Action stateHasChanged, CrudeOptions userOptions)
        {
            StateHasChanged = stateHasChanged;

            TablePageSize = userOptions.TablePageSize;
            Formatter = userOptions.Formatter;
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
