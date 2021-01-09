using System.Collections.Generic;

namespace Crude.Core.Models
{
    public abstract class CrudeTable<T>
        where T : class
    {
        public ulong Page { get; set; }

        public bool IsSearchable { get; set; }

        public string? UnescapedSearchTerm { get; set; }

        public bool IsSortable { get; set; }

        public string? SortColumn { get; set; }

        public bool SortDescending { get; set; }

        public ulong ElementCount { get; set; }

        public abstract IEnumerable<T> GetElements(ulong index, uint size);
    }
}
