using CQRS;

namespace SoundHealing.Application.Errors.AuthErrors;

public class IncorrectPasswordError() : ErrorResponse("Incorrect password");