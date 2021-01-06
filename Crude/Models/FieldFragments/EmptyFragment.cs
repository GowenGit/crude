using Microsoft.AspNetCore.Components;

namespace Crude.Models.FieldFragments
{
    internal class NotRenderedFragment : FieldFragment
    {
        public override RenderFragment Render(RenderContext context) => builder =>
        {
            builder.AddContent(0, RenderContext.NotRenderedPlaceholder);
        };

        public RenderFragment RenderValue(RenderContext context) => builder =>
        {
            builder.AddContent(0, RenderContext.NotRenderedPlaceholder);
        };
    }
}