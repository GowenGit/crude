using System;
using System.Globalization;
using Crude.Core.Formatters;

namespace Crude
{
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