using MediatR;
using Microsoft.AspNetCore.Http;
using SoundHealing.Core.Interfaces;

namespace SoundHealing.Application.Commands.Meditations.FilesCommands;

public record GetMeditationImageCommand(Guid meditationId) : IRequest<IResult>;

public class GetMeditationImageCommandHandler(IS3Repository s3Repository)
    : IRequestHandler<GetMeditationImageCommand, IResult>
{
    public async Task<IResult> Handle(GetMeditationImageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await s3Repository.GetMeditationImageAsync(request.meditationId);
            
            if (response is null)
                return Results.NotFound($"Meditation image with ID {request.meditationId} not found.");
            
            return Results.File(
                response.ResponseStream,
                response.Headers.ContentType,
                response.Metadata["file-name"]);
        }
        catch (Exception ex)
        {
            return Results.InternalServerError($"An error occurred while downloading meditation image: {ex.Message}");
        }
    }
}