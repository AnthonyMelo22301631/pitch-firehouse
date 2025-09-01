using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Pitch.Pages
{
    public class ColaboradoresModel : PageModel
    {
        public List<Colaborador> Colaboradores { get; set; }

        public void OnGet()
        {
            // Simulação de dados (substitua por consulta ao banco de dados)
            Colaboradores = new List<Colaborador>
            {
                new Colaborador { Nome = "João Silva", Especialidade = "Fotógrafo", Telefone = "(11) 99999-9999" },
                new Colaborador { Nome = "Maria Souza", Especialidade = "DJ", Telefone = "(21) 98888-8888" },
                new Colaborador { Nome = "Carlos Oliveira", Especialidade = "Buffet", Telefone = "(31) 97777-7777" }
            };
        }
    }

    public class Colaborador
    {
        public string Nome { get; set; }
        public string Especialidade { get; set; }
        public string Telefone { get; set; }
    }
}