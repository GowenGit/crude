using Crude.Core.Attributes;
using Crude.Core.Table;
using Crude.Demo.Wasm.Services;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Crude.Demo.Wasm.ViewModel
{
    public class UserProfileViewModel
    {
        [Required]
        [Display(Name = "Handle")]
        [MinLength(2)]
        public string Nickname { get; set; } = "Jeff";

        [CrudeDisable]
        [CrudeOrder(1)]
        [EmailAddress]
        public string Email { get; set; } = "jeff@email.com";

        [Required]
        public DateTime Birthday { get; set; } = DateTime.Now.AddYears(-30);

        [Required]
        public Gender Gender { get; set; } = Gender.Male;

        public string Description { get; set; }

        [CrudeIgnore]
        public string Name { get; set; }

        [CrudeDisable]
        [Display(Name = "Balance (USD)")]
        public int? Balance { get; set; }

        [CrudeOnSubmit("Save")]
        private void OnSave(EditContext context)
        {
            Console.WriteLine("Save button clicked");
        }

        [CrudeOnButtonClick("Close Account")]
        private void OnClose(EditContext context)
        {
            Console.WriteLine("Close Account button clicked");
        }
    }

    /// <summary>
    /// Very inefficient implementation.
    /// </summary>
    public class OwnedItemTable : CrudeTableModel<UserItemViewModel>
    {
        private readonly DummyDataService _dummyDataService;

        public OwnedItemTable(DummyDataService dummyDataService)
        {
            IsSearchable = false;
            IsSortable = true;
            TablePageSize = 10;
            _dummyDataService = dummyDataService;
        }

        public override Task<ulong> GetTotalElementCountAsync()
        {
            return Task.FromResult((ulong)GetElementsInternal(0, int.MaxValue).Count());
        }

        public override Task<IEnumerable<UserItemViewModel>> GetElementsAsync()
        {
            return Task.FromResult(GetElementsInternal((int)Page, (int)TablePageSize));
        }

        private IEnumerable<UserItemViewModel> GetElementsInternal(int pageIndex, int size)
        {
            var products = _dummyDataService.GetProducts();

            // Search part
            if (!string.IsNullOrWhiteSpace(UnescapedSearchTerm))
            {
                products = products.Where(product => product.DisplayName
                    .Contains(UnescapedSearchTerm, StringComparison.CurrentCultureIgnoreCase));
            }

            // Sort part
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
                .Select(product => new UserItemViewModel(product));
        }
    }

    public class UserItemViewModel
    {
        public int Id { get; }

        [Display(Name = "Name")]
        public string DisplayName { get; }

        /// <summary>
        /// List item view models need
        /// a default constructor.
        /// </summary>
        public UserItemViewModel() { }

        public UserItemViewModel(Product product)
        {
            Id = product.Id;
            DisplayName = product.DisplayName;
        }

        [CrudeOnClick(nameof(Id))]
        private void OnClick()
        {
            Console.WriteLine($"Id {Id} was pressed");
        }
    }

    public enum Gender
    {
        Male = 0,
        Female = 1
    }
}
