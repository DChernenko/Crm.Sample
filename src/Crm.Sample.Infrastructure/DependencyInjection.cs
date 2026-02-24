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

            services.Configure<ConnectionStrings>(configuration.GetSection(nameof(ConnectionStrings)));
            services.Configure<RedisOptions>(configuration.GetSection(nameof(RedisOptions)));
            services.Configure<RabbitMqOptions>(configuration.GetSection(nameof(RabbitMqOptions)));
            services.Configure<EmailOptions>(configuration.GetSection(nameof(EmailOptions)));
            services.Configure<CronJobsOptions>(configuration.GetSection(nameof(CronJobsOptions)));
            #endregion Options

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("MsSqlConnection")));

            //Redis 
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var redisOptions = configuration.GetSection("RedisOptions").Get<RedisOptions>()
                ?? throw new InvalidOperationException("Redis connection string not configured"); ;

                return ConnectionMultiplexer.Connect(redisOptions.ConnectionString);
            });

            // Register repositories
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Services
            services.AddScoped<IEmailService, EmailService>();

            //Redis 
            services.AddScoped<IApplicationCache, RedisCache>();

            return services;
        }
    }
}
