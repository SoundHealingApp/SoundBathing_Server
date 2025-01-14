using CQRS;

namespace Auth.Application.Errors.Auth;

public class UserWithEmailNotFoundError(string email) 
    : ErrorResponse($"User with email {email} does not exist");