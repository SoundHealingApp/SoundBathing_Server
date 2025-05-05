using SoundHealing.Extensions;
using MediatR;
using SoundHealing.Application.Errors.UsersErrors;
using SoundHealing.Core.Interfaces;

namespace SoundHealing.Application.Commands.UserData;

public record ChangeUserDataCommand(Guid UserId, string? Name, string? Surname, DateOnly? BirthDate) : IRequest<Result<Unit>>;

internal sealed class ChangeUserDataCommandHandler(IUserRepository userRepository) : IRequestHandler<ChangeUserDataCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(ChangeUserDataCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsyncWithoutIncludes(request.UserId, cancellationToken);

        if (user == null)
            return new UserWithIdNotFoundError(request.UserId.ToString());
        
        if (request.Name != null)
            user.ChangeName(request.Name);
        
        if (request.Surname != null)
            user.ChangeSurname(request.Surname);
        
        if (request.BirthDate != null)
            user.ChangeBirthDate(request.BirthDate!.Value);
        
        await userRepository.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}