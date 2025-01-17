using CQRS;

namespace SoundHealing.Application.Errors.AuthErrors;

public class UserAlreadyExistsError(string email)
    : ErrorResponse($"User with email {email} already exists.");