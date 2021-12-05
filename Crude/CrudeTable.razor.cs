using Crude.Core;
using Crude.Core.Table;
using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Threading.Tasks;

namespace Crude
{
    public partial class CrudeTable<TItem>
        where TItem : class
    {
        private CrudeTableFragment<TItem>? _renderer;

        [Parameter]
        public CrudeTableModel<TItem>? ViewModel { get; set; }

        [Parameter]
        public CrudeOptions Options { get; set; } = new (CultureInfo.CurrentCulture);

        protected override async Task OnInitializedAsync()
        {
            _renderer = new CrudeTableFragment<TItem>(ViewModel!);

            await _renderer.LoadData();
        }

        private RenderFragment Render()
        {
            var context = new RenderContext(this, StateHasChanged, ViewModel!, Options);

            return _renderer!.Render(context);
        }
    }
}
