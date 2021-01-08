using System;
using Microsoft.AspNetCore.Components;

namespace Crude.Core.Models
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

    public class CrudeButton
    {
        public EventCallback Callback { get; }

        public string Name { get; }

        public CrudeButton(EventCallback callback, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            Callback = callback;
        }
    }
}