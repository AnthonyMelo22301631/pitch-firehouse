using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pitch.Models;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Pitch.Pages.Account
{
    public class CadastroModel : PageModel
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;

        public CadastroModel(UserManager<Users> userManager, SignInManager<Users> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty, Required]
        public string UserName { get; set; }

        [BindProperty, Required, EmailAddress]
        public string Email { get; set; }

        [BindProperty, Required, DataType(DataType.Password)]
        public string Senha { get; set; }

        [BindProperty]
        public string Telefone { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = new Users
            {
                UserName = UserName,
                Email = Email,
                PhoneNumber = Telefone
            };

            var result = await _userManager.CreateAsync(user, Senha);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                TempData["Mensagem"] = "Usuário cadastrado com sucesso!";
                return RedirectToPage("/Perfil");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return Page();
        }
    }
}
