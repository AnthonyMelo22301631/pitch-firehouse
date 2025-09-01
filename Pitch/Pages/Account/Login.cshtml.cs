using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pitch.Models;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Pitch.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<Users> _signInManager;
        private readonly UserManager<Users> _userManager;

        public LoginModel(SignInManager<Users> signInManager, UserManager<Users> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string ErrorMessage { get; set; } = string.Empty;

        public class InputModel
        {
            [Required, EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required, DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            public bool RememberMe { get; set; }
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user is null)
            {
                ErrorMessage = "E-mail não encontrado.";
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,                   // overload com o usuário
                Input.Password,
                Input.RememberMe,
                lockoutOnFailure: true
            );

            if (result.Succeeded)
                return RedirectToPage("/Perfil");

            if (result.IsLockedOut)
                ErrorMessage = "Usuário bloqueado. Tente novamente mais tarde.";
            else
                ErrorMessage = "Login inválido.";

            return Page();
        }
    }
}
