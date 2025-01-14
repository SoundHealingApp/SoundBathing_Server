using CQRS;

namespace Auth.Application.Errors.Auth;

public class UserWithIdNotFoundError(string userId) 
    : ErrorResponse($"User with id {userId} does not exist");