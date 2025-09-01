using System.Security.Claims;
using System.Threading.Tasks;
using Pitch.Models;
using System.Collections.Generic;

namespace Pitch.Repositories
{
    public interface IUserRepository
    {
        Task<List<Users>> ListarTodosAsync();
        Task<Users?> BuscarPorIdAsync(string id);
        Task<Users?> BuscarPorEmailAsync(string email);
        Task AdicionarAsync(Users user, string senha);
        Task AtualizarAsync(Users user);
        Task RemoverAsync(string id);

        // ✅ adicione esta linha
        Task<string?> GetCurrentUserIdAsync(ClaimsPrincipal user);
    }
}
