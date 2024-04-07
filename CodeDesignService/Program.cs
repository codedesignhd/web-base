using CodeDesign.Services.Work;
using CodeDesign.Services.Workers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton(typeof(SendEmailService));
        services.AddHostedService<EmailWorker>();
    })
    .Build();

await host.RunAsync();
