using CodeDesign.ES;
using CodeDesign.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CodeDesign.Services.Work
{
    public class SendEmailService
    {
        private readonly ILogger<SendEmailService> _logger;
        public SendEmailService(ILogger<SendEmailService> logger)
        {
            _logger = logger;
        }
        public void Run()
        {
            try
            {
                int max_error = Convert.ToInt32(Utilities.ConfigurationManager.AppSettings["MailSettings:MaxError"]);
                if (max_error < 1)
                {
                    max_error = 5;
                }


                int port = Convert.ToInt32(Utilities.ConfigurationManager.AppSettings["MailSettings:Port"]);
                string host = Utilities.ConfigurationManager.AppSettings["MailSettings:Host"];
                string sender_address = Utilities.ConfigurationManager.AppSettings["MailSettings:SenderAddress"];
                string password = Utilities.ConfigurationManager.AppSettings["MailSettings:SenderSecret"];
                SmtpClient mailClient = new SmtpClient(host)
                {
                    Port = port,
                    Credentials = new NetworkCredential(sender_address, password),
                    EnableSsl = true,
                };

                mailClient.SendCompleted += OnMailSendCompleted;
                string sender_name = Utilities.ConfigurationManager.AppSettings["MailSettings:SenderName"];

                MailAddress sender = new MailAddress(sender_address, sender_name);
                List<Email> emails = EmailRepository.Instance.GetMailResend(max_error, new string[] { "nguoi_nhan", "title", "body" }).ToList();
                emails.Add(new Email
                {
                    nguoi_nhan = "tungvv@xmedia.vn",
                    title = "test",
                    body = "test",
                });
                foreach (var email in emails)
                {
                    MailAddress to = new MailAddress(email.nguoi_nhan);
                    MailMessage message = new MailMessage(sender, to)
                    {
                        Body = email.body,
                        IsBodyHtml = true,
                        Subject = email.title,
                    };
                    Task t = mailClient.SendMailAsync(message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private void OnMailSendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            _logger.LogInformation(JsonSerializer.Serialize(e));
        }
    }
}
