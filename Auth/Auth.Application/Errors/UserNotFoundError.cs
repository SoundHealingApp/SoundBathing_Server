using CQRS;

namespace Auth.Application.Errors;

public class UserNotFoundError(string email) 
    : ErrorResponse($"User with email {email} does not exist");