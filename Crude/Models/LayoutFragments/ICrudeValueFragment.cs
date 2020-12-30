using Microsoft.AspNetCore.Components;

namespace Crude.Models.LayoutFragments
{
    internal interface ICrudeLayoutFragment
    {
        RenderFragment Render(RenderContext context);
    }
}