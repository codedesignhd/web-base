using CodeDesign.GoogleService;

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
    }
}