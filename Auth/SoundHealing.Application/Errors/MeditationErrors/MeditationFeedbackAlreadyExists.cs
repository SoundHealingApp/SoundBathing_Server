using CQRS;

namespace SoundHealing.Application.Errors.MeditationErrors;

public class UserAlreadyProvidedFeedbackError(Guid userId, Guid meditationId)
    : ErrorResponse($"User with ID '{userId}' has already provided feedback for meditation with ID '{meditationId}'.");
