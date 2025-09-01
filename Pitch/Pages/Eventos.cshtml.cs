using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pitch.Models;
using Pitch.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pitch.Pages.Eventos
{
    [Authorize]
    public class EventosModel : PageModel
    {
        private readonly IEventoRepository _eventoRepository;
        private readonly UserManager<Users> _userManager;

        public EventosModel(IEventoRepository eventoRepository, UserManager<Users> userManager)
        {
            _eventoRepository = eventoRepository;
            _userManager = userManager;
        }

        public IEnumerable<Evento> ListaEventos { get; set; } = new List<Evento>();

        public async Task OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            if (!string.IsNullOrEmpty(userId))
            {
                ListaEventos = await _eventoRepository.ListarTodosAsync();
            }
        }
    }
}
