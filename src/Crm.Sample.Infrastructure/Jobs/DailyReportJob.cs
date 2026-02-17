using Crm.Sample.Application.Common.Interfaces;
using Crm.Sample.Domain.Repositories.Customers;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Crm.Sample.Infrastructure.Jobs
{
    public class DailyReportJob : IJob
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<DailyReportJob> _logger;

        public DailyReportJob(
            ICustomerRepository customerRepository,
            IEmailService emailService,
            ILogger<DailyReportJob> logger)
        {
            _customerRepository = customerRepository;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Daily report job started");

            var newCustomersCount = await _customerRepository.CountNewCustomersAsync(DateTime.UtcNow.AddDays(-1));

            var subject = "Daily Report";
            var body = $"<h1>Daily Report</h1><p>New customers in last 24h: {newCustomersCount}</p>";
            await _emailService.SendEmailAsync("admin@library.com", subject, body);

            _logger.LogInformation("Daily report job completed");
        }
    }
}
