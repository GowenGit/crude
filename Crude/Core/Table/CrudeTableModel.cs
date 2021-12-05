using System.Collections.Generic;

#pragma warning disable SA1623

namespace Crude.Core.Table
{
    public abstract class CrudeTableModel<T>
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
        public bool IsSearchable { get; set; }

        /// <summary>
        /// Gets if table is sortable.
        /// </summary>
        public bool IsSortable { get; set; }

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
        public string TableSearchPlaceholder { get; set; } = "Search";

        /// <summary>
        /// Gets find button value.
        /// </summary>
        public string TableFindButton { get; set; } = "Find";

        /// <summary>
        /// Gets max elements in the page.
        /// </summary>
        public uint TablePageSize { get; set; } = 10;

        /// <summary>
        /// Gets pagination buttons lookahead.
        /// </summary>
        public uint TablePageLookahead { get; set; } = 3;

        /// <summary>
        /// Fetch total element count so we can make predictions on how to paginate table.
        /// </summary>
        public abstract ulong GetTotalElementCount();

        public abstract IEnumerable<T> GetElements();
    }
}
