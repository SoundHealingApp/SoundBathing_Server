using FluentValidation;
using SoundHealing.Application.Contracts.Requests.Meditation;

namespace SoundHealing.Application.Validators.Meditation;

public class AddMeditationFeedbackRequestValidator : AbstractValidator<AddMeditationFeedbackRequest>
{
    public AddMeditationFeedbackRequestValidator()
    {
        RuleFor(x => x.Estimate)
            .Cascade(CascadeMode.Stop)
            .LessThan(6)
            .GreaterThan(0);
    }
}