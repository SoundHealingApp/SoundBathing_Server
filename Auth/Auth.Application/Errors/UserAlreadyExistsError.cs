using Auth.Core;
using Auth.Core.CQRS;

namespace Auth.Application.Errors;

public class UserAlreadyExistsError(string userName)
    : ErrorResponse($"User with username {userName} already exists.");