using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Crude.Models.FieldFragments
{
    internal class DateFragment : FieldFragment
    {
        private readonly DateTime _value;

        internal DateFragment(DateTime value)
        {
            _value = value;
        }

        public override RenderFragment Render(RenderContext context) => builder =>
        {
            var seq = 0;

            builder.OpenComponent<InputDate<DateTime>>(seq++);
            builder.AddAttribute(1, "Placeholder", _value.ToString(context.Formatter));
            builder.CloseComponent();
        };
    }
}