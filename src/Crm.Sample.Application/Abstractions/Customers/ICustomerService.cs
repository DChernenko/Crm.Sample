using Crm.Sample.Application.Abstractions.Base;
using Crm.Sample.Application.Dtos.Customers;
using Crm.Sample.Domain.Entities.Customers;

namespace Crm.Sample.Application.Abstractions.Customers
{
    public interface ICustomerService : IBaseService<Customer, CreateCustomerDto, UpdateCustomerDto, CustomerDto>
    { }
}
