using System;
using System.Globalization;
using Crude.Core.Formatters;

namespace Crude
{
    public class CrudeOptions
    {
        internal BaseRenderFormatter Formatter { get; }

        public uint TablePageSize { get; } = 10;

        public uint TablePageLookahead { get; } = 3;

        public CrudeOptions(BaseRenderFormatter formatter, uint tablePageSize, uint tablePageLookahead)
        {
            if (tablePageSize < 1)
            {
                throw new ArgumentException("Page size should be greater than 0", nameof(tablePageSize));
            }

            TablePageSize = tablePageSize;
            TablePageLookahead = tablePageLookahead;
            Formatter = formatter;
        }

        public CrudeOptions(CultureInfo culture)
        {
            Formatter = new DefaultRenderFormatter(culture);
        }
    }
}