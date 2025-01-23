using MediatR;
using Microsoft.AspNetCore.Http;
using SoundHealing.Core.Interfaces;

namespace SoundHealing.Application.Commands.Meditations.MeditationsFilesCommands;

public record GetMeditationAudioCommand(Guid meditationId) : IRequest<IResult>;

public class GetMeditationAudioCommandHandler(IS3Repository s3Repository)
    : IRequestHandler<GetMeditationAudioCommand, IResult>
{
    public async Task<IResult> Handle(GetMeditationAudioCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await s3Repository.GetMeditationAudioAsync(request.meditationId);
            
            if (response is null)
                return Results.NotFound($"Meditation audio with ID {request.meditationId} not found.");
            
            return Results.File(
                response.ResponseStream, 
                response.Headers.ContentType, 
                response.Metadata["file-name"]);
        }
        catch (Exception ex)
        {
            return Results.InternalServerError($"An error occurred while downloading meditation audio: {ex.Message}");
        }
    }
}