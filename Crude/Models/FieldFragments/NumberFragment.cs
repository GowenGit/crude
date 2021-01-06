using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Crude.Models.FieldFragments
{
    internal class NumberFragment<T> : FieldFragment where T : IFormattable
    {
        private readonly T _number;

        internal NumberFragment(T doubleValue)
        {
            _number = doubleValue;
        }

        public override RenderFragment Render(RenderContext context) => builder =>
        {
            var seq = 0;

            builder.OpenComponent<InputNumber<T>>(seq++);
            builder.AddAttribute(1, "Placeholder", _number.ToString(null, context.Formatter));
            builder.CloseComponent();
        };
    }
}