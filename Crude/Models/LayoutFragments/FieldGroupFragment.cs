using Crude.Models.Fragments;
using Microsoft.AspNetCore.Components;

namespace Crude.Models.LayoutFragments
{
    internal class FieldGroupFragment : ICrudeLayoutFragment
    {
        private readonly string _name;
        private readonly IFieldFragment _fieldFragment;

        public FieldGroupFragment(string name, IFieldFragment fieldFragment)
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

            builder.AddContent(seq++, _fieldFragment.RenderValue(context));

            builder.CloseElement();

            builder.CloseElement();
        };
    }
}