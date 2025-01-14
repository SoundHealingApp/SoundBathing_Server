using System.Net;
using Auth.Application.Contracts.Requests;
using CQRS;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SoundHealing.Application.Commands.UserData;
using SoundHealing.Application.Contracts.Requests;
using SoundHealing.Application.Errors.Auth;

namespace SoundHealing.Controllers;

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
}