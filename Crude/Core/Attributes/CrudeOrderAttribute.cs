using System;

namespace Crude.Core.Attributes
{
    public sealed class CrudeOrderAttribute : CrudePropertyAttribute
    {
        public int Order { get; }

        public CrudeOrderAttribute(int order)
        {
            if (order < 0)
            {
                throw new ArgumentException("Order should be greater or equal to 0", nameof(order));
            }

            Order = order;
        }
    }
}