using System.Security.Claims;
using CodeDesign.Dtos;
using CodeDesign.BL;
using CodeDesign.BL.Response;
using CodeDesign.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace CodeDesign.Web.Controllers
{
    [Route("auth")]
    public class TaiKhoanController : BaseController
    {
        #region DI && Init
        public TaiKhoanController(DependencyContainer dependencies) : base(dependencies)
        {

        }

        #endregion

        #region Login
        [AllowAnonymous, HttpGet]
        [Route("/login", Name = "Login")]
        public IActionResult Login(string redirect_uri)
        {
            ViewBag.redirect_uri = redirect_uri;
            return View();
        }

        [AllowAnonymous, HttpPost]
        [Route("/PostLogin", Name = "PostLogin")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            CodeDesign.Models.Account tk = AccountBL.Instance.Login(request.username, request.password);
            if (tk != null)
            {
                //set cookie
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, tk.username),
                    new Claim(ClaimTypes.Role, Convert.ToString((int)tk.role)),
                    new Claim(ClaimTypes.Email, tk.email),
                    new Claim(ClaimTypes.GivenName, tk.fullname),
                    new Claim(ClaimTypesCustom.USERNAME, tk.username),
                    new Claim(ClaimTypesCustom.THUOC_TINH,string.Join(",", tk.thuoc_tinh)),
                };
                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                {
                    IsPersistent = request.is_remember,
                    AllowRefresh = true,
                    RedirectUri = Url.RouteUrl("Login"),
                    IssuedUtc = DateTime.UtcNow,
                    ExpiresUtc = DateTime.UtcNow.AddYears(1),
                });
                AccountBL.Instance.UpdateLastLogin(tk.username);

                string redirect_uri = request.redirect_uri;
                if (string.IsNullOrWhiteSpace(redirect_uri))
                {
                    redirect_uri = "/";
                }
                return Json(new Response<object>()
                {
                    success = true,
                    message = "Đăng nhập thành công",
                    data = new
                    {
                        redirectUri = redirect_uri
                    }
                });
            }
            return Json(new Response(false, "Tài khoản hoặc mật khẩu không chính xác"));
        }

        #endregion

        #region Register
        [AllowAnonymous, HttpGet]
        [Route("/register", Name = "Register")]
        public IActionResult Register(string redirect_uri)
        {
            ViewBag.redirect_uri = redirect_uri;
            return View();
        }
        [AllowAnonymous, HttpPost]
        [Route("/PostRegister", Name = "PostRegister")]
        public IActionResult Register(RegisterUserDto dto)
        {
            var validate = _dependencies.Validator.Register.Validate(dto);
            if (validate.IsValid)
            {
                KeyValuePair<bool, string> result = AccountBL.Instance.Register(dto);
                return Json(new Response(result));
            }
            else
            {
                return Json(new Response(false, validate.GetMessage()));
            }
        }

        #endregion

        #region Recovery

        [AllowAnonymous, HttpGet]
        [Route("/recovery", Name = "ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        #endregion

        #region 3-party Login
        [Route("facebook-signin")]
        public IActionResult FacebookSignIn()
        {
            return Json("");
        }
        [Route("/signin-google/")]
        public IActionResult GoogleSignIn()
        {
            return Json("");
        }
        #endregion

        [HttpGet, Route("/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
