using System.Globalization;
using System.Net;
using CQRS;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoundHealing.Application.Commands.LiveStreams;
using SoundHealing.Application.Contracts.Requests.LiveStreams;
using SoundHealing.Application.Errors.LiveStreamErrors;
using SoundHealing.Core;

namespace SoundHealing.Controllers.LiveStreams;

[ApiController]
[Route("api/livestreams")]
public class LiveStreamController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Добавить трансляцию
    /// </summary>
    [HttpPost]
    [Authorize(PermissionsConstants.LiveStreamsAdministration)]
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
            return BadRequest("Invalid date format. Use 'dd-MM-yyyy HH:mm'.");
        }
        
        // Конвертируем время в UTC и учитываем часовой пояс Лондона
        var utcStartDateTime = parsedStartDateTime.ToUniversalTime();
        
        var result = await mediator.Send(
            new AddLiveStreamCommand(
                request.Title,
                request.Description,
                request.TherapeuticPurpose,
                utcStartDateTime,
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
    [HttpGet("nearest")]
    [Authorize(PermissionsConstants.GetLiveStreamsInfo)]
    public async Task<IActionResult> GetNearest(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetNearestStreamCommand(), cancellationToken);
        
        return result switch
        {
            { IsSuccess: true } => Ok(result.Data),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
    
    /// <summary>
    /// Получить предстоящие трансляции (для админа).
    /// </summary>
    [HttpGet("upcoming")]
    [Authorize(PermissionsConstants.LiveStreamsAdministration)]
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
    /// Получить прошедшие трансляции (для админа).
    /// </summary>
    [HttpGet("past")]
    [Authorize(PermissionsConstants.LiveStreamsAdministration)]
    public async Task<IActionResult> GetPast(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetPastStreamsCommand(), cancellationToken);
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
    [Authorize(PermissionsConstants.LiveStreamsAdministration)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid liveStreamId,
        [FromBody] EditLiveStreamRequest request,
        CancellationToken cancellationToken)
    {
        if (!DateTime.TryParseExact(
                request.StartDateTime, 
                "dd-MM-yyyy HH:mm", 
                CultureInfo.InvariantCulture, 
                DateTimeStyles.None, 
                out var parsedStartDateTime))
        {
            return BadRequest("Invalid date format. Use 'dd-MM-yyyy HH:mm'.");
        }
        
        // Конвертируем время в UTC 
        var utcStartDateTime = parsedStartDateTime.ToUniversalTime();
        
        var result = await mediator.Send(
            new EditLiveStreamCommand(
                liveStreamId,
                request.Title,
                request.Description,
                request.TherapeuticPurpose,
                utcStartDateTime,
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
    [Authorize(PermissionsConstants.LiveStreamsAdministration)]
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