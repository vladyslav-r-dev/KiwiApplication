using FluentValidation;
using KiwiApp.Contracts;

namespace KiwiApp.Validator;

public class UpdateBookingRequestValidator : AbstractValidator<UpdateBookingRequest>
{
    public UpdateBookingRequestValidator()
    {
        RuleFor(x => x.Passengers).NotEmpty().WithMessage("Passengers are required");
        RuleFor(x => x.Email).NotEmpty().MaximumLength(50).EmailAddress();
    }
}