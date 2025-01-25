using System.Net;
using CQRS;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SoundHealing.Application.Commands.UserData;
using SoundHealing.Application.Contracts.Requests.User;
using SoundHealing.Application.Errors.AuthErrors;
using SoundHealing.Application.Errors.UsersErrors;

namespace SoundHealing.Controllers.Users.UserData;

[ApiController]
[Route("users")]
public class UserController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Добавить данные о пользователе.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> AddUserData(
        [FromBody] AddUserRequest command, 
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new AddUserCommand(command.UserId, command.Name, command.Surname, command.BirthDate),
            cancellationToken);

        return result switch
        {
            { IsSuccess: true } => Ok(),
            { ErrorResponse: UserAlreadyExistsError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.Conflict),
            _ => throw new UnexpectedErrorResponseException()
        };
    }

    /// <summary>
    /// Получить информацию о пользователе.
    /// </summary>
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
    /// Изменить данные пользователя
    /// </summary>
    [HttpPatch("{userId:guid}")]
    public async Task<IActionResult> ChangeUserData(
        [FromRoute] Guid userId,
        [FromBody] ChangeUserDataRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new ChangeUserDataCommand(userId, request.Name, request.Surname, request.BirthDate),
            cancellationToken);

        return result switch
        {
            { IsSuccess: true } => Ok(),
            { ErrorResponse: UserWithIdNotFoundError err } => Problem(err.Message,
                statusCode: (int)HttpStatusCode.NotFound),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
}