using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pitch.Models;
using Pitch.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pitch.Pages.Eventos
{
    [Authorize]
    public class DetalhesEventoModel : PageModel
    {
        private readonly IEventoRepository _eventoRepository;
        private readonly IUserRepository _userRepository;

        public DetalhesEventoModel(IEventoRepository eventoRepository, IUserRepository userRepository)
        {
            _eventoRepository = eventoRepository;
            _userRepository = userRepository;
        }

        // ?? Nomes diferentes para evitar ambiguidade
        public Pitch.Models.Evento EventoAtual { get; set; }
        public List<Comentario> Comentarios { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var eventoDb = await _eventoRepository.BuscarPorIdAsync(id);
            if (eventoDb == null) return NotFound();

            EventoAtual = eventoDb;
            Comentarios = new List<Comentario>(eventoDb.Comentarios ?? new List<Comentario>());
            return Page();
        }

        public async Task<IActionResult> OnPostComentarAsync(int id, string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                TempData["err"] = "Digite um comentário válido.";
                return RedirectToPage(new { id });
            }

            var userId = await _userRepository.GetCurrentUserIdAsync(User);
            var comentario = new Comentario
            {
                EventoId = id,
                UserId = userId!,
                Texto = texto.Trim()
            };

            await _eventoRepository.AdicionarComentarioAsync(comentario);
            return RedirectToPage(new { id });
        }
    }
}
