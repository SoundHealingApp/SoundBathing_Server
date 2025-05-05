using SoundHealing.Extensions;
using MediatR;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Application.Commands.Quotes;

public record CreateQuoteCommand(string Text, string Author) : IRequest<Result<Guid>>;

internal sealed class CreateQuoteCommandHandler(IQuoteRepository quoteRepository) : IRequestHandler<CreateQuoteCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateQuoteCommand request, CancellationToken cancellationToken)
    {
        var quote = new Quote(request.Text.Trim(), request.Author);
        await quoteRepository.AddAsync(quote, cancellationToken);
        
        return quote.Id;
    }
}