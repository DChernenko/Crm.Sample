using Crm.Sample.Application.Dtos.Customers;
using FluentValidation;

namespace Crm.Sample.Application.Validations.Customers
{
    public class CreateCustomerDtoValidator : BaseCustomerValidator<CreateCustomerDto>
    {
        public CreateCustomerDtoValidator()
        {
            RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters.")
            .Matches(@"^\+?[0-9\s\-\(\)]+$").WithMessage("Invalid phone number format.");
        }
    }
}
