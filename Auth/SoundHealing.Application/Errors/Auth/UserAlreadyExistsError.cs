using CQRS;

namespace SoundHealing.Application.Errors.Auth;

public class UserAlreadyExistsError(string email)
    : ErrorResponse($"User with email {email} already exists.");