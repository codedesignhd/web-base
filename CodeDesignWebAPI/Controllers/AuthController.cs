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
using CodeDesign.Dtos.Validators;

namespace CodeDesign.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly ICodeDesignValidatorFactory _validator;
        public AuthController(IAuthService auth, ICodeDesignValidatorFactory validator)
        {
            _auth = auth;
            _validator = validator;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthRequest request)
        {
            var validate = _validator.Validate(request);
            if (validate.IsValid)
            {
                AuthResponse response = _auth.Authenticate(request);
                return Ok(response);
            }
            return BadRequest(new Response(false, validate.GetMessage()));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("refresh-token")]
        public IActionResult RefreshToken([FromBody] string token)
        {
            string refreshToken = token ?? _auth.GetTokenCookie();
            AuthResponse response = _auth.RefreshToken(refreshToken);
            _auth.SetTokenCookie(response.refreshToken);
            return Ok(response);
        }

        [HttpGet]
        [Route("sign-out")]
        public new async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(JwtBearerDefaults.AuthenticationScheme);
            return Ok(new Response(true, "success"));
        }
    }
}
