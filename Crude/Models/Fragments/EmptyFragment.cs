using Microsoft.AspNetCore.Components;

namespace Crude.Models.Fragments
{
    // TODO: Not needed, change to standard fragment usages
    internal class EmptyFragment : IFieldFragment
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

    internal class NotRenderedFragment : IFieldFragment
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