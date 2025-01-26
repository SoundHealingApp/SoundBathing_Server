using CQRS;

namespace SoundHealing.Application.Errors.Quotes;

public class QuotesDoesNotExists()
    : ErrorResponse("Quotes does not exists");