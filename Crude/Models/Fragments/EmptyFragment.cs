using Microsoft.AspNetCore.Components;

namespace Crude.Models.Fragments
{
    internal class EmptyFragment : ICrudeFragment
    {
        public RenderFragment Render(RenderContext context) => builder =>
        {
            builder.AddContent(0, RenderContext.EmptyPlaceholder);
        };
    }

    internal class NotRenderedFragment : ICrudeFragment
    {
        public RenderFragment Render(RenderContext context) => builder =>
        {
            builder.AddContent(0, RenderContext.NotRenderedPlaceholder);
        };
    }
}