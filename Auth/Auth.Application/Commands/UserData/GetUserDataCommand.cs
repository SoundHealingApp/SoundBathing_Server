using Auth.Application.Errors;
using Auth.Application.Errors.Auth;
using Auth.Core.Interfaces;
using Auth.Core.Models;
using CQRS;
using MediatR;

namespace Auth.Application.Commands.UserData;

public record GetUserDataCommand(string UserId) : IRequest<Result<User>>;

public class GetUserDataCommandHandler(IUserRepository userRepository) : IRequestHandler<GetUserDataCommand, Result<User>>
{
    public async Task<Result<User>> Handle(GetUserDataCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(Guid.Parse(request.UserId), cancellationToken);

        if (user == null)
            return new UserWithIdNotFoundError(request.UserId);

        return user;
    }
}