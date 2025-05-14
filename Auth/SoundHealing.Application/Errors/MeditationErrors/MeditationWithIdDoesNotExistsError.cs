using CQRS;
using SoundHealing.Application.Interfaces;

namespace SoundHealing.Application.Errors.MeditationErrors;

public class MeditationWithIdDoesNotExistsError(Guid meditationId)
    : ErrorResponse($"Meditation with Id {meditationId} does not exists.");