using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Crude.Models.FieldFragments
{
    internal class NumberFragment<T> : FieldFragment
    {
        internal NumberFragment(CrudeProperty property) : base(property) { }

        public override RenderFragment Render(RenderContext context) => builder =>
        {
            var seq = 0;

            builder.OpenComponent<InputNumber<T>>(seq++);

            AddFieldAttributesByType(ref seq, builder);
        };
    }
}