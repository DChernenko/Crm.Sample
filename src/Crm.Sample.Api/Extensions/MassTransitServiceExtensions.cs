using Crm.Sample.Application.Common.Interfaces;
using Crm.Sample.Infrastructure.Consumers.Customers;
using Crm.Sample.Infrastructure.MessageBus;
using Crm.Sample.Infrastructure.Options;
using MassTransit;

namespace Crm.Sample.Api.Extensions
{
	public static class MassTransitServiceExtensions
	{
		public static IServiceCollection AddMassTransitWithRabbitMq(this IServiceCollection services, IConfiguration configuration)
		{
			var options = configuration.GetSection(nameof(RabbitMqOptions)).Get<RabbitMqOptions>()
				?? throw new InvalidOperationException($"Failed to bind {nameof(RabbitMqOptions)} from configuration.");

			services.AddMassTransit(busConfigurator =>
			{
				busConfigurator.AddConsumer<CustomerCreatedConsumer>();

				busConfigurator.UsingRabbitMq((context, cfg) =>
				{
					var host = options.Host;
					var virtualHost = options.VirtualHost;
					var username = options.Username;
					var password = options.Password;

					cfg.Host(host, virtualHost, h =>
					{
						h.Username(username);
						h.Password(password);
					});

					cfg.ConfigureEndpoints(context);
				});
			});

			services.AddScoped<IMessageBus, MassTransitMessageBus>();

			return services;
		}
	}
}
