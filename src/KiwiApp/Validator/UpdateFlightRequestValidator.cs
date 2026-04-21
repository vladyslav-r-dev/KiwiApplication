using FluentValidation;
using KiwiApp.Contracts;

namespace KiwiApp.Validator;

public class UpdateFlightRequestValidator : AbstractValidator<UpdateFlightRequest>
{
    public UpdateFlightRequestValidator()
    {
        RuleFor(x => x.From).NotEmpty().WithMessage("From is required");
        RuleFor(x => x.To).NotEmpty().WithMessage("To is required");
    }
}