using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pitch.Models;
using Pitch.Repositories;
using System;
using System.Threading.Tasks;

namespace Pitch.Pages.CriacaoEventos
{
    [Authorize]
    public class CriacaoEventosModel : PageModel
    {
        private readonly IEventoRepository _eventoRepository;
        private readonly UserManager<Users> _userManager;

        public CriacaoEventosModel(IEventoRepository eventoRepository,
                                   UserManager<Users> userManager)
        {
            _eventoRepository = eventoRepository;
            _userManager = userManager;
        }

        // Campos do formulário
        [BindProperty] public string TipoFesta { get; set; } = string.Empty;
        [BindProperty] public string[]? Servicos { get; set; }
        [BindProperty] public string TipoLugar { get; set; } = string.Empty;
        [BindProperty] public DateTime DataEvento { get; set; }

        public bool hasData { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // >>> PEGA O ID REAL DO IDENTITY <<<
            var userId = _userManager.GetUserId(User); // mesmo que (await _userManager.GetUserAsync(User))?.Id
            if (string.IsNullOrEmpty(userId))
            {
                // por segurança, se algo falhar com o login
                return Challenge(); // força reautenticação
            }

            var evento = new Evento
            {
                TipoFesta = TipoFesta,
                Servicos = string.Join(",", Servicos ?? Array.Empty<string>()),
                TipoLugar = TipoLugar,
                DataEvento = DataEvento,
                UserId = userId // <-- ID do AspNetUsers
            };

            await _eventoRepository.AdicionarAsync(evento);

            hasData = true;

            // redireciona para a listagem
            return RedirectToPage("/Eventos"); // ajuste para a sua rota
        }
    }
}
