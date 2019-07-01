using System.Runtime.Serialization;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Models
{
    [DataContract]
    public class RedirectModel
    {
        [DataMember]
        public string RedirectUrl
        {
            get;
            set;
        }
    }
}