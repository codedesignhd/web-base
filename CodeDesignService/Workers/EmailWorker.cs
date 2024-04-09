using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeDesign.Services.Work;

namespace CodeDesign.Services.Workers
{
    public class EmailWorker : BackgroundService
    {
        private readonly ILogger<EmailWorker> _logger;

        private readonly SendEmailService _email_service;
        public EmailWorker(SendEmailService email_service, ILogger<EmailWorker> logger)
        {
            _email_service = email_service;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            double.TryParse(CodeDesign.Utilities.ConfigurationManager.AppSettings["MailSettings:DelayTime"], out double delay);
            if (delay == 0)
            {
                delay = 1;
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                _email_service.Run();
                await Task.Delay((int)(delay * 60000), stoppingToken);
            }
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);
            return base.StartAsync(cancellationToken);
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker stopped at: {time}", DateTimeOffset.Now);
            return base.StopAsync(cancellationToken);
        }
    }
}
