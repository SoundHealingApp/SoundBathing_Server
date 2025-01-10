using Auth.Core;
using Auth.Core.CQRS;

namespace Auth.Application.Errors;

public class IncorrectPassword() : ErrorResponse("Incorrect password");