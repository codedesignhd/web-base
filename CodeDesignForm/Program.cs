using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.Design;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DemoForm;
using CodeDesignSql;
using CodeDesignSql.Repositories;

namespace CodeDesignForm
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            var host = CreateHostBuilder().Build();
            ServiceProvider = host.Services;

            Application.Run(ServiceProvider.GetRequiredService<Form1>());
        }


        public static IServiceProvider? ServiceProvider { get; private set; }
        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<DemoDbContext>(options =>
                    {
                        options.UseSqlServer(context.Configuration.GetConnectionString("DemoDb"));
                    });
                    services.AddSingleton<UnitOfWork>();
                    services.AddTransient<Form1>();
                });
        }
    }
}