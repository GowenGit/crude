using Crude.Models.Formatters;
using System;
using System.Globalization;

namespace Crude
{
    public class RenderOptions
    {
        public const string EmptyPlaceholder = "N/A";

        public const string NotRenderedPlaceholder = "No value resolver";

        internal BaseRenderFormatter Formatter { get; }

        public int TablePageSize { get; } = 10;

        public RenderOptions(BaseRenderFormatter formatter, int tablePageSize)
        {
            if (tablePageSize < 1)
            {
                throw new ArgumentException("Page size should be greater than 0", nameof(tablePageSize));
            }

            Formatter = formatter;
        }

        public RenderOptions(BaseRenderFormatter formatter)
        {
            Formatter = formatter;
        }

        public RenderOptions(CultureInfo culture)
        {
            Formatter = new DefaultRenderFormatter(culture);
        }
    }
}
