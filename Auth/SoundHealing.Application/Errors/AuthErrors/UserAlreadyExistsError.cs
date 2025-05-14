using CQRS;
using SoundHealing.Application.Interfaces;

namespace SoundHealing.Application.Errors.AuthErrors;

public class UserAlreadyExistsError(string email)
    : ErrorResponse($"User with email {email} already exists.");