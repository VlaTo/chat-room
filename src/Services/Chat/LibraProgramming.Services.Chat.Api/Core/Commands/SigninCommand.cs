using LibraProgramming.ChatRoom.Services.Chat.Persistence.Models;
using MediatR;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core.Commands
{
    public class SigninCommand : IRequest
    {
        public bool ShouldPersist
        {
            get;
        }

        public Customer Customer
        {
            get;
        }

        public SigninCommand(Customer customer, bool shouldPersist = true)
        {
            Customer = customer;
            ShouldPersist = shouldPersist;
        }
    }
}