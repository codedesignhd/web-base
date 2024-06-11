using CodeDesignDtos.Accounts;
using CodeDesignDtos.Auth;

namespace CodeDesignWebAPI.Services.Auth
{
    public interface IAuthService
    {
        AuthResponse Authenticate(AuthRequest request);
        AuthResponse RefreshToken(string refreshToken);

        void SetTokenCookie(string refreshToken);
        string GetTokenCookie();

        AppUser GetUser();
    }
}
