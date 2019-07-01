using System.Threading.Tasks;
using LibraProgramming.ChatRoom.Services.Chat.Persistence.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core.Services
{
    internal sealed class EntityFrameworkSigninService : ISigninService<Customer, long>
    {
        private readonly UserManager<Customer> userManager;
        private readonly SignInManager<Customer> signInManager;

        public EntityFrameworkSigninService(
            UserManager<Customer> userManager,
            SignInManager<Customer> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<SignInResult> ValidateCredentialsAsync(Customer user, string password)
        {
            var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
            return result;
        }

        public Task<Customer> FindByEmailAsync(string email)
        {
            return userManager.FindByEmailAsync(email);
        }

        public Task SigninAsync(Customer user, AuthenticationProperties properties, string authenticationMethod)
        {
            return signInManager.SignInAsync(user, properties, authenticationMethod);
        }
    }
}