using System;
using Microsoft.AspNetCore.Components;

namespace Crude.Models.Fragments
{
    internal class EnumFragment : ICrudeValueFragment
    {
        private readonly Enum _value;

        internal EnumFragment(Enum value)
        {
            _value = value;
        }

        public RenderFragment RenderForm(RenderContext context)
        {
            throw new NotImplementedException();
        }

        public RenderFragment RenderValue(RenderContext context) => builder =>
        {
            builder.AddContent(0, _value.ToString());
        };
    }
}