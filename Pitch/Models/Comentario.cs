using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pitch.Models
{
    public class Comentario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int EventoId { get; set; }

        [ForeignKey(nameof(EventoId))]
        public Evento Evento { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public Users Usuario { get; set; }

        [Required]
        [MaxLength(500)]
        public string Texto { get; set; }

        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    }
}
