using Microsoft.AspNetCore.Components;

namespace Crude.Models.Fragments
{
    internal class EmptyFragment : ICrudeFragment
    {
        public RenderFragment Render(RenderOptions options) => builder =>
        {
            builder.AddContent(0, RenderOptions.EmptyPlaceholder);
        };
    }

    internal class NotRenderedFragment : ICrudeFragment
    {
        public RenderFragment Render(RenderOptions options) => builder =>
        {
            builder.AddContent(0, RenderOptions.NotRenderedPlaceholder);
        };
    }
}