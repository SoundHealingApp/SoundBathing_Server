using Auth.Application.Contracts.Requests.Auth;
using FluentValidation;
using SoundHealing.Application.Contracts.Requests.Auth;

namespace SoundHealing.Application.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email must not be empty.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password must not be empty.")
            .Length(6, 20).WithMessage("Password must be between 6 and 20 characters.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
    }
}