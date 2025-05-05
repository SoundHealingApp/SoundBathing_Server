using SoundHealing.Extensions;

namespace SoundHealing.Application.Errors.MeditationErrors;

public class MeditationAlreadyExistsError(string title)
    : ErrorResponse($"Meditation with title {title} already exists.");