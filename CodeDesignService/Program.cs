using CodeDesignServices.Work;
using CodeDesignServices.Workers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton(typeof(SendEmailService));
        services.AddHostedService<EmailWorker>();
    })
    .Build();

await host.RunAsync();
