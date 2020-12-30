using System;

namespace Crude.Models.Attributes
{
    public class CrudeOnClickAttribute : CrudeMethodAttribute
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