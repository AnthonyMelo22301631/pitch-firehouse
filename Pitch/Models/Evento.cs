using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pitch.Models
{
    public class Evento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tipo da Festa")]
        public string TipoFesta { get; set; }

        // Guardar serviços selecionados (ex: "Buffet,DJ,Fotógrafo")
        public string Servicos { get; set; }

        [Required]
        [Display(Name = "Tipo de Lugar")]
        public string TipoLugar { get; set; }

        [DataType(DataType.Date)]
        [Required]
        [Display(Name = "Data do Evento")]
        public DateTime DataEvento { get; set; }

        // Relação com o usuário criador
        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public Users Criador { get; set; }

        // ✅ Novo: soft delete
        [Display(Name = "Evento Cancelado?")]
        public bool Cancelado { get; set; } = false;

        // ✅ Novo: relação com comentários
        public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
    }
}
