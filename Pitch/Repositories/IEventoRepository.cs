using Pitch.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pitch.Repositories
{
    public interface IEventoRepository
    {
        Task<IEnumerable<Evento>> ListarTodosAsync();
        Task<IEnumerable<Evento>> ListarPorUsuarioAsync(string userId);
        Task<Evento?> BuscarPorIdAsync(int id);

        Task AdicionarAsync(Evento evento);
        Task AtualizarAsync(Evento evento);
        Task RemoverAsync(int id);

        // ➕ Novos métodos
        Task CancelarAsync(int id);                    // Soft delete
        Task AdicionarComentarioAsync(Comentario c);   // Adicionar comentário
    }
}
