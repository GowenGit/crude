using System;
using Microsoft.AspNetCore.Components;

namespace Crude.Models.Fragments
{
    internal class DateFragment : ICrudeValueFragment
    {
        private readonly DateTime _value;

        internal DateFragment(DateTime value)
        {
            _value = value;
        }

        public RenderFragment RenderForm(RenderContext context)
        {
            throw new NotImplementedException();
        }

        public RenderFragment RenderValue(RenderContext context) => builder =>
        {
            builder.AddContent(0, _value.ToString(context.Formatter));
        };
    }
}