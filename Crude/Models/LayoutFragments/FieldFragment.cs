using Microsoft.AspNetCore.Components;

namespace Crude.Models.LayoutFragments
{
    internal class FieldFragment : ICrudeLayoutFragment
    {
        private readonly LabelFragment _label;

        public FieldFragment(LabelFragment label)
        {
            _label = label;
        }

        public RenderFragment Render(RenderContext context) => builder =>
        {
            var seq = 0;

            builder.OpenElement(seq++, "crude-field-fragment");

            builder.AddContent(seq++, _label.Render(context));

            builder.CloseElement();
        };
    }
}