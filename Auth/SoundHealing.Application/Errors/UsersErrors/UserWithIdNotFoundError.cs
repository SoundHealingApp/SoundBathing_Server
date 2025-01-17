using CQRS;

namespace SoundHealing.Application.Errors.UsersErrors;

public class UserWithIdNotFoundError(string userId) 
    : ErrorResponse($"User with id {userId} does not exist");