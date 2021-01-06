using Microsoft.AspNetCore.Components;

namespace Crude.Core.Fragments
{
    internal interface IFragment
    {
        RenderFragment Render(RenderContext context);
    }
}