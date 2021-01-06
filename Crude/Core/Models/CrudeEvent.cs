using System;

namespace Crude.Core.Models
{
    public class CrudeEvent
    {
        public Action Callback { get; }

        public CrudeEvent(Action callback)
        {
            Callback = callback;
        }
    }
}