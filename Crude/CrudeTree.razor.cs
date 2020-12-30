using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Crude
{
    public partial class CrudeTree
    {
        [Parameter]
        public IReadOnlyList<object> ViewModels { get; set; } = new List<object>();

        [Parameter]
        public RenderOptions Options { get; set; } = new RenderOptions(CultureInfo.CurrentCulture);

        private CrudeTreeRenderer? _renderer;

        protected override Task OnInitializedAsync()
        {
            foreach (var viewModel in ViewModels)
            {
                _renderer = new CrudeTreeRenderer(viewModel);
            }

            return base.OnInitializedAsync();
        }
    }
}
