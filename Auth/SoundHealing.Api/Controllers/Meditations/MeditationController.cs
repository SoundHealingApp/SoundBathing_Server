using System.Net;
using CQRS;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SoundHealing.Application.Commands.Meditations;
using SoundHealing.Application.Contracts.Requests.Meditation;
using SoundHealing.Application.Errors.MeditationErrors;
using SoundHealing.Core.Enums;

namespace SoundHealing.Controllers.Meditations;

[ApiController]
[Route("api/meditations")]
public class MeditationController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Добавить новую медитацию
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> AddMeditationAsync(
        [FromBody] AddMeditationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new AddMeditationCommand(request), cancellationToken);

        return result switch
        {
            { IsSuccess: true } => Ok(),
            { ErrorResponse: MeditationAlreadyExistsError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.Conflict),
            _ => throw new UnexpectedErrorResponseException()
        };
    }

    /// <summary>
    /// Получить список медитаций по типу
    /// </summary>
    [HttpGet("type/{meditationType}")]
    public async Task<IActionResult> GetMeditationsByTypeAsync(
        [FromRoute] MeditationType meditationType,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetMeditationsCommand(meditationType), cancellationToken);
        
        return result switch
        {
            { IsSuccess: true } => Ok(result.Data),
            { ErrorResponse: MeditationsDoesNotExists err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.NotFound),
            _ => throw new UnexpectedErrorResponseException()
        }; 
    }

    /// <summary>
    /// Получить информацию о медитации по ID
    /// </summary>
    [HttpGet("{meditationId}")]
    public async Task<IActionResult> GetMeditationInfoByIdAsync(
        [FromRoute] Guid meditationId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetMeditationInfoByIdCommand(meditationId), cancellationToken);
        
        return result switch
        {
            { IsSuccess: true } => Ok(result.Data),
            { ErrorResponse: MeditationWithIdDoesNotExists err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.NotFound),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
}