using CQRS;
using SoundHealing.Application.Interfaces;

namespace SoundHealing.Application.Errors.MeditationErrors;

public class MeditationAlreadyExistsError(string title)
    : ErrorResponse($"Meditation with title {title} already exists.");