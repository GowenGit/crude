using Crude.Core.Attributes;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.ComponentModel.DataAnnotations;

namespace Crude.Demo.Wasm.ViewModel
{
    public class RegistrationViewModel
    {
        [Required]
        [Display(Name = "Display Name")]
        [CrudePlaceholder]
        [StringLength(20, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [CrudePlaceholder]
        [EmailAddress]
        public string Email { get; set; }

        [CrudePassword]
        [Required]
        [MinLength(6)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [CrudePassword]
        [Required]
        [Display(Name = "Re-type Password")]
        [Compare(nameof(Password))]
        public string PasswordConfirm { get; set; }

        [CrudeOnSubmit("Register")]
        private void OnRegister(EditContext context)
        {
            Console.WriteLine("Register button clicked");
        }

        [CrudeOnButtonClick("Login")]
        private void OnLogin(EditContext context)
        {
            Console.WriteLine("Login button clicked");
        }
    }
}
