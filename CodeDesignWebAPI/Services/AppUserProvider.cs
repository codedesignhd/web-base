using CodeDesign.DTO.Vms;
using CodeDesign.Models;
using System.Security.Claims;

namespace CodeDesign.WebAPI.Services
{
    public class AppUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AppUserProvider> _logger;
        public AppUserProvider(IHttpContextAccessor httpContextAccessor, ILogger<AppUserProvider> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        public AppUser GetCurrentUser()
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User;
            AppUser appUser = new AppUser()
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
            appUser.Role = role;
            string props = principal.FindFirstValue(ClaimTypesCustom.THUOC_TINH);
            if (!string.IsNullOrWhiteSpace(props))
            {
                try
                {
                    appUser.Props = props.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
            return appUser;
        }
    }
}
