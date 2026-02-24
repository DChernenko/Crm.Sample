using Crm.Sample.Application.Abstractions.Customers;
using Crm.Sample.Application.Common.Interfaces;
using Crm.Sample.Application.Dtos.Customers;
using Crm.Sample.Application.Services.Base;
using Crm.Sample.Domain.Entities.Customers;
using Crm.Sample.Domain.Events.Customers;
using Crm.Sample.Domain.Repositories.Customers;
using FluentValidation;

namespace Crm.Sample.Application.Services.Customers
{
    public class CustomerService : BaseService<Customer, CreateCustomerDto, UpdateCustomerDto, CustomerDto>, ICustomerService
    {
        private readonly IMessageBus _messageBus;

        public CustomerService(
            IMessageBus messageBus,
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork,
            IApplicationCache cache,
            IValidator<CreateCustomerDto> createValidator,
            IValidator<UpdateCustomerDto> updateValidator
            ) : base(customerRepository, unitOfWork, cache, createValidator, updateValidator)
        {
            _messageBus = messageBus;
        }

        public override async Task<CustomerDto> CreateAsync(CreateCustomerDto createDto, int userId, CancellationToken cancellationToken = default)
        {
            var result = await base.CreateAsync(createDto, userId, cancellationToken);

            await _messageBus.PublishAsync(new CustomerCreatedEvent
            {
                CustomerId = result.Id.Value,
                Email = createDto.Email,
                FullName = $"{createDto.FirstName} {createDto.LastName}"
            }, cancellationToken);

            return result;
        }

        protected override CustomerDto MapToResponse(Customer entity)
        {
            return new CustomerDto
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Phone = entity.Phone,
                Email = entity.Email,
                CreatedAt = entity.CreateDate
            };
        }

        protected override Customer MapToEntity(CreateCustomerDto createDto, int userId)
        {
            return new Customer
            {
                FirstName = createDto.FirstName,
                LastName = createDto.LastName,
                Phone = createDto.Phone,
                Email = createDto.Email,

                CreateDate = DateTime.Now,
                CreatorId = userId,
                ModifiedDate = DateTime.Now
            };
        }

        protected override void MapToEntity(UpdateCustomerDto updateDto, Customer entity, int userId)
        {
            entity.FirstName = updateDto.FirstName;
            entity.LastName = updateDto.LastName;
            entity.ModifierId = userId;
        }
    }
}
