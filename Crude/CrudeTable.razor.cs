using Crude.Core;
using Crude.Core.Table;
using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace Crude
{
    public partial class CrudeTable<TItem>
        where TItem : class
    {
        [Parameter]
        public CrudeTableModel<TItem>? ViewModel { get; set; }

        [Parameter]
        public CrudeOptions Options { get; set; } = new (CultureInfo.CurrentCulture);

        private RenderFragment Render()
        {
            var renderer = new CrudeTableFragment<TItem>(ViewModel!);

            var context = new RenderContext(this, StateHasChanged, ViewModel!, Options);

            return renderer.Render(context);
        }
    }
}
