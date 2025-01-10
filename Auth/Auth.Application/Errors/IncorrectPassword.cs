using CQRS;

namespace Auth.Application.Errors;

public class IncorrectPassword() : ErrorResponse("Incorrect password");