using SoundHealing.Extensions;
using MediatR;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Application.Commands.Quotes;

public record GetAllQuotesCommand : IRequest<Result<IEnumerable<Quote>>>;

internal sealed class GetAllQuotesCommandHandler(IQuoteRepository quoteRepository)
    : IRequestHandler<GetAllQuotesCommand, Result<IEnumerable<Quote>>>
{
    public async Task<Result<IEnumerable<Quote>>> Handle(
        GetAllQuotesCommand request, CancellationToken cancellationToken)
    {
        return await quoteRepository.GetAllAsync(cancellationToken);
    }
}