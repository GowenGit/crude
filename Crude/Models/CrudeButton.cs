using System;
using Microsoft.AspNetCore.Components;

namespace Crude.Models
{
    public class CrudeButton<T>
    {
        public EventCallback<T> Callback { get; }

        public string Name { get; }

        public CrudeButton(EventCallback<T> callback, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            Callback = callback;
        }
    }

    public class CrudeEvent
    {
        public Action Callback { get; }

        public CrudeEvent(Action callback)
        {
            Callback = callback;
        }
    }
}