namespace Crm.Sample.Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = true, CancellationToken cancellationToken = default);
    }
}
