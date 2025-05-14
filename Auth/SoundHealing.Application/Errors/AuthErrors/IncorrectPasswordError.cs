using CQRS;
using SoundHealing.Application.Interfaces;

namespace SoundHealing.Application.Errors.AuthErrors;

public class IncorrectPasswordError() : ErrorResponse("Incorrect password");