using CQRS;

namespace Auth.Application.Errors;

public class UserNotFoundError(string userName) 
    : ErrorResponse($"User with userName {userName} does not exist");