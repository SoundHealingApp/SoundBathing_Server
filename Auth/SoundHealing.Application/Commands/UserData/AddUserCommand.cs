using CQRS;
using MediatR;
using SoundHealing.Application.Errors.Auth;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Application.Commands.UserData;

public record AddUserCommand(string UserId, string Name, string Surname, DateTime BirthDate)
    : IRequest<Result<Unit>>;

internal sealed class AddUserCommandHandler(IUserRepository userRepository)
    : IRequestHandler<AddUserCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(Guid.Parse(request.UserId), cancellationToken);

        if (user != null)
            return new UserAlreadyExistsError(request.UserId);
        
        user = new User(request.UserId, request.Name, request.Surname, request.BirthDate);

        await userRepository.AddAsync(user, cancellationToken);

        return Unit.Value;
    }
}