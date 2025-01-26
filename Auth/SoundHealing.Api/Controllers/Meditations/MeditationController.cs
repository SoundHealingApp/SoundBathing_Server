using System.Net;
using CQRS;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SoundHealing.Application.Commands.Meditations;
using SoundHealing.Application.Commands.Meditations.FilesCommands;
using SoundHealing.Application.Contracts.Requests.Meditation;
using SoundHealing.Application.Errors.MeditationErrors;
using SoundHealing.Application.Errors.S3Errors;
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
    public async Task<IActionResult> AddAsync(
        [FromForm] AddMeditationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new AddMeditationCommand(request), cancellationToken);

        return result switch
        {
            { IsSuccess: true } => Ok(),
            { ErrorResponse: MeditationAlreadyExistsError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.Conflict),
            { ErrorResponse: S3ImageUploadError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.InternalServerError),
            { ErrorResponse: S3AudioUploadError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.InternalServerError),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
    
    /// <summary>
    /// Изменить данные медитации.
    /// </summary>
    [HttpPatch("{meditationId:guid}")]
    public async Task<IActionResult> ChangeDataAsync(
        [FromRoute] Guid meditationId,
        [FromForm] EditMeditationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new EditMeditationCommand(
                meditationId,
                request.Title,
                request.Description,
                request.MeditationType,
                request.TherapeuticPurpose,
                request.Image,
                request.Audio,
                request.Frequency), cancellationToken);

        return result switch
        {
            { IsSuccess: true } => Ok(),
            { ErrorResponse: MeditationWithIdDoesNotExistsError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.NotFound),
            { ErrorResponse: S3ImageUploadError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.InternalServerError),
            { ErrorResponse: S3AudioUploadError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.InternalServerError),
            _ => throw new UnexpectedErrorResponseException()
        };
    }

    /// <summary>
    /// Получить список медитаций по типу
    /// </summary>
    [HttpGet("type/{meditationType}")]
    public async Task<IActionResult> GetByTypeAsync(
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
    [HttpGet("{meditationId:guid}")]
    public async Task<IActionResult> GetInfoByIdAsync(
        [FromRoute] Guid meditationId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetMeditationInfoByIdCommand(meditationId), cancellationToken);
        
        return result switch
        {
            { IsSuccess: true } => Ok(result.Data),
            { ErrorResponse: MeditationWithIdDoesNotExistsError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.NotFound),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
    
    /// <summary>
    /// Удалить медитацию по Id.
    /// </summary>
    [HttpDelete("{meditationId:guid}")]
    public async Task<IActionResult> DeleteByIdAsync(
        [FromRoute] Guid meditationId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new DeleteMeditationByIdCommand(meditationId), cancellationToken);
        
        return result switch
        {
            { IsSuccess: true } => Ok(result.Data),
            { ErrorResponse: MeditationWithIdDoesNotExistsError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.NotFound),
            _ => throw new UnexpectedErrorResponseException()
        };
    }

    /// <summary>
    /// Скачать изображение медитации
    /// </summary>
    [HttpGet("{meditationId:guid}/image")]
    public async Task<IResult> DownloadImageAsync(
        [FromRoute] Guid meditationId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetMeditationImageCommand(meditationId), cancellationToken);
        
        return result;
    }
    
    /// <summary>
    /// Скачать аудио медитации
    /// </summary>
    [HttpGet("{meditationId:guid}/audio")]
    public async Task<IResult> DownloadAudioAsync(
        [FromRoute] Guid meditationId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetMeditationAudioCommand(meditationId), cancellationToken);
        
        return result;
    }
}