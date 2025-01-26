using CQRS;
using MediatR;
using SoundHealing.Application.Errors.Quotes;
using SoundHealing.Core.Interfaces;

namespace SoundHealing.Application.Commands.Quotes;

public record DeleteQuoteCommand(Guid QuoteId) : IRequest<Result<Unit>>;

internal sealed class DeleteQuoteCommandHandler(IQuoteRepository quoteRepository)
    : IRequestHandler<DeleteQuoteCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteQuoteCommand request, CancellationToken cancellationToken)
    {
        var quote = await quoteRepository.GetByIdAsync(request.QuoteId, cancellationToken);
        
        if (quote == null)
            return new QuoteWithIdDoesNotExists(request.QuoteId);
        
        await quoteRepository.DeleteAsync(quote, cancellationToken);
        
        return Unit.Value;
    }
}