using System.Net;
using Auth.Application.Commands;
using Auth.Application.Contracts.Requests;
using Auth.Application.Errors;
using CQRS;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Auth.Controllers;


[ApiController]
[Route("[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] UserRegisterRequest request,
        [FromServices] IValidator<UserRegisterRequest> validator,
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
            new LoginCommand(request.Email, request.Password),
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