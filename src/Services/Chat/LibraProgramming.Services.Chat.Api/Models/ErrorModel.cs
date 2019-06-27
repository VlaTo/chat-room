using IdentityServer4.Models;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Models
{
    public sealed class ErrorModel
    {
        public ErrorMessage ErrorMessage
        {
            get;
        }

        public ErrorModel(ErrorMessage errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}