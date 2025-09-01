using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pitch.Models;
using Pitch.Repositories;
using System.Threading.Tasks;

namespace Pitch.Pages.Eventos
{
    [Authorize]
    public class EditarEventoModel : PageModel
    {
        private readonly IEventoRepository _eventoRepository;

        public EditarEventoModel(IEventoRepository eventoRepository)
        {
            _eventoRepository = eventoRepository;
        }

        // ?? RENOMEADO para evitar ambiguidade
        [BindProperty]
        public Pitch.Models.Evento EventoVm { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var eventoDb = await _eventoRepository.BuscarPorIdAsync(id);
            if (eventoDb == null) return NotFound();

            EventoVm = eventoDb;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _eventoRepository.AtualizarAsync(EventoVm);
            TempData["ok"] = "Evento atualizado com sucesso!";
            return RedirectToPage("/Eventos/Eventos");
        }
    }
}
