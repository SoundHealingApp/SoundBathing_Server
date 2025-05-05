using SoundHealing.Extensions;

namespace SoundHealing.Application.Errors.MeditationErrors;

public class GivenMeditationsDoesNotExists(List<Guid> meditationsIds)
    : ErrorResponse(
        meditationsIds.Count switch
        {
            0 => "No meditations were provided.",
            1 => $"There is no such meditation with id: {meditationsIds[0]}",
            _ => $"There are no such meditations with ids: {string.Join(", ", meditationsIds)}"
        });
