using System;

namespace Crude.Core.Attributes
{
    public sealed class CrudeAutocompleteAttribute : CrudePropertyAttribute
    {
        public string Name { get; }

        public CrudeAutocompleteAttribute(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
        }
    }
}