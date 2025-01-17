using System.Net;
using CQRS;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SoundHealing.Application.Commands.LikeMeditationsCommand;
using SoundHealing.Application.Commands.Meditations.LikeMeditationsCommand;
using SoundHealing.Application.Commands.UserData;
using SoundHealing.Application.Contracts.Requests.UserEdit;
using SoundHealing.Application.Errors.AuthErrors;
using SoundHealing.Application.Errors.UsersErrors;

namespace SoundHealing.Controllers.Users;

[ApiController]
[Route("users")]
public class UserController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddUserData(
        [FromBody] AddUserRequest command, 
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new AddUserCommand(command.UserCredentialsId, command.Name, command.Surname, command.BirthDate),
            cancellationToken);

        return result switch
        {
            { IsSuccess: true } => Ok(result.Data),
            { ErrorResponse: UserAlreadyExistsError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.Conflict),
            _ => throw new UnexpectedErrorResponseException()
        };
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserData(
        [FromRoute] string userId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetUserDataCommand(userId), cancellationToken);
        
        return result switch
        {
            { IsSuccess: true } => Ok(result.Data),
            { ErrorResponse: UserWithIdNotFoundError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.NotFound),
            { ErrorResponse: UserAlreadyExistsError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.Conflict),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
    
    /// <summary>
    /// Добавить медитацию в понравившуюся.
    /// </summary>
    [HttpPost("users/{userId}/meditations/{meditationId}/like")]
    public async Task<IActionResult> LikeMeditation(
        [FromRoute] Guid userId,
        [FromRoute] Guid meditationId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new LikeMeditationCommand(userId, meditationId), cancellationToken);

        return result switch
        {
            { IsSuccess: true } => Ok(result.Data),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
    
    /// <summary>
    /// Получить понравившиеся медитации.
    /// </summary>
    [HttpGet("users/{userId}/liked-meditations")]
    public async Task<IActionResult> GetLikedMeditations(
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetLikedMeditationsCommand(userId), cancellationToken);
        
        return result switch
        {
            { IsSuccess: true } => Ok(result.Data),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
}