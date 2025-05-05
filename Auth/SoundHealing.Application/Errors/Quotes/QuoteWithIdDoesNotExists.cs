using SoundHealing.Extensions;

namespace SoundHealing.Application.Errors.Quotes;

public class QuoteWithIdDoesNotExists(Guid id)
    : ErrorResponse($"Quote with id {id} does not exists");