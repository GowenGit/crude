using Microsoft.AspNetCore.Components;

namespace Crude.Models.Fragments
{
    internal class EmptyFragment : ICrudeValueFragment
    {
        public RenderFragment RenderForm(RenderContext context) => builder =>
        {
            builder.AddContent(0, RenderContext.NotRenderedPlaceholder);
        };

        public RenderFragment RenderValue(RenderContext context) => builder =>
        {
            builder.AddContent(0, RenderContext.NotRenderedPlaceholder);
        };
    }

    internal class NotRenderedFragment : ICrudeValueFragment
    {
        public RenderFragment RenderForm(RenderContext context) => builder =>
        {
            builder.AddContent(0, RenderContext.NotRenderedPlaceholder);
        };

        public RenderFragment RenderValue(RenderContext context) => builder =>
        {
            builder.AddContent(0, RenderContext.NotRenderedPlaceholder);
        };
    }
}