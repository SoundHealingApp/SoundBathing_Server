using Auth.Application.Errors;
using Auth.Application.Interfaces;
using Auth.Core.Interfaces;
using Auth.Core.Models;
using CQRS;
using MediatR;

namespace Auth.Application.Commands;

public record RegisterCommand(string Email, string Password) : IRequest<Result<Unit>>;

internal sealed class RegisterCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher) 
    : IRequestHandler<RegisterCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.GetByEmailAsync(request.Email) != null)
            return new UserAlreadyExistsError(request.Email);
        
        var hashedPassword = passwordHasher.Generate(request.Password);
        
        var user = User.Create(request.Email, hashedPassword);
        
        await userRepository.AddAsync(user);

        return Unit.Value;
    }
}