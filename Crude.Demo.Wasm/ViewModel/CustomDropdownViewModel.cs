using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Crude.Core.Attributes;
using Crude.Core.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace Crude.Demo.Wasm.ViewModel
{
    public class CustomDropdownViewModel
    {
        [Required]
        [Display(Name = "Choose Your Pet")]
        public CustomDropdown Pet { get; set; } = new();

        [CrudeOnSubmit("Save")]
        private void OnSave(EditContext context)
        {
            Console.WriteLine($"User selected {Pet.Value}");
        }
    }

    public class CustomDropdown : CrudeDropdown
    {
        public CustomDropdown()
        {
            Value = "dog";
        }

        public override IEnumerable<KeyValuePair<string, string>> GetDropdownPairs()
        {
            return new List<KeyValuePair<string, string>>
            {
                new(string.Empty, string.Empty),
                new("snake", "Snake Jim"),
                new("dog", "Dog Jeff")
            };
        }
    }
}