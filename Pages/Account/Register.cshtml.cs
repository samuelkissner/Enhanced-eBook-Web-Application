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
    public class RegisterModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        public string Result { get; private set; }

        public RegisterModel(UserManager<IdentityUser> userManager,
                             SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        
        [BindProperty]
        public NewUserRegistration RegistrationInfo { get; set; }
        public void OnGet()
        {
        }

        //method to register a new user 
        public async Task<IActionResult> OnPostAsync()
        {
            // check that the form was filled out correctly
            if (!ModelState.IsValid)
            {
                Result = "Please correct the form.";
                return Page();
            }
            // create a new Identity User using form data stored in the instance of NewUserRegistration
            var user = new IdentityUser
            {
                UserName = this.RegistrationInfo.Email,
                Email = this.RegistrationInfo.Email
            };

            //add new user to the database 
            var result = await userManager.CreateAsync(user, this.RegistrationInfo.Password);

            //if registration failed, then add errors to model
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return Page();
                }
            }

            //if the user was successfully registered, then log them in and return to the index
            await signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToPage("/Index");
        }
    }
    public class NewUserRegistration
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password",
            ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}