using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core.Services
{
    public interface ISigninService<T, TKey>
        where T : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        Task<SignInResult> ValidateCredentialsAsync(T user, string password);

        Task<T> FindByEmailAsync(string email);

        Task SigninAsync(T user, AuthenticationProperties properties, string authenticationMethod);
    }
}