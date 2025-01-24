using System.Net;
using CQRS;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SoundHealing.Application.Commands.Meditations.LikeMeditationsCommand;
using SoundHealing.Application.Commands.Meditations.RecommendMeditationsCommands;
using SoundHealing.Application.Commands.UserData;
using SoundHealing.Application.Contracts.Requests.User;
using SoundHealing.Application.Errors.AuthErrors;
using SoundHealing.Application.Errors.MeditationErrors;
using SoundHealing.Application.Errors.UsersErrors;

namespace SoundHealing.Controllers.Users;

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
    
    [HttpPost("{userId:guid}/change-data")]
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
    
    /// <summary>
    /// Добавить медитацию в понравившуюся.
    /// </summary>
    [HttpPost("{userId}/meditations/{meditationId}/like")]
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
            { ErrorResponse: MeditationWithIdDoesNotExists err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.NotFound),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
    
    /// <summary>
    /// Удалить медитацию из понравившихся
    /// </summary>
    [HttpDelete("{userId}/liked-meditations/{meditationId}")]
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
            { ErrorResponse: MeditationWithIdDoesNotExists err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.NotFound),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
    
    /// <summary>
    /// Получить понравившиеся медитации.
    /// </summary>
    [HttpGet("{userId}/meditations/liked")]
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
    
    /// <summary>
    /// Добавить медитации в рекоммендованные.
    /// </summary>
    [HttpPost("{userId}/meditations/recommend")]
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
    [HttpGet("{userId}/meditations/recommended")]
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