using CQRS;
using SoundHealing.Application.Interfaces;

namespace SoundHealing.Application.Errors.LiveStreamErrors;

public class LiveStreamWithIdDoesNotExistsError(Guid liveStreamId)
    : ErrorResponse($"Live stream with id '{liveStreamId}' does not exists.");