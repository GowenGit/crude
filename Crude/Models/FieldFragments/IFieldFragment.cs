using Crude.Models.Fragments;
using Microsoft.AspNetCore.Components;

namespace Crude.Models.FieldFragments
{
    internal abstract class FieldFragment : IFragment
    {
        public abstract RenderFragment Render(RenderContext context);
    }
}