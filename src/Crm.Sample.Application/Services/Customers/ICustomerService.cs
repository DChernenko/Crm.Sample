using Crm.Sample.Application.Dtos.Customers;
using Crm.Sample.Application.Services.Base;
using Crm.Sample.Domain.Entities.Customers;

namespace Crm.Sample.Application.Services.Customers
{
    public interface ICustomerService : IBaseService<Customer, CreateCustomerDto, UpdateCustomerDto, CustomerDto>
    { }
}
