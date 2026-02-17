using Crm.Sample.Application.Validations.Customers;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Sample.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddValidatorsFromAssemblyContaining<CreateCustomerDtoValidator>();

            return services;
        }
    }
}