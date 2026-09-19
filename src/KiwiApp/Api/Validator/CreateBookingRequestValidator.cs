using FluentValidation;
using KiwiApp.Api.Contracts;

namespace KiwiApp.Api.Validator;

public class CreateBookingRequestValidator : AbstractValidator<CreateBookingRequest>
{
    public CreateBookingRequestValidator()
    {
        RuleFor(x => x.Passengers).NotEmpty().WithMessage("Passengers are required");
        RuleFor(x => x.Email).NotEmpty().MaximumLength(50).EmailAddress();
    }
}