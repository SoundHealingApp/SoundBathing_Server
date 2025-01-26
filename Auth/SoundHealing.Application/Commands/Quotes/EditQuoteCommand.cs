using CQRS;
using MediatR;
using SoundHealing.Application.Errors.Quotes;
using SoundHealing.Core.Interfaces;

namespace SoundHealing.Application.Commands.Quotes;

public record EditQuoteCommand(Guid quoteId, string? Text, string? Author) : IRequest<Result<Unit>>;

internal sealed class EditQuoteCommandHandler(IQuoteRepository quoteRepository) : IRequestHandler<EditQuoteCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(EditQuoteCommand request, CancellationToken cancellationToken)
    {
        var quote = await quoteRepository.GetByIdAsync(request.quoteId, cancellationToken);
        
        if (quote is null)
            return new QuoteWithIdDoesNotExists(request.quoteId);
        
        if (request.Text != null)
            quote.ChangeText(request.Text);
        
        if (request.Author != null)
            quote.ChangeAuthor(request.Author);
        
        await quoteRepository.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}