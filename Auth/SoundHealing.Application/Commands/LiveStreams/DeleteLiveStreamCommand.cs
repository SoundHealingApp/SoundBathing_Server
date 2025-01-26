using CQRS;
using MediatR;
using SoundHealing.Application.Errors.LiveStreamErrors;
using SoundHealing.Core.Interfaces;

namespace SoundHealing.Application.Commands.LiveStreams;

public record DeleteLiveStreamCommand(Guid LiveStreamId) : IRequest<Result<Unit>>;

internal sealed class DeleteLiveStreamCommandHandler(ILiveStreamRepository liveStreamRepository) : IRequestHandler<DeleteLiveStreamCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteLiveStreamCommand request, CancellationToken cancellationToken)
    {
        var liveStream = await liveStreamRepository.GetAsync(request.LiveStreamId, cancellationToken);

        if (liveStream == null)
            return new LiveStreamWithIdDoesNotExistsError(request.LiveStreamId);
        
        await liveStreamRepository.DeleteAsync(liveStream, cancellationToken);
        
        return Unit.Value;
    }
}