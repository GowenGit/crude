using System;
using Microsoft.AspNetCore.Components;

namespace Crude.Models.Fragments
{
    internal class DateFragment : ICrudeFragment
    {
        private readonly DateTime _value;

        internal DateFragment(DateTime value)
        {
            _value = value;
        }

        public RenderFragment Render(RenderContext context) => builder =>
        {
            builder.AddContent(0, _value.ToString(context.Formatter));
        };
    }
}