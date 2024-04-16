using CodeDesign.BL;
using CodeDesign.Dtos.Auth;
using CodeDesign.Dtos.Responses;
using CodeDesign.Models;
using CodeDesign.WebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeDesign.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        public AuthController(IAuthService auth)
        {

            _auth = auth;

        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthRequest request)
        {
            AuthResponse response = _auth.Authenticate(request);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public IActionResult RefreshToken([FromBody] string token)
        {
            string refreshToken = token ?? _auth.GetTokenCookie();
            AuthResponse response = _auth.RefreshToken(refreshToken);
            _auth.SetTokenCookie(response.refreshToken);
            return Ok(response);
        }

        [HttpGet, Route("sign-out")]
        public new async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(JwtBearerDefaults.AuthenticationScheme);
            return new JsonResult(new Response(true, "success"));
        }
    }
}
