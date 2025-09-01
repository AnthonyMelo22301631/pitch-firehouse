using Microsoft.EntityFrameworkCore;
using Pitch.Data;
using Pitch.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitch.Repositories
{
    public class EventoRepository : IEventoRepository
    {
        private readonly AppDbContext _context;

        public EventoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Evento>> ListarTodosAsync()
        {
            return await _context.Eventos
                .Include(e => e.Criador)
                .Include(e => e.Comentarios)
                .ToListAsync();
        }

        public async Task<IEnumerable<Evento>> ListarPorUsuarioAsync(string userId)
        {
            return await _context.Eventos
                .Include(e => e.Criador)
                .Include(e => e.Comentarios)
                .Where(e => e.UserId == userId)
                .ToListAsync();
        }

        public async Task<Evento?> BuscarPorIdAsync(int id)
        {
            return await _context.Eventos
                .Include(e => e.Criador)
                .Include(e => e.Comentarios).ThenInclude(c => c.Usuario)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task AdicionarAsync(Evento evento)
        {
            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Evento evento)
        {
            _context.Eventos.Update(evento);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento != null)
            {
                _context.Eventos.Remove(evento);
                await _context.SaveChangesAsync();
            }
        }

        // ➕ Soft delete
        public async Task CancelarAsync(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento != null)
            {
                evento.Cancelado = true; // marca como cancelado
                _context.Eventos.Update(evento);
                await _context.SaveChangesAsync();
            }
        }

        // ➕ Comentários
        public async Task AdicionarComentarioAsync(Comentario c)
        {
            _context.Comentarios.Add(c);
            await _context.SaveChangesAsync();
        }
    }
}
