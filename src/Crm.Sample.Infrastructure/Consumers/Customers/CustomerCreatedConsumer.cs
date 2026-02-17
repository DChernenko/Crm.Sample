using Crm.Sample.Application.Common.Interfaces;
using Crm.Sample.Domain.Events.Customers;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Crm.Sample.Infrastructure.Consumers.Customers
{
    public class CustomerCreatedConsumer : IConsumer<CustomerCreatedEvent>
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<CustomerCreatedConsumer> _logger;

        public CustomerCreatedConsumer(IEmailService emailService, ILogger<CustomerCreatedConsumer> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CustomerCreatedEvent> context)
        {
            var message = context.Message;

            _logger.LogInformation("Sending welcome email to {Email} for customer {CustomerId}",
                message.Email, message.CustomerId);

            var subject = "Welcome";
            var body = $@"
            <h1>Hello {message.FullName},</h1>
            <p>Thank you for registering with Library Manager. We're excited to have you on board!</p>";

            await _emailService.SendEmailAsync(message.Email, subject, body);

            _logger.LogInformation("Welcome email sent successfully");
        }
    }
}
