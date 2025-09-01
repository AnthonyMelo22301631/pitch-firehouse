using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pitch.Models;
using System.Threading.Tasks;

namespace Pitch.Pages
{
    [Authorize]
    public class PerfilModel : PageModel
    {
        private readonly UserManager<Users> _userManager;

        public PerfilModel(UserManager<Users> userManager)
        {
            _userManager = userManager;
        }

        public Users Usuario { get; set; }
        public bool EhColaborador { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Usuario = await _userManager.GetUserAsync(User);

            if (Usuario == null)
            {
                // Se não tiver usuário logado, volta para a página de login
                return RedirectToPage("/Account/Login");
            }

            EhColaborador = await _userManager.IsInRoleAsync(Usuario, "Colaborador");
            return Page();
        }

        public async Task<IActionResult> OnPostTornarColaborador(string especialidade, string descricao)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            user.Especialidade = especialidade;
            user.Descricao = descricao;

            await _userManager.UpdateAsync(user);
            await _userManager.AddToRoleAsync(user, "Colaborador");

            EhColaborador = true;

            return RedirectToPage();
        }
    }
}
