using CQRS;
using SoundHealing.Application.Interfaces;

namespace SoundHealing.Application.Errors.Quotes;

public class QuoteWithIdDoesNotExists(Guid id)
    : ErrorResponse($"Quote with id {id} does not exists");