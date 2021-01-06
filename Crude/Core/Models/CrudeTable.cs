using System.Collections.Generic;

namespace Crude.Core.Models
{
    public abstract class CrudeTable<T> where T : class
    {
        public int Page { get; set; }

        public abstract IEnumerable<T> GetElements(int index, int size);
    }
}
