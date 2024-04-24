using CodeDesign.BL;
using CodeDesign.Dtos.Accounts;
using CodeDesign.Dtos.Auth;
using CodeDesign.Models;
using CodeDesign.Utilities.Constants;
using Microsoft.CodeAnalysis.Scripting;
using Nest;
using System.Security.Claims;
using System.Security.Principal;

namespace CodeDesign.WebAPI.Services
{
    public interface IAuthService
    {
        AuthResponse Authenticate(AuthRequest request);
        AuthResponse RefreshToken(string refreshToken);

        void SetTokenCookie(string refreshToken);
        string GetTokenCookie();

        AppUser GetUser();
    }

    internal class AuthService : IAuthService
    {
        private readonly JwtService _jwt;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string _cookieRefreshToken = "refreshToken";
        public AuthService(JwtService jwt, IHttpContextAccessor contextAccessor)
        {
            _jwt = jwt;
            _contextAccessor = contextAccessor;
        }

        public string GetIP()
        {
            return Utilities.CommonUtils.GetIpAddress(_contextAccessor.HttpContext);
        }

        public AuthResponse Authenticate(AuthRequest request)
        {
            Account user = AccountBL.Instance.Login(request);
            if (user != null)
            {
                string accessToken = _jwt.GenAccessToken(user);
                string refreshToken = string.Empty;
                if (user.IsValidRefreshToken())
                {
                    refreshToken = user.refresh_token.token;
                }
                else
                {
                    RefreshToken refreshTokenInfo = _jwt.GenerateRefreshToken(GetIP());
                    AccountBL.Instance.UpdateRefreshToken(user.username, refreshTokenInfo);
                    SetTokenCookie(refreshTokenInfo.token);
                    refreshToken = refreshTokenInfo.token;
                }
                return new AuthResponse("Authenticated", true, accessToken, refreshToken);
            }
            return new AuthResponse { success = false, message = "Tài khoản hoặc mật khẩu không chính xác" };
        }

        public AuthResponse RefreshToken(string refreshToken)
        {
            var user = AccountBL.Instance.GetAccountByRefreshToken(refreshToken);
            if (user is null)
                return new AuthResponse("Token không hợp lệ");
            string accessToken = _jwt.GenAccessToken(user);

            return new AuthResponse("Authenticated", true, accessToken, refreshToken);

        }

        public void SetTokenCookie(string refreshToken)
        {
            _contextAccessor.HttpContext.Response.Cookies.Append(_cookieRefreshToken, refreshToken, new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(7),
                SameSite = SameSiteMode.Lax,
                HttpOnly = true
            });
        }
        public string GetTokenCookie()
        {
            return _contextAccessor.HttpContext.Request.Cookies[_cookieRefreshToken];
        }
        public AppUser GetUser()
        {
            ClaimsPrincipal principal = _contextAccessor.HttpContext.User;
            AppUser user = new AppUser()
            {
                IsAuthenticated = principal.Identity.IsAuthenticated,
                Username = principal.FindFirstValue(ClaimTypesCustom.Username),
                Fullname = principal.FindFirstValue(ClaimTypes.GivenName),
                Email = principal.FindFirstValue(ClaimTypes.Email),
                Props = new List<int>(),
            };
            if (long.TryParse(principal.FindFirstValue(ClaimTypesCustom.Id), out long id))
            {
                user.Id = id;
            }
            if (!Enum.TryParse(principal.FindFirstValue(ClaimTypes.Role), out Role role))
            {
                role = Role.Guest;
            };
            user.Role = role;
            string props = principal.FindFirstValue(ClaimTypesCustom.Properties);
            if (!string.IsNullOrWhiteSpace(props))
            {
                user.Props = props
                    .Split(',')
                    .Select(it =>
                    {
                        int.TryParse(it, out int prop);
                        return prop;
                    })
                    .ToList();
            }
            return user;
        }
    }
}
