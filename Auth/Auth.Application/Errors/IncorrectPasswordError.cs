using CQRS;

namespace Auth.Application.Errors;

public class IncorrectPasswordError() : ErrorResponse("Incorrect password");