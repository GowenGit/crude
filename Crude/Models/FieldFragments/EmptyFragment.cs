using Microsoft.AspNetCore.Components;

namespace Crude.Models.FieldFragments
{
    internal class NotRenderedFragment : FieldFragment
    {
        internal NotRenderedFragment(CrudeProperty property) : base(property) { }

        public override RenderFragment Render(RenderContext context) => builder =>
        {
            builder.AddContent(0, RenderContext.NotRenderedPlaceholder);
        };
    }
}