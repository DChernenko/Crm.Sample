using Crm.Sample.Application.Common.Interfaces;
using MassTransit;

namespace Crm.Sample.Infrastructure.MessageBus
{
    public class MassTransitMessageBus : IMessageBus
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public MassTransitMessageBus(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) 
            where T : class
           => _publishEndpoint.Publish(message, cancellationToken);
    }
}
