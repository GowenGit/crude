using Crude.Models.FieldFragments;
using Microsoft.AspNetCore.Components;

namespace Crude.Models.LayoutFragments
{
    internal class FieldGroupFragment : ICrudeLayoutFragment
    {
        private readonly string _name;
        private readonly FieldFragment _fieldFragment;

        public FieldGroupFragment(string name, FieldFragment fieldFragment)
        {
            _name = name;
            _fieldFragment = fieldFragment;
        }

        public RenderFragment Render(RenderContext context) => builder =>
        {
            var seq = 0;

            builder.OpenElement(seq++, "crude-field-fragment");

            builder.OpenElement(seq++, "label");

            builder.OpenElement(seq++, "span");
            builder.AddContent(seq++, _name.ToString(context.Formatter));
            builder.CloseElement();

            builder.AddContent(seq++, _fieldFragment.Render(context));

            builder.CloseElement();

            builder.CloseElement();
        };
    }
}