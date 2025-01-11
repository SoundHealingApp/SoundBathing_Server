using CQRS;

namespace Auth.Application.Errors;

public class UserAlreadyExistsError(string email)
    : ErrorResponse($"User with email {email} already exists.");