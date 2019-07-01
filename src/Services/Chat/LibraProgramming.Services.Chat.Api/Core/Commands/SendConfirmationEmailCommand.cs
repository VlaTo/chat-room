using LibraProgramming.ChatRoom.Services.Chat.Persistence.Models;
using MediatR;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core.Commands
{
    public class SendConfirmationEmailCommand : IRequest
    {
        public SendConfirmationEmailCommand(Customer customer)
        {
        }
    }
}