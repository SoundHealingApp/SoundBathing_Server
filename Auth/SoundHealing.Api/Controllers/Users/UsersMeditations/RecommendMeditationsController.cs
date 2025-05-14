using System.Net;
using CQRS;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoundHealing.Application.Commands.UserMeditations.RecommendMeditationsCommands;
using SoundHealing.Application.Errors.MeditationErrors;
using SoundHealing.Application.Errors.UsersErrors;
using SoundHealing.Application.Interfaces;
using SoundHealing.Core;
using SoundHealing.Extensions;

namespace SoundHealing.Controllers.Users.UsersMeditations;

[ApiController]
[Route("api/users/{userId:guid}/recommended-meditations")]
public class RecommendMeditationsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Добавить медитации в рекоммендованные.
    /// </summary>
    [HttpPost]
    [Authorize(PermissionsConstants.ManageMeditationsRecommendations)]
    public async Task<IActionResult> RecommendMeditation(
        [FromRoute] Guid userId,
        [FromQuery] List<Guid> meditationId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new RecommendMeditationCommand(userId, meditationId), cancellationToken);

        return result switch
        {
            { IsSuccess: true } => Ok(),
            { ErrorResponse: UserWithIdNotFoundError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.NotFound),
            { ErrorResponse: GivenMeditationsDoesNotExists err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.NotFound),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
    
    /// <summary>
    /// Получить рекомендованные медитации.
    /// </summary>
    [HttpGet]
    [Authorize(PermissionsConstants.ManageMeditationsRecommendations)]
    public async Task<IActionResult> GetRecommendMeditation(
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetRecommendedMeditationsCommand(userId), cancellationToken);

        return result switch
        {
            { IsSuccess: true } => Ok(result.Data),
            { ErrorResponse: UserWithIdNotFoundError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.NotFound),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
}