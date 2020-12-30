using System;
using Microsoft.AspNetCore.Components;

namespace Crude.Models.Fragments
{
    internal class EnumFragment : ICrudeFragment
    {
        private readonly Enum _value;

        internal EnumFragment(Enum value)
        {
            _value = value;
        }

        public RenderFragment Render(RenderOptions options) => builder =>
        {
            builder.AddContent(0, _value.ToString());
        };
    }
}