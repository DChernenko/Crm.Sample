using Crm.Sample.Application.Abstractions.Customers;
using Crm.Sample.Application.Common.Interfaces;
using Crm.Sample.Domain.Repositories.Customers;
using Crm.Sample.Infrastructure.Options;
using Crm.Sample.Infrastructure.Persistence;
using Crm.Sample.Infrastructure.Repositories.Customers;
using Crm.Sample.Infrastructure.Services.Emails;
using Crm.Sample.Infrastructure.Services.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Crm.Sample.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Options

            services.Configure<MsSqlDbOptions>(configuration.GetSection(nameof(MsSqlDbOptions)));
            services.Configure<RedisOptions>(configuration.GetSection(nameof(RedisOptions)));
            services.Configure<RabbitMqOptions>(configuration.GetSection(nameof(RabbitMqOptions)));
            services.Configure<EmailOptions>(configuration.GetSection(nameof(EmailOptions)));
            services.Configure<CronJobsOptions>(configuration.GetSection(nameof(CronJobsOptions)));
            #endregion Options

            var msSqlSettings = configuration.GetSection("MsSqlDbOptions").Get<MsSqlDbOptions>()
                ?? throw new InvalidOperationException("Database connection string not configured");

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(msSqlSettings.ConnectionString));

            //Redis 
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var RedisOptions = configuration.GetSection("RedisOptions").Get<RedisOptions>()
                ?? throw new InvalidOperationException("Redis connection string not configured"); ;

                return ConnectionMultiplexer.Connect(RedisOptions.ConnectionString);
            });

            // Register repositories
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Services
            services.AddScoped<IEmailService, EmailService>();

            //Redis 
            services.AddScoped<IApplicationCach, RedisCache>();

            return services;
        }
    }
}
