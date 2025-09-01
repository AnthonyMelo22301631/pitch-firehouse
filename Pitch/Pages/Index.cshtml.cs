using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pitch.Models;

namespace Pitch.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SignInManager<Users> _signInManager;
        private readonly ILogger<IndexModel> _logger;

        // �NICO construtor
        public IndexModel(SignInManager<Users> signInManager, ILogger<IndexModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        // Use OnGetAsync para m�todos ass�ncronos no Razor Pages
        public async Task OnGetAsync()
        {
#if DEBUG
            await _signInManager.SignOutAsync();
#endif
        }
    }
}
