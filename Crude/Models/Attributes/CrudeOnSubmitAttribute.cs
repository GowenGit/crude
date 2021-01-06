using System;

namespace Crude.Models.Attributes
{
    public class CrudeOnSubmitAttribute : CrudeMethodAttribute
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