using Microsoft.AspNetCore.Components;
using System.Globalization;

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
            var renderer = new CrudeTreeRenderer();

            var context = new RenderContext(ViewModel, StateHasChanged, Options);

            return renderer.Render(context);
        }
    }
}
