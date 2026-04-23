using FluentValidation;
using KiwiApp.DTOs;

namespace KiwiApp.Validator;

public class RegistrationValidator : AbstractValidator<RegisterUserDto>
{
    public RegistrationValidator()
    {
        RuleFor(x => x.Password).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Email).NotEmpty().MaximumLength(50).EmailAddress();
    }
}