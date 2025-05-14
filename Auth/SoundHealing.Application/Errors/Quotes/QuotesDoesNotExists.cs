using CQRS;
using SoundHealing.Application.Interfaces;

namespace SoundHealing.Application.Errors.Quotes;

public class QuotesDoesNotExists()
    : ErrorResponse("Quotes does not exists");