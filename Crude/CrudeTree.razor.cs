using Microsoft.AspNetCore.Components;
using System.Globalization;
using Crude.Core;
using Crude.Core.LayoutFragments;

namespace Crude
{
    public partial class CrudeTree
    {
        [Parameter]
        public object ViewModel { get; set; } = new object();

        [Parameter]
        public CrudeOptions Options { get; set; } = new CrudeOptions(CultureInfo.CurrentCulture);

        private RenderFragment Render()
        {
            var renderer = new CrudeTreeFragment();

            var context = new RenderContext(this, StateHasChanged, ViewModel, Options);

            return renderer.Render(context);
        }
    }
}
