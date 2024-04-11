using CodeDesign.Models;
using CodeDesign.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace CodeDesign.WebAPI.Services
{
    public static class AppPolicyProvider
    {
        public static AuthorizationOptions AddPolicies(this AuthorizationOptions options)
        {
            options.AddPolicy(AppPolicy.AdminOnly, policy =>
            {
                policy.RequireRole(Convert.ToString(Role.Admin));
            });
            return options;
        }
    }
}
