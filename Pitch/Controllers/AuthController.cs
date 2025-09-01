using Microsoft.AspNetCore.Mvc;
using Pitch.Models;
using Pitch.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Pitch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // DTO para cadastro
        public class RegisterDto
        {
            [Required]
            [EmailAddress]
            public string? Email { get; set; }

            [Required]
            [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
            public string? Password { get; set; }

            [Phone]
            public string? PhoneNumber { get; set; }
        }

        [HttpGet("teste")]
        public string Teste() => "API de autenticação está funcionando!";

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new Users
            {
                UserName = model.Email, // pode ser alterado para algo diferente do email
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            try
            {
                await _userRepository.AdicionarAsync(user, model.Password!);
                return Ok(new { message = "Usuário registrado com sucesso via Repository!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
