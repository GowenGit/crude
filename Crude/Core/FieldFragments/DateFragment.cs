using Crude.Core.Parsers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Crude.Core.FieldFragments
{
    internal class DateFragment<T> : FieldFragment
    {
        internal DateFragment(CrudeProperty property) : base(property) { }

        public override RenderFragment Render(RenderContext context) => builder =>
        {
            var seq = 0;

            builder.OpenComponent<InputDate<T>>(seq++);

            AddFieldAttributesByType(ref seq, builder);
        };
    }
}