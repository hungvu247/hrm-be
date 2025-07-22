using human_resource_management.Dto;
using human_resource_management.IService;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace human_resource_management.Service
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> config)
        {
            _settings = config.Value;

            _smtpClient = new SmtpClient(_settings.Host, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.Username, _settings.Password),
                EnableSsl = true
            };
        }

        public async Task SendEmailAsync(string toEmail, string password)
        {
            var templatePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                _settings.TemplateFolder,
                _settings.TemplateFileName
            );

            var templateContent = await File.ReadAllTextAsync(templatePath);

            var htmlBody = templateContent
                .Replace("{{WelcomeTitle}}", _settings.WelcomeTitle)
                .Replace("{{WelcomeMessage}}", _settings.WelcomeMessage)
                .Replace("{{Password}}", password)
                .Replace("{{Footer}}", _settings.Footer);

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_settings.Username, _settings.FromName),
                Subject = _settings.Subject,
                Body = htmlBody,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
