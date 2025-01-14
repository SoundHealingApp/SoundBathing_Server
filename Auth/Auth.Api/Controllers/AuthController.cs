using System.Net;
using Auth.Application.Commands;
using Auth.Application.Commands.Auth;
using Auth.Application.Contracts.Requests;
using Auth.Application.Contracts.Requests.Auth;
using Auth.Application.Errors;
using Auth.Application.Errors.Auth;
using CQRS;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Auth.Controllers;


[ApiController]
[Route("auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
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
            { IsSuccess: true } => Ok(result.Data.ToTuple()),
            { ErrorResponse: UserAlreadyExistsError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.UnprocessableEntity),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new LoginCommand(request.Email, request.Password),
            cancellationToken);

        return result switch
        {
            { IsSuccess: true } => Ok(result.Data.ToTuple()),
            { ErrorResponse: IncorrectPasswordError err } => Problem(err.Message,
                statusCode: (int)HttpStatusCode.Unauthorized),
            { ErrorResponse: UserWithEmailNotFoundError err } => Problem(err.Message,
                statusCode: (int)HttpStatusCode.Unauthorized),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
}