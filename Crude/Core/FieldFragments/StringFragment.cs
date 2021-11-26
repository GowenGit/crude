using Crude.Core.Parsers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Crude.Core.FieldFragments
{
    internal class StringFragment : FieldFragment
    {
        public override string FragmentType => "string";

        internal StringFragment(CrudeProperty property) : base(property) { }

        public override RenderFragment Render(RenderContext context) => builder =>
        {
            var seq = 0;

            builder.OpenComponent<InputText>(seq++);

            AddFieldAttributesByType(ref seq, builder);
        };
    }
}