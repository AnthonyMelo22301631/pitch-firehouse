using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pitch.Models;

namespace Pitch.Data
{
    public class AppDbContext : IdentityDbContext<Users>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // ✅ Eventos da aplicação
        public DbSet<Evento> Eventos { get; set; }

        // ✅ Comentários dos eventos
        public DbSet<Comentario> Comentarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração do relacionamento Evento -> Comentários
            modelBuilder.Entity<Comentario>()
                .HasOne(c => c.Evento)
                .WithMany(e => e.Comentarios)
                .HasForeignKey(c => c.EventoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuração do relacionamento Comentário -> Usuário
            modelBuilder.Entity<Comentario>()
                .HasOne(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
