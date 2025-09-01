using Microsoft.AspNetCore.Identity;

namespace Pitch.Models
{
    public class Users : IdentityUser
    {
        public string? Nome { get; set; }
        public string? Especialidade { get; set; }
        public string? Descricao { get; set; }
    }
}
