using CodeDesign.Couchbase;
using CodeDesignModels;
using CodeDesignWebAPI.Core.Constants;
using Microsoft.AspNetCore.Authorization;

namespace CodeDesignWebAPI.Extensions
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
                Server = new Uri(CodeDesignUtilities.ConfigurationManager.AppSettings["Couchbase:Server"]),
                BucketName = CodeDesignUtilities.ConfigurationManager.AppSettings["Couchbase:BucketName"],
                Username = CodeDesignUtilities.CryptoUtils.Decode(CodeDesignUtilities.ConfigurationManager.AppSettings["Couchbase:Username"]),
                Password = CodeDesignUtilities.CryptoUtils.Decode(CodeDesignUtilities.ConfigurationManager.AppSettings["Couchbase:Password"]),
            };
            services.AddSingleton<ICodeDesignCb, CodeDesignCb>(s =>
            {
                return new CodeDesignCb(options);
            });
        }
    }
}