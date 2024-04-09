using CodeDesign.Dtos;
using CodeDesign.Models;
using System.Security.Claims;

namespace CodeDesign.Web.Services
{
    public class AppUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AppUserService> _logger;
        public AppUserService(IHttpContextAccessor httpContextAccessor, ILogger<AppUserService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        public AppUser GetPrincipal()
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User;
            AppUser app_user = new AppUser()
            {
                IsAuthenticated = principal.Identity.IsAuthenticated,
                Username = principal.FindFirstValue(ClaimTypesCustom.USERNAME),
                Fullname = principal.FindFirstValue(ClaimTypes.GivenName),
                Email = principal.FindFirstValue(ClaimTypes.Email),
                Props = new List<int>(),
            };
            if (!Enum.TryParse(principal.FindFirstValue(ClaimTypes.Role), out Role role))
            {
                role = Role.USER;
            };
            app_user.Role = role;
            string prop_joined = principal.FindFirstValue(ClaimTypesCustom.THUOC_TINH);
            if (!string.IsNullOrWhiteSpace(prop_joined))
            {
                try
                {
                    app_user.Props = prop_joined.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
            return app_user;
        }
    }
}
