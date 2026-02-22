using Crm.Sample.Application.Common.Interfaces;
using Crm.Sample.Infrastructure.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Crm.Sample.Infrastructure.Services.Emails
{
    public class EmailService : IEmailService
    {
        private readonly EmailOptions _emailOptions;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptionsSnapshot<EmailOptions> EmailOptions, ILogger<EmailService> logger)
        {
            _emailOptions = EmailOptions.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true, CancellationToken cancellationToken = default)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_emailOptions.SenderName, _emailOptions.SenderEmail));
                email.To.Add(new MailboxAddress("", to));
                email.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                if (isHtml)
                    bodyBuilder.HtmlBody = body;
                else
                    bodyBuilder.TextBody = body;

                email.Body = bodyBuilder.ToMessageBody();

                using var smtp = new SmtpClient();

            #if DEBUG
                await smtp.ConnectAsync(_emailOptions.SmtpServer, _emailOptions.SmtpPort, SecureSocketOptions.None);
            #else
                await smtp.ConnectAsync(_EmailOptions.SmtpServer, _EmailOptions.SmtpPort,
                    _EmailOptions.UseSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls, cancellationToken);
            #endif

                if (!string.IsNullOrEmpty(_emailOptions.SmtpUsername))
                    await smtp.AuthenticateAsync(_emailOptions.SmtpUsername, _emailOptions.SmtpPassword, cancellationToken);

                await smtp.SendAsync(email, cancellationToken);
                await smtp.DisconnectAsync(true, cancellationToken);

                _logger.LogInformation("Email sent successfully to {Email}", to);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email}", to);
                throw;
            }
        }
    }
}
