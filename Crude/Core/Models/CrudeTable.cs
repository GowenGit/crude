using System;
using System.Collections.Generic;

#pragma warning disable SA1623

namespace Crude.Core.Models
{
    public abstract class CrudeTable<T>
        where T : class
    {
        /// <summary>
        /// Gets or sets current page index.
        /// </summary>
        public ulong Page { get; set; }

        /// <summary>
        /// Gets or sets search term provided by the user.
        /// </summary>
        public string? UnescapedSearchTerm { get; set; }

        /// <summary>
        /// Gets if table is searchable.
        /// </summary>
        public bool IsSearchable { get; }

        /// <summary>
        /// Gets if table is sortable.
        /// </summary>
        public bool IsSortable { get; }

        /// <summary>
        /// Gets or sets which column to sort by.
        /// </summary>
        public string? SortColumn { get; set; }

        /// <summary>
        /// Gets or sets if the sorting order.
        /// </summary>
        public bool SortDescending { get; set; }

        /// <summary>
        /// Gets search placeholder.
        /// </summary>
        public string TableSearchPlaceholder { get; }

        /// <summary>
        /// Gets find button value.
        /// </summary>
        public string TableFindButton { get; }

        /// <summary>
        /// Gets max elements in the page.
        /// </summary>
        public uint TablePageSize { get; }

        /// <summary>
        /// Gets pagination buttons lookahead.
        /// </summary>
        public uint TablePageLookahead { get; }

        /// <summary>
        /// Fetch total element count so we can make predictions on how to paginate table.
        /// </summary>
        public abstract ulong GetTotalElementCount();

        public abstract IEnumerable<T> GetElements();

        protected CrudeTable(
            bool isSearchable,
            bool isSortable,
            string tableSearchPlaceholder = "Search",
            string tableFindButton = "Find",
            uint tablePageSize = 10,
            uint tablePageLookahead = 3)
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

            IsSearchable = isSearchable;
            IsSortable = isSortable;
            TableSearchPlaceholder = tableSearchPlaceholder;
            TableFindButton = tableFindButton;
            TablePageSize = tablePageSize;
            TablePageLookahead = tablePageLookahead;
        }
    }
}
