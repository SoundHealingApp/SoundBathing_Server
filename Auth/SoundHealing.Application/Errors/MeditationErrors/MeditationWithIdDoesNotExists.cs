using CQRS;

namespace SoundHealing.Application.Errors.MeditationErrors;

public class MeditationWithIdDoesNotExists(Guid meditationId)
    : ErrorResponse($"Meditation with Id {meditationId} does not exists.");