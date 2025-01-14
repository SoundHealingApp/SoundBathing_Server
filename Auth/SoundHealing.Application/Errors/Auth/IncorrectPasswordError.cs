using CQRS;

namespace SoundHealing.Application.Errors.Auth;

public class IncorrectPasswordError() : ErrorResponse("Incorrect password");