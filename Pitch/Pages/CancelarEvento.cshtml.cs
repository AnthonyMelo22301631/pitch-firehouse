using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pitch.Repositories;
using System.Threading.Tasks;

namespace Pitch.Pages
{
    [Authorize]
    public class CancelarEventoModel : PageModel
    {
        private readonly IEventoRepository _eventoRepository;

        public CancelarEventoModel(IEventoRepository eventoRepository)
        {
            _eventoRepository = eventoRepository;
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _eventoRepository.CancelarAsync(id);
            TempData["ok"] = "Evento cancelado!";
            return RedirectToPage("/Eventos");
        }
    }
}
