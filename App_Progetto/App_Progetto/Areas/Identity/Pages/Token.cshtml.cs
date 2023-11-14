using App_Progetto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App_Progetto.Areas.Identity.Pages
{
    [Authorize]
    public class JwtTokenModel : PageModel
    {
        readonly ITokenRepository _tokenRepository;

        public Token? Token { get; set; }

        public JwtTokenModel(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public void OnGet(int days = 10)
        {
            Token = _tokenRepository.GenerateToken(days);
        }
    }
}
