using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pitch.Models;
using Pitch.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Pitch.Pages
{
    [Authorize]
    [ValidateAntiForgeryToken]
    public class EditarEventoModel : PageModel
    {
        private readonly IEventoRepository _eventoRepository;
        private readonly UserManager<Users> _userManager;

        public EditarEventoModel(IEventoRepository eventoRepository, UserManager<Users> userManager)
        {
            _eventoRepository = eventoRepository;
            _userManager = userManager;
        }

        [BindProperty]
        public Evento Evento { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var eventoDb = await _eventoRepository.BuscarPorIdAsync(id);
            if (eventoDb == null) return NotFound();

            // só o dono pode editar
            var userId = _userManager.GetUserId(User);
            if (eventoDb.UserId != userId) return Forbid();

            Evento = eventoDb;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id) // id também vem pela rota
        {
            // Se o hidden não trouxe, usa o id da rota
            if (Evento.Id == 0) Evento.Id = id;

            if (Evento.Id == 0)
            {
                ModelState.AddModelError("", "Id do evento não foi enviado.");
                return Page();
            }

            // ⚠️ Ignora validação dos campos que NÃO vêm no form (mas são Required no model)
            ModelState.Remove("Evento.UserId");
            ModelState.Remove("Evento.Criador");
            ModelState.Remove("Evento.CriadorId");                 // se existir no seu model
            ModelState.Remove("Evento.Criador.UserName");          // por garantia

            if (!ModelState.IsValid)
            {
                // Mostra os erros restantes (ex.: campos realmente editáveis vazios)
                TempData["err"] = string.Join(" | ",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Page();
            }

            var eventoDb = await _eventoRepository.BuscarPorIdAsync(Evento.Id);
            if (eventoDb == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (eventoDb.UserId != userId) return Forbid();

            // Atualiza apenas os campos editáveis
            eventoDb.TipoFesta = (Evento.TipoFesta ?? "").Trim();
            eventoDb.Servicos = (Evento.Servicos ?? "").Trim();
            eventoDb.TipoLugar = (Evento.TipoLugar ?? "").Trim();
            eventoDb.DataEvento = Evento.DataEvento;

            // O método do repositório PRECISA dar SaveChangesAsync()
            await _eventoRepository.AtualizarAsync(eventoDb);

            TempData["ok"] = "Evento atualizado com sucesso!";
            return RedirectToPage("/DetalhesEvento", new { id = eventoDb.Id });
        }
    }
}
