using CodeDesign.GoogleService;
using CodeDesign.Models;
using CodeDesign.WebAPI.Core.Constants;
using Microsoft.AspNetCore.Authorization;

namespace CodeDesign.WebAPI.ServiceExtensions
{
    /// <summary>
    /// ApplicationServicesExtensions
    /// </summary>
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddGoogleService(this IServiceCollection services)
        {
            services.AddScoped<GDriverService>();
            return services;
        }
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