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
        public async Task<bool> SendEmailAsync(string email, string content)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_smtpSetting.SenderName, _smtpSetting.Username));
                message.To.Add(MailboxAddress.Parse(email));
                message.Subject = "Verify your email";
                message.Body = new TextPart("html")
                {
                    Text = $"<p>Verification Code : {content}</p>"
                };

                using var client = new SmtpClient();
                await client.ConnectAsync(_smtpSetting.Host, _smtpSetting.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpSetting.SenderEmail, _smtpSetting.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                return true; // success
            }
            catch (Exception ex)
            {
                return false; // failed
            }
        }

    }
}