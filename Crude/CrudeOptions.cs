using Crude.Core.Formatters;
using System;
using System.Globalization;

namespace Crude
{
    public class CrudeOptions
    {
        internal BaseRenderFormatter Formatter { get; }

        public string TableSearchPlaceholder { get; } = "Search";

        public string TableFindButton { get; } = "Find";

        public uint TablePageSize { get; } = 10;

        public uint TablePageLookahead { get; } = 3;

        public CrudeOptions(
            BaseRenderFormatter formatter,
            uint tablePageSize,
            uint tablePageLookahead,
            string tableSearchPlaceholder,
            string tableFindButton)
        {
            if (tablePageSize < 1)
            {
                throw new ArgumentException("Page size should be greater than 0", nameof(tablePageSize));
            }

            if (string.IsNullOrWhiteSpace(tableSearchPlaceholder))
            {
                throw new ArgumentNullException(nameof(tableSearchPlaceholder));
            }

            if (string.IsNullOrWhiteSpace(tableFindButton))
            {
                throw new ArgumentNullException(nameof(tableFindButton));
            }

            TablePageSize = tablePageSize;
            TablePageLookahead = tablePageLookahead;
            TableSearchPlaceholder = tableSearchPlaceholder;
            TableFindButton = tableFindButton;
            Formatter = formatter;
        }

        public CrudeOptions(CultureInfo culture)
        {
            Formatter = new DefaultRenderFormatter(culture);
        }
    }
}