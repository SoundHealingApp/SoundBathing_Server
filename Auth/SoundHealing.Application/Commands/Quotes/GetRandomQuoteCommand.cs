using CQRS;
using MediatR;
using SoundHealing.Application.Errors.Quotes;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Application.Commands.Quotes;

public record GetRandomQuoteCommand : IRequest<Result<Quote>>;

internal sealed class GetRandomQuoteCommandHandler(IQuoteRepository quoteRepository)
    : IRequestHandler<GetRandomQuoteCommand, Result<Quote>>
{
    public async Task<Result<Quote>> Handle(GetRandomQuoteCommand request, CancellationToken cancellationToken)
    {
       var randomQuote = await quoteRepository.GetRandomAsync(cancellationToken);

       if (randomQuote is null)
           return new QuotesDoesNotExists();
       
       return randomQuote;
    }
}