using System.Net;
using CQRS;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoundHealing.Application.Commands.Quotes;
using SoundHealing.Application.Contracts.Requests.Quotes;
using SoundHealing.Application.Errors.Quotes;
using SoundHealing.Application.Interfaces;
using SoundHealing.Core;
using SoundHealing.Extensions;

namespace SoundHealing.Controllers.Quotes;

[ApiController]
[Route("api/quotes")]
public class QuoteController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Создать цитату.
    /// </summary>
    [HttpPost]
    [Authorize(PermissionsConstants.QuotesAdministration)]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateQuoteRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new CreateQuoteCommand(request.Text, request.Author), cancellationToken);
        
        return result switch
        {
            { IsSuccess: true } => Ok(result.Data),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
    
    /// <summary>
    /// Получить все цитаты (для админа)
    /// </summary>
    [HttpGet]
    [Authorize(PermissionsConstants.QuotesAdministration)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetAllQuotesCommand(), cancellationToken);
        
        return result switch
        {
            { IsSuccess: true } => Ok(result.Data),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
    
    /// <summary>
    /// Получить рандомную цитату.
    /// </summary>
    [HttpGet("random")]
    public async Task<IActionResult> GetRandomAsync(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetRandomQuoteCommand(), cancellationToken);
        
        return result switch
        {
            { IsSuccess: true } => Ok(result.Data),
            { ErrorResponse: QuotesDoesNotExists err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.NotFound),
            _ => throw new UnexpectedErrorResponseException()
        };
    }

    /// <summary>
    /// Обновить цитату.
    /// </summary>
    [HttpPut("{quoteId:guid}")]
    [Authorize(PermissionsConstants.QuotesAdministration)]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] Guid quoteId,
        [FromBody] EditQuoteRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new EditQuoteCommand(quoteId, request.Text, request.Author),
            cancellationToken);
        
        return result switch
        {
            { IsSuccess: true } => Ok(),
            { ErrorResponse: QuoteWithIdDoesNotExists err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.NotFound),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
    
    /// <summary>
    /// Удалить цитату.
    /// </summary>
    [HttpDelete("{quoteId:guid}")]
    [Authorize(PermissionsConstants.QuotesAdministration)]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] Guid quoteId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new DeleteQuoteCommand(quoteId), cancellationToken);
        
        return result switch
        {
            { IsSuccess: true } => Ok(),
            { ErrorResponse: QuoteWithIdDoesNotExists err } => Problem(
                err.Message, statusCode: (int)HttpStatusCode.NotFound),
            _ => throw new UnexpectedErrorResponseException()
        };
    }
}