using CodeDesign.GoogleService;
using CodeDesign.Models;
using CodeDesign.WebAPI.Core.Constants;
using Microsoft.AspNetCore.Authorization;

namespace CodeDesign.WebAPI.ServiceExtensions
{
    /// <summary>
    /// ApplicationServicesExtensions
    /// </summary>
    public static class ServicesConfigureExtensions
    {
        public static IServiceCollection AddGoogleService(this IServiceCollection services)
        {
            services.AddScoped<GDriverService>();
            return services;
        }
        public static AuthorizationOptions AddPolicies(this AuthorizationOptions options)
        {
            options.AddPolicy(PolicyNames.SysOnly, policy =>
            {
                policy.RequireRole(Convert.ToString(Role.Sys));
            });
            options.AddPolicy(PolicyNames.AdminOnly, policy =>
            {
                policy.RequireRole(Convert.ToString(Role.Sys), Convert.ToString(Role.Admin));
            });
            options.AddPolicy(PolicyNames.UserOnly, policy =>
            {
                policy.RequireRole(Convert.ToString(Role.User));
            });
            return options;
        }
    }
}