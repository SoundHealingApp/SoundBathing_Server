using SoundHealing.Extensions;

namespace SoundHealing.Application.Errors.MeditationErrors;

public class MeditationsDoesNotExists()
    : ErrorResponse("There are no meditations here yet");