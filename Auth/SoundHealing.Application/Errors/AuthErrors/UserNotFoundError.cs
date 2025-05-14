using CQRS;
using SoundHealing.Application.Interfaces;

namespace SoundHealing.Application.Errors.AuthErrors;

public class UserWithEmailNotFoundError(string email) 
    : ErrorResponse($"User with email {email} does not exist");