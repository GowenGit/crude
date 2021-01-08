using System;

namespace Crude.Core.Attributes
{
    public class CrudeOnButtonClickAttribute : CrudeMethodAttribute
    {
        public string Name { get; }

        public CrudeOnButtonClickAttribute(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
        }
    }
}