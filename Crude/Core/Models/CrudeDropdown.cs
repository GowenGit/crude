using System.Collections.Generic;

namespace Crude.Core.Models
{
    public abstract class CrudeDropdown
    {
        public abstract IEnumerable<KeyValuePair<string, string>> GetDropdownPairs();

        public string Value { get; set; } = string.Empty;
    }
}