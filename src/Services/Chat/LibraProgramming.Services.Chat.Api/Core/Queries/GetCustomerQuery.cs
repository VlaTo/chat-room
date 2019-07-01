using LibraProgramming.ChatRoom.Services.Chat.Api.Core.Models;
using MediatR;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core.Queries
{
    public class GetCustomerQuery : IRequest<CustomerResult>
    {
        public string Email
        {
            get;
        }

        public string Password
        {
            get;
        }

        public GetCustomerQuery(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}