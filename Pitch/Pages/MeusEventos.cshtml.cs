using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pitch.Models;
using Pitch.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pitch.Pages
{
    [Authorize]
    public class MeusEventosModel : PageModel
    {
        private readonly IEventoRepository _eventoRepository;
        private readonly UserManager<Users> _userManager;

        public MeusEventosModel(IEventoRepository eventoRepository, UserManager<Users> userManager)
        {
            _eventoRepository = eventoRepository;
            _userManager = userManager;
        }

        public IEnumerable<Evento> ListaEventos { get; set; } = new List<Evento>();

        public async Task OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            ListaEventos = await _eventoRepository.ListarPorUsuarioAsync(userId);
        }
    }
}
