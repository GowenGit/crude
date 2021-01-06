using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Crude.Models.FieldFragments
{
    internal class StringFragment : FieldFragment
    {
        internal StringFragment(CrudeProperty property) : base(property) { }

        public override RenderFragment Render(RenderContext context) => builder =>
        {
            var seq = 0;

            builder.OpenComponent<InputText>(seq++);

            AddFieldAttributesByType(ref seq, builder);
        };
    }
}