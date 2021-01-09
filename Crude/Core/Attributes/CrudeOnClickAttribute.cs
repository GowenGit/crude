using System;

namespace Crude.Core.Attributes
{
    public sealed class CrudeOnClickAttribute : CrudeMethodAttribute
    {
        public string Property { get; }

        public CrudeOnClickAttribute(string property)
        {
            if (string.IsNullOrWhiteSpace(property))
            {
                throw new ArgumentNullException(nameof(property));
            }

            Property = property;
        }
    }
}