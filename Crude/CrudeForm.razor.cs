using Microsoft.AspNetCore.Components;
using System.Globalization;
using Crude.Core;
using Crude.Core.LayoutFragments;

namespace Crude
{
    public partial class CrudeForm
    {
        [Parameter]
        public object? ViewModel { get; set; } = new ();

        [Parameter]
        public CrudeOptions Options { get; set; } = new (CultureInfo.CurrentCulture);

        private RenderFragment Render()
        {
            var renderer = new CrudeFormFragment();

            var context = new RenderContext(this, StateHasChanged, ViewModel!, Options);

            return renderer.Render(context);
        }
    }
}
