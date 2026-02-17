using Crm.Sample.Application.Common.Interfaces;
using Crm.Sample.Application.Services.Customers;
using Crm.Sample.Application.Services.Redis;
using Crm.Sample.Domain.Repositories.Customers;
using Crm.Sample.Infrastructure.Options;
using Crm.Sample.Infrastructure.Persistence;
using Crm.Sample.Infrastructure.Repositories.Customers;
using Crm.Sample.Infrastructure.Services.Customers;
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

            // Configure MongoDB settings
            //services.Configure<MongoDbSettings>(
            //    configuration.GetSection("MongoDBSettings"));

            var msSqlSettings = configuration.GetSection("MsSqlDbOptions").Get<MsSqlDbOptions>()
                ?? throw new InvalidOperationException("Database connection string not configured");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(msSqlSettings.ConnectionString));

            // Register MongoDB context
            //services.AddSingleton<MongoDBContext>();
            services.AddDbContext<AppDbContext>();

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
            //services.AddScoped<IOrderRepository, OrderRepository>();

            // Services
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IEmailService, EmailService>();

            //Redis 
            services.AddScoped<IRedisCache, RedisCache>();

            return services;
        }
    }
}
