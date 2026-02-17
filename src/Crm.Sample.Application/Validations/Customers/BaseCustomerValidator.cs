using Crm.Sample.Application.Dtos.Customers;
using FluentValidation;

namespace Crm.Sample.Application.Validations.Customers
{
    public abstract class BaseCustomerValidator<T> : AbstractValidator<T>
        where T : BaseCustomerDto
    {
        protected BaseCustomerValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(100).WithMessage("First name must not exceed 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(100).WithMessage("Last name must not exceed 50 characters.");
        }
    }
}
