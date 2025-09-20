using Skill_Exchange.Application.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
namespace Skill_Exchange.Infrastructure.AuthenticationServices
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSetting;
        public EmailService(SmtpSettings smtpSettings)
        {
            _smtpSetting = smtpSettings;
        }
        public async Task SendEmailAsync(string email, string content)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtpSetting.SenderName, _smtpSetting.Username));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = "Verify your email";
            message.Body = new TextPart("html")
            {
                Text = $"<p>Please verify your email by clicking <a href='{content}'>here</a></p>"
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpSetting.Host, _smtpSetting.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_smtpSetting.SenderEmail, _smtpSetting.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}