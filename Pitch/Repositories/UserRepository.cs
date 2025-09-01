using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pitch.Data;
using Pitch.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pitch.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;

        public UserRepository(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<Users>> ListarTodosAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<Users?> BuscarPorIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<Users?> BuscarPorEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task AdicionarAsync(Users user, string senha)
        {
            await _userManager.CreateAsync(user, senha);
        }

        public async Task AtualizarAsync(Users user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        // ➕ Recupera o id do usuário logado
        public Task<string?> GetCurrentUserIdAsync(ClaimsPrincipal user)
        {
            var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Task.FromResult(id);
        }
    }
}
