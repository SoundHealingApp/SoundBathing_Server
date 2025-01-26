using CQRS;
using MediatR;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Application.Commands.Quotes;

public record CreateQuoteCommand(string Text, string Author) : IRequest<Result<Unit>>;

internal sealed class CreateQuoteCommandHandler(IQuoteRepository quoteRepository) : IRequestHandler<CreateQuoteCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(CreateQuoteCommand request, CancellationToken cancellationToken)
    {
        await quoteRepository.AddAsync(new Quote(request.Text.Trim(), request.Author), cancellationToken);
        
        return Unit.Value;
    }
}