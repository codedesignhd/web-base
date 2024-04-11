﻿using CodeDesign.Dtos;
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
            AppUser app_user = new AppUser()
            {
                IsAuthenticated = principal.Identity.IsAuthenticated,
                Username = principal.FindFirstValue(ClaimTypesCustom.Username),
                Fullname = principal.FindFirstValue(ClaimTypes.GivenName),
                Email = principal.FindFirstValue(ClaimTypes.Email),
                Props = new List<int>(),
            };
            if (!Enum.TryParse(principal.FindFirstValue(ClaimTypes.Role), out Role role))
            {
                role = Role.User;
            };
            app_user.Role = role;
            string props = principal.FindFirstValue(ClaimTypesCustom.Properties);
            if (!string.IsNullOrWhiteSpace(props))
            {
                try
                {
                    app_user.Props = props
                        .Split(',')
                        .Select(x => Convert.ToInt32(x))
                        .ToList();
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
