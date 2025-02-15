using System.Net;
using CQRS;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SoundHealing.Application.Commands.Auth;
using SoundHealing.Application.Contracts.Requests.Auth;
using SoundHealing.Application.Errors.AuthErrors;
using SoundHealing.Application.Errors.UsersErrors;
using SoundHealing.Core;

namespace SoundHealing.Controllers.Users.Auth;


[ApiController]
[Route("auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpGet("user")]
    public async Task<IActionResult> Get(
        [FromQuery] string email,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new CheckUserExistenseQuery(email),
            cancellationToken);

        return result switch
        {
            { IsSuccess: true } => Ok(),
            { ErrorResponse: UserAlreadyExistsError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.UnprocessableEntity),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        [FromServices] IValidator<RegisterRequest> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var modelStateDictionary = new ModelStateDictionary();
            
            foreach (var failure in validationResult.Errors)
            {
                modelStateDictionary.AddModelError(failure.PropertyName, failure.ErrorMessage);
            }

            return ValidationProblem(modelStateDictionary);
        }
        
        var result = await mediator.Send(
            new RegisterCommand(request.Email, request.Password),
            cancellationToken);

        return result switch
        {
            { IsSuccess: true } => Ok(result.Data),
            { ErrorResponse: UserAlreadyExistsError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.UnprocessableEntity),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new LoginCommand(request.Email, request.Password),
            cancellationToken);

        return result switch
        {
            { IsSuccess: true } => Ok(result.Data),
            { ErrorResponse: IncorrectPasswordError err } => Problem(err.Message,
                statusCode: (int)HttpStatusCode.Unauthorized),
            { ErrorResponse: UserWithEmailNotFoundError err } => Problem(err.Message,
                statusCode: (int)HttpStatusCode.Unauthorized),
            _ => throw new UnexpectedErrorResponseException()
        };
    }

    [HttpPost("{userId:guid}/change-credentials")]
    [Authorize(PermissionsConstants.EditUserInfo)]
    public async Task<IActionResult> ChangeCredentials(
        [FromRoute] Guid userId,
        [FromBody] ChangeCredentialsRequest request,
        [FromServices] IValidator<ChangeCredentialsRequest> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var modelStateDictionary = new ModelStateDictionary();
            
            foreach (var failure in validationResult.Errors)
            {
                modelStateDictionary.AddModelError(failure.PropertyName, failure.ErrorMessage);
            }

            return ValidationProblem(modelStateDictionary);
        }
        
        var result = await mediator.Send(
            new ChangeCredentialsCommand(userId, request.Email, request.Password),
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