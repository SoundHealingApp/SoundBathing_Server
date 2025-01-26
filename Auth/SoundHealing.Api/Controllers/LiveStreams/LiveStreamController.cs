using System.Globalization;
using System.Net;
using CQRS;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SoundHealing.Application.Commands.LiveStreams;
using SoundHealing.Application.Contracts.Requests.LiveStreams;
using SoundHealing.Application.Errors.LiveStreamErrors;

namespace SoundHealing.Controllers.LiveStreams;

[ApiController]
[Route("livestreams")]
public class LiveStreamController(IMediator mediator) : ControllerBase
{
    // TODO: Для админа: 1) получить прошедшие стримы, 2) получить предстоящие стримы
    
    [HttpPost]
    public async Task<IActionResult> Add(
        [FromBody] AddLiveStreamRequest request, 
        CancellationToken cancellationToken)
    {
        if (!DateTime.TryParseExact(
                request.StartDateTime, 
                "dd-MM-yyyy HH:mm", 
                CultureInfo.InvariantCulture, 
                DateTimeStyles.None, 
                out var parsedStartDateTime))
        {
            return BadRequest("Invalid date format. Use 'yyyy-MM-dd HH:mm'.");
        }
        
        var result = await mediator.Send(
            new AddLiveStreamCommand(
                request.Title,
                request.Description,
                request.TherapeuticPurpose,
                DateTime.SpecifyKind(parsedStartDateTime, DateTimeKind.Utc),
                request.YouTubeUrl), cancellationToken);
        
        return result switch
        {
            { IsSuccess: true } => Ok(),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
    
    /// <summary>
    /// Получить ближайшую предстоящую трансляцию (или null) - для юзера
    /// </summary>
    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcoming(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetUpcomingStreamsCommand(), cancellationToken);
        
        return result switch
        {
            { IsSuccess: true } => Ok(result.Data),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
    
    /// <summary>
    /// Получить все существующие трансляции (для админа).
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllStreamsCommand(), cancellationToken);
        
        return result switch
        {
            { IsSuccess: true } => Ok(result.Data),
            _ => throw new UnexpectedErrorResponseException()
        };
    }

    /// <summary>
    /// Обновить данные трансляции
    /// </summary>
    [HttpPatch("{liveStreamId:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid liveStreamId,
        [FromBody] EditLiveStreamRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new EditLiveStreamCommand(
                liveStreamId,
                request.Title,
                request.Description,
                request.TherapeuticPurpose,
                request.StartDateTime,
                request.YouTubeUrl),
            cancellationToken);
        
        return result switch
        {
            { IsSuccess: true } => Ok(),
            { ErrorResponse: LiveStreamWithIdDoesNotExistsError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.NotFound),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
    
    /// <summary>
    /// Удалить трансляцию.
    /// </summary>
    [HttpDelete("{liveStreamId:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid liveStreamId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new DeleteLiveStreamCommand(liveStreamId), 
            cancellationToken);
        
        return result switch
        {
            { IsSuccess: true } => Ok(),
            { ErrorResponse: LiveStreamWithIdDoesNotExistsError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.NotFound),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
}