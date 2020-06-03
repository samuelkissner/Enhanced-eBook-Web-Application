using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EnhancedEbookWebApp
{
    public class LoginModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        public string Result { get; set; }

        [BindProperty]
        public UserLogin LoginCredentials { get; set; }

        //constructor
        public LoginModel(UserManager<IdentityUser> userManager,
                          SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        
        public void OnGet()
        {
        }

        //login method
        public async Task<IActionResult> OnPostAsync(string returnUrl)
        {
            // check that the form was filled out correctly
            if (!ModelState.IsValid)
            {
                Result = "Please correct the form.";
                return Page();
            }

            //sign in user
            var result = await signInManager.PasswordSignInAsync(this.LoginCredentials.Email, this.LoginCredentials.Password, this.LoginCredentials.RememberMe, false);

            // check to see if the login attempt failed
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid Login Attempt");
                return Page();
            }
            

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return (LocalRedirect(returnUrl));
            }

            return RedirectToPage("/Index");
        }

      
        public async Task<IActionResult> OnPostLogout()
        {
            await signInManager.SignOutAsync();
            return RedirectToPage("/Index");

        }

        public class UserLogin
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remebmer Me")]
            public bool RememberMe { get; set; }
        }
 


    }
}