using System;
using System.Threading;
using System.Threading.Tasks;
using LibraProgramming.ChatRoom.Services.Chat.Api.Core.Models;
using LibraProgramming.ChatRoom.Services.Chat.Api.Core.Queries;
using LibraProgramming.ChatRoom.Services.Chat.Api.Core.Services;
using LibraProgramming.ChatRoom.Services.Chat.Persistence.Models;
using MediatR;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core.Handlers
{
    internal sealed class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, CustomerResult>
    {
        private readonly ISigninService<Customer, long> signinService;

        public GetCustomerQueryHandler(ISigninService<Customer, long> signinService)
        {
            this.signinService = signinService;
        }

        public async Task<CustomerResult> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var customer = await signinService.FindByEmailAsync(request.Email);

            if (null == customer)
            {
                throw new InvalidOperationException();
            }

            var signin = await signinService.ValidateCredentialsAsync(customer, request.Password);

            if (signin.Succeeded)
            {
                return CustomerResult.Succeeded(customer);
            }

            if (signin.IsNotAllowed)
            {
                return CustomerResult.NotAllowed(customer);
            }

            if (signin.IsLockedOut)
            {
                return CustomerResult.LockedOut(customer);
            }

            if (signin.RequiresTwoFactor)
            {
                return CustomerResult.TwoFactorRequired(customer);
            }

            return CustomerResult.Failed();
        }
    }
}