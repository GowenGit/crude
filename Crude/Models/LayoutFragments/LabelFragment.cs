using System;
using Crude.Models.Fragments;
using Microsoft.AspNetCore.Components;

namespace Crude.Models.LayoutFragments
{
    internal class LabelFragment : ICrudeLayoutFragment
    {
        private readonly string _name;
        private readonly ICrudeValueFragment _valueFragment;

        public LabelFragment(string name, ICrudeValueFragment valueFragment)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            _name = name;
            _valueFragment = valueFragment;
        }

        public RenderFragment Render(RenderContext context) => builder =>
        {
            var seq = 0;

            builder.OpenElement(seq++, "label");

            builder.OpenElement(seq++, "span");
            builder.AddContent(seq++, _name.ToString(context.Formatter));
            builder.CloseElement();

            builder.AddContent(seq++, _valueFragment.RenderValue(context));

            builder.CloseElement();
        };
    }
}
