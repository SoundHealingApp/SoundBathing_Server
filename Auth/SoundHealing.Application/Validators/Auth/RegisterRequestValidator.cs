using FluentValidation;
using SoundHealing.Application.Contracts.Requests.Auth;

namespace SoundHealing.Application.Validators.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email must not be empty.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}