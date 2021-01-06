using Crude.Core.Parsers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Crude.Core.FieldFragments
{
    internal class BooleanFragment : FieldFragment
    {
        internal BooleanFragment(CrudeProperty property) : base(property) { }

        public override RenderFragment Render(RenderContext context) => builder =>
        {
            var seq = 0;

            builder.OpenComponent<InputCheckbox>(seq++);

            AddFieldAttributesByType(ref seq, builder);
        };
    }
}