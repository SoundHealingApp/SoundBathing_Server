using CQRS;
using MediatR;
using SoundHealing.Application.Errors.AuthErrors;
using SoundHealing.Application.Interfaces;
using SoundHealing.Core.Interfaces;

namespace SoundHealing.Application.Commands.Auth;

public record CheckUserExistenseQuery(string Email) : IRequest<Result<Unit>>;

internal sealed class CheckUserExistenseQueryHandler(IUserCredentialsRepository userCredentialsRepository) : IRequestHandler<CheckUserExistenseQuery, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(CheckUserExistenseQuery request, CancellationToken cancellationToken)
    {
        if (await userCredentialsRepository.GetByEmailAsync(request.Email) != null)
            return new UserAlreadyExistsError(request.Email);
        
        return Unit.Value;
    }
}