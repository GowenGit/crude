using Crude.Core.Attributes;
using Crude.Core.Table;
using Crude.Demo.Wasm.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Crude.Demo.Wasm.ViewModel
{
    public class ProductTable : CrudeTableModel<ProductListingViewModel>
    {
        private readonly DummyDataService _dummyDataService;

        public ProductTable(DummyDataService dummyDataService)
        {
            IsSearchable = true;
            IsSortable = true;
            TablePageSize = 5;
            _dummyDataService = dummyDataService;
        }

        public override Task<ulong> GetTotalElementCountAsync()
        {
            return Task.FromResult((ulong)GetElementsInternal(0, int.MaxValue).Count());
        }

        public override Task<IEnumerable<ProductListingViewModel>> GetElementsAsync()
        {
            return Task.FromResult(GetElementsInternal((int)Page, (int)TablePageSize));
        }

        private IEnumerable<ProductListingViewModel> GetElementsInternal(int pageIndex, int size)
        {
            var products = _dummyDataService.GetProducts();

            // Search part
            if (!string.IsNullOrWhiteSpace(UnescapedSearchTerm))
            {
                products = products.Where(product => product.DisplayName
                    .Contains(UnescapedSearchTerm, StringComparison.CurrentCultureIgnoreCase));
            }

            // Sort part
            // Note: this method is extremely inefficient.
            // It uses reflection to fetch item values based on
            // name. For production use it is advisable to offload
            // sorting to the database.
            if (!string.IsNullOrWhiteSpace(SortColumn))
            {
                var propertyInfo = typeof(Product).GetProperty(SortColumn);

                if (SortDescending)
                {
                    products = products
                        .OrderByDescending(
                            product => propertyInfo!.GetValue(product));
                }
                else
                {
                    products = products
                        .OrderBy(
                            product => propertyInfo!.GetValue(product));
                }
            }

            // Pagination part
            return products
                .Skip(pageIndex * size)
                .Take(size)
                .Select(product => new ProductListingViewModel(product));
        }
    }

    public class ProductListingViewModel
    {
        public int Id { get; }

        [Display(Name = "Name")]
        public string DisplayName { get; }

        public int Quantity { get; }

        [Display(Name = "Price (USD)")]
        public decimal Price { get; }

        /// <summary>
        /// List item view models need
        /// a default constructor.
        /// </summary>
        public ProductListingViewModel() { }

        public ProductListingViewModel(Product product)
        {
            Id = product.Id;
            DisplayName = product.DisplayName;
            Quantity = product.Quantity;
            Price = product.Price;
        }

        [CrudeOnClick(nameof(Id))]
        private async Task OnClick()
        {
            // Some busy task
            await Task.Delay(1000);

            Console.WriteLine($"Id {Id} was pressed");
        }
    }
}