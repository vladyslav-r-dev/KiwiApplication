using FluentValidation;
using KiwiApp.Contracts;

namespace KiwiApp.Validator;

public class CreateBookingRequestValidator : AbstractValidator<CreateBookingRequest>
{
    public CreateBookingRequestValidator()
    {
        RuleFor(x => x.Passengers).NotEmpty().WithMessage("Passengers are required");
        RuleFor(x => x.Email).NotEmpty().MaximumLength(50).EmailAddress();
    }
}