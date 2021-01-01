using System;
using Microsoft.AspNetCore.Components;

namespace Crude.Models.Fragments
{
    internal class NumberFragment<T> : IFieldFragment where T : IFormattable
    {
        private readonly T _number;

        internal NumberFragment(T doubleValue)
        {
            _number = doubleValue;
        }

        public RenderFragment RenderForm(RenderContext context)
        {
            throw new NotImplementedException();
        }

        public RenderFragment RenderValue(RenderContext context) => builder =>
        {
            builder.AddContent(0, _number.ToString(null, context.Formatter));
        };
    }
}