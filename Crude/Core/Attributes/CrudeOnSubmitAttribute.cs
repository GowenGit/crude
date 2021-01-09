using System;

namespace Crude.Core.Attributes
{
    public sealed class CrudeOnSubmitAttribute : CrudeMethodAttribute
    {
        public string Name { get; }

        public CrudeOnSubmitAttribute(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
        }
    }
}