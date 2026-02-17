using Crm.Sample.Infrastructure.Jobs;
using Crm.Sample.Infrastructure.Options;
using Quartz;

namespace Crm.Sample.Api.Extensions
{
    public static class QuartzServiceExtensions
    {
        public static IServiceCollection AddQuartzJobs(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.GetSection(nameof(CronJobsOptions)).Get<CronJobsOptions>()
				?? throw new InvalidOperationException($"Failed to bind {nameof(CronJobsOptions)} from configuration.");

			services.AddQuartz(quartz =>
            {
                var jobKey = new JobKey(nameof(DailyReportJob));
                quartz.AddJob<DailyReportJob>(opts => opts.WithIdentity(jobKey));

                quartz.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("DailyReportJobTrigger")
                    .WithCronSchedule(options.DailyReportJob, cron => cron
                        .InTimeZone(TimeZoneInfo.Utc)
                        .WithMisfireHandlingInstructionDoNothing()
                    ));
            });

            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });

            return services;
        }
    }
}
