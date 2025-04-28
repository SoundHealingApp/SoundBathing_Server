using FluentValidation;
using SoundHealing.Application.Contracts.Requests.Meditation;

namespace SoundHealing.Application.Validators.Meditation;

public class ChangeFeedbackRequestValidator : AbstractValidator<ChangeMeditationFeedbackRequest>
{
    public ChangeFeedbackRequestValidator()
    {
        RuleFor(x => x.Estimate)
            .Cascade(CascadeMode.Stop)
            .LessThan(6)
            .GreaterThan(0);
    }
}