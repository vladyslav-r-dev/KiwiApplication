using FluentValidation;
using KiwiApp.Api.Contracts;

namespace KiwiApp.Api.Validator;

public class UpdateFlightRequestValidator : AbstractValidator<UpdateFlightRequest>
{
    public UpdateFlightRequestValidator()
    {
        RuleFor(x => x.From).NotEmpty().WithMessage("From is required");
        RuleFor(x => x.To).NotEmpty().WithMessage("To is required");
    }
}