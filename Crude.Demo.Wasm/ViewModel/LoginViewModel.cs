using System;
using System.ComponentModel.DataAnnotations;
using Crude.Core.Attributes;
using Microsoft.AspNetCore.Components.Forms;

namespace Crude.Demo.Wasm.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [CrudeAutocomplete("email")]
        public string Email { get; set; }

        [CrudePassword]
        [Required]
        [Display(Name = "Password")]
        [CrudeAutocomplete("current-password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

        [CrudeOnSubmit("Login")]
        private void OnLogin(EditContext context)
        {
            Console.WriteLine($"User typed in {Email}");
        }
    }
}