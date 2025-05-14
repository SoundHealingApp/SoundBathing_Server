using System.Net;
using CQRS;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoundHealing.Application.Commands.UserMeditations.LikeMeditationsCommand;
using SoundHealing.Application.Errors.MeditationErrors;
using SoundHealing.Application.Errors.UsersErrors;
using SoundHealing.Application.Interfaces;
using SoundHealing.Core;
using SoundHealing.Extensions;

namespace SoundHealing.Controllers.Users.UsersMeditations;

[ApiController]
[Route("users/{userId:guid}/liked-meditations")]
internal class LikeMeditationController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Добавить медитацию в понравившуюся.
    /// </summary>
    [HttpPost("{meditationId:guid}")]
    [Authorize(PermissionsConstants.ManageMeditationsLikes)]
    public async Task<IActionResult> LikeMeditation(
        [FromRoute] Guid userId,
        [FromRoute] Guid meditationId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new LikeMeditationCommand(userId, meditationId), cancellationToken);

        return result switch
        {
            { IsSuccess: true } => Ok(),
            { ErrorResponse: UserWithIdNotFoundError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.NotFound),
            { ErrorResponse: MeditationWithIdDoesNotExistsError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.NotFound),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
    
    /// <summary>
    /// Удалить медитацию из понравившихся
    /// </summary>
    [HttpDelete("{meditationId:guid}")]
    [Authorize(PermissionsConstants.ManageMeditationsLikes)]
    public async Task<IActionResult> DeleteLikeFromMeditation(
        [FromRoute] Guid userId,
        [FromRoute] Guid meditationId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new DeleteLikeFromMeditationCommand(userId, meditationId), cancellationToken);

        return result switch
        {
            { IsSuccess: true } => Ok(),
            { ErrorResponse: UserWithIdNotFoundError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.NotFound),
            { ErrorResponse: MeditationWithIdDoesNotExistsError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.NotFound),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
    
    /// <summary>
    /// Получить понравившиеся медитации.
    /// </summary>
    [HttpGet]
    [Authorize(PermissionsConstants.ManageMeditationsLikes)]
    public async Task<IActionResult> GetLikedMeditations(
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetLikedMeditationsCommand(userId), cancellationToken);
        
        return result switch
        {
            { IsSuccess: true } => Ok(result.Data),
            { ErrorResponse: UserWithIdNotFoundError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.NotFound),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
}