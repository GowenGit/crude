using Crude.Core.Parsers;
using Microsoft.AspNetCore.Components;

namespace Crude.Core.FieldFragments
{
    internal class NotRenderedFragment : FieldFragment
    {
        public override string FragmentType => "empty";

        internal NotRenderedFragment(CrudeProperty property) : base(property) { }

        public override RenderFragment Render(RenderContext context) => builder =>
        {
            builder.AddContent(0, RenderContext.NotRenderedPlaceholder);
        };
    }
}