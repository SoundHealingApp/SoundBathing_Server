using FluentValidation;
using SoundHealing.Application.Contracts.Requests.Auth;

namespace SoundHealing.Application.Validators.Auth;

public class ChangeCredentialsRequestValidator : AbstractValidator<ChangeCredentialsRequest>
{
    public ChangeCredentialsRequestValidator()
    {
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email must not be empty.")
            .EmailAddress().WithMessage("Invalid email format.")
            .When(x => x.Email != null);

        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Password must not be empty.")
            .Length(6, 20).WithMessage("Password must be between 6 and 20 characters.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.")
            .When(x => x.Password != null);
    }
}