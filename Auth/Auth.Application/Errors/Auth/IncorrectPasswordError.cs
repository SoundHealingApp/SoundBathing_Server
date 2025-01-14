using CQRS;

namespace Auth.Application.Errors.Auth;

public class IncorrectPasswordError() : ErrorResponse("Incorrect password");