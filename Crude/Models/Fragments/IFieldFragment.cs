using Microsoft.AspNetCore.Components;

namespace Crude.Models.Fragments
{
    internal interface IFieldFragment
    {
        RenderFragment RenderForm(RenderContext context);

        RenderFragment RenderValue(RenderContext context);
    }
}