using Microsoft.AspNetCore.Components;

namespace Crude.Models.Fragments
{
    internal interface IFragment
    {
        RenderFragment Render(RenderContext context);
    }
}