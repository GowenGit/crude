using Crude.Models.Fragments;
using Microsoft.AspNetCore.Components;

namespace Crude.Models.LayoutFragments
{
    internal class FieldFragment : ICrudeFragment
    {
        private readonly LabelFragment _label;

        private readonly ICrudeFragment _element;

        public FieldFragment(LabelFragment label, ICrudeFragment element)
        {
            _label = label;
            _element = element;
        }

        public RenderFragment Render(RenderOptions options) => builder =>
        {
            var seq = 0;

            builder.OpenElement(seq++, "crude-field-fragment");

            builder.OpenElement(seq++, "crude-field-label");
            builder.AddContent(seq++, _label.Render(options));
            builder.CloseElement();

            builder.OpenElement(seq++, "crude-field-value");
            builder.AddContent(seq++, _element.Render(options));
            builder.CloseElement();

            builder.CloseElement();
        };
    }
}