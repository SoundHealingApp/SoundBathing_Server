using System.Net;
using CQRS;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SoundHealing.Application.Commands.Meditations.FeedbackCommands;
using SoundHealing.Application.Contracts.Requests.Meditation;
using SoundHealing.Application.Errors.MeditationErrors;
using SoundHealing.Application.Errors.UsersErrors;

namespace SoundHealing.Controllers.Meditations;

[ApiController]
[Route("api/meditations/{meditationId:guid}")]
public class FeedbackController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Добавить отзыв к медитации.
    /// </summary>
    [HttpPost("feedback")]
    public async Task<IActionResult> AddFeedback(
        [FromRoute] Guid meditationId,
        [FromBody] AddMeditationFeedbackRequest request,
        [FromServices] IValidator<AddMeditationFeedbackRequest> validator,
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
            new AddMeditationFeedbackCommand(request.UserId, meditationId, request.Comment, request.Estimate), 
            cancellationToken);

        return result switch
        {
            { IsSuccess: true } => Ok(),
            { ErrorResponse: UserWithIdNotFoundError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.NotFound),
            { ErrorResponse: MeditationWithIdDoesNotExistsError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.NotFound),
            { ErrorResponse: UserAlreadyProvidedFeedbackError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.Conflict),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
    
    /// <summary>
    /// Получить отзывы медитации.
    /// </summary>
    [HttpGet("feedbacks")]
    public async Task<IActionResult> GetFeedbacks(
        [FromRoute] Guid meditationId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetMeditationFeedbacksCommand(meditationId), cancellationToken);

        return result switch
        {
            { IsSuccess: true } => Ok(result.Data),
            { ErrorResponse: MeditationWithIdDoesNotExistsError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.NotFound),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
    
    /// <summary>
    /// Может ли пользователь добавлять отзыв к данной медитации.
    /// </summary>
    [HttpGet("can-add-feedback")]
    public async Task<IActionResult> CanUserAddFeedback(
        [FromRoute] Guid meditationId,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new CanUserAddFeedbackToMeditationCommand(userId, meditationId), cancellationToken);

        return result switch
        {
            { IsSuccess: true } => Ok(result.Data),
            { ErrorResponse: UserWithIdNotFoundError err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.NotFound),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
}