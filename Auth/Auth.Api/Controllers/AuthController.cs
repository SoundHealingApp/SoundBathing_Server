using System.Net;
using Auth.Application.Commands;
using Auth.Application.Errors;
using Auth.Contracts.Requests;
using Auth.Core.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers;


[ApiController]
[Route("[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] UserRegisterRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new RegisterCommand(request.UserName, request.Password),
            cancellationToken);

        return result switch
        {
            { IsSuccess: true } => Ok(),
            { ErrorResponse: UserAlreadyExistsError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.UnprocessableEntity),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] UserLoginRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new LoginCommand(request.UserName, request.Password),
            cancellationToken);

        return result switch
        {
            { IsSuccess: true } => Ok(result.Data),
            { ErrorResponse: IncorrectPassword err } => Problem(err.Message,
                statusCode: (int)HttpStatusCode.Unauthorized),
            { ErrorResponse: UserNotFoundError err } => Problem(err.Message,
                statusCode: (int)HttpStatusCode.Unauthorized),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
}