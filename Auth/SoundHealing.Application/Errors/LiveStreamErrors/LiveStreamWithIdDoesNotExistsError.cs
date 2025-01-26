using CQRS;

namespace SoundHealing.Application.Errors.LiveStreamErrors;

public class LiveStreamWithIdDoesNotExistsError(Guid liveStreamId)
    : ErrorResponse($"Live stream with id '{liveStreamId}' does not exists.");