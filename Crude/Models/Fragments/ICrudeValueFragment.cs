using Microsoft.AspNetCore.Components;

namespace Crude.Models.Fragments
{
    internal interface ICrudeValueFragment
    {
        RenderFragment RenderForm(RenderContext context);

        RenderFragment RenderValue(RenderContext context);
    }
}