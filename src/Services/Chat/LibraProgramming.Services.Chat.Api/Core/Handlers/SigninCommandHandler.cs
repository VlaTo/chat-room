using System;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4;
using LibraProgramming.ChatRoom.Services.Chat.Api.Core.Commands;
using LibraProgramming.ChatRoom.Services.Chat.Api.Core.Services;
using LibraProgramming.ChatRoom.Services.Chat.Persistence.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core.Handlers
{
    internal sealed class SigninCommandHandler : IRequestHandler<SigninCommand>
    {
        private readonly ISigninService<Customer, long> loginService;

        public SigninCommandHandler(ISigninService<Customer, long> loginService)
        {
            this.loginService = loginService;
        }

        public async Task<Unit> Handle(SigninCommand request, CancellationToken cancellationToken)
        {
            AuthenticationProperties properties = null;

            if (AccountOptions.AllowRememberMe && request.ShouldPersist)
            {
                properties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    IssuedUtc = DateTimeOffset.UtcNow,
                    ExpiresUtc = DateTimeOffset.UtcNow + AccountOptions.RememberMeSigninDuration
                };
            }

            await loginService.SigninAsync(
                request.Customer,
                properties,
                IdentityServerConstants.LocalIdentityProvider
            );

            return Unit.Value;
        }
    }
}