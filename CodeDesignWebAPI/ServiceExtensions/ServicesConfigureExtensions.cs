using CodeDesign.BL.Providers;
using CodeDesign.Couchbase;
using CodeDesign.Models;
using CodeDesign.WebAPI.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;

namespace CodeDesign.WebAPI.ServiceExtensions
{
    /// <summary>
    /// ApplicationServicesExtensions
    /// </summary>
    public static class ServicesConfigureExtensions
    {
        public static IServiceCollection AddGoogleService(this IServiceCollection services)
        {
            //services.AddScoped<GDriverService>();
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

        public static void AddCouchbase(this IServiceCollection services)
        {
            CouchbaseConfigOptions options = new CouchbaseConfigOptions
            {
                Server = new Uri(Utilities.ConfigurationManager.AppSettings["Couchbase:Server"]),
                BucketName = Utilities.ConfigurationManager.AppSettings["Couchbase:BucketName"],
                Username = Utilities.CryptoUtils.Decode(Utilities.ConfigurationManager.AppSettings["Couchbase:Username"]),
                Password = Utilities.CryptoUtils.Decode(Utilities.ConfigurationManager.AppSettings["Couchbase:Password"]),
            };
            services.AddSingleton<ICodeDesignCb, CodeDesignCb>(s =>
            {
                return new CodeDesignCb(options);
            });
        }
    }
}