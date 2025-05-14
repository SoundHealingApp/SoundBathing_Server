using CQRS;
using MediatR;
using SoundHealing.Application.Contracts.Dto;
using SoundHealing.Application.Errors.UsersErrors;
using SoundHealing.Application.Interfaces;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Application.Commands.UserData;

public record GetUserDataCommand(string UserId) : IRequest<Result<UserDto>>;

public class GetUserDataCommandHandler(IUserRepository userRepository)
    : IRequestHandler<GetUserDataCommand, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(GetUserDataCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(Guid.Parse(request.UserId), cancellationToken);

        if (user == null)
            return new UserWithIdNotFoundError(request.UserId);
        
        var userDto = new UserDto(user.Id, user.Name, user.Surname, user.BirthDate);

        return userDto;
    }
}