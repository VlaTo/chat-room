using System.Runtime.Serialization;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Models
{
    [DataContract]
    public class ExternalProvider
    {
        [DataMember]
        public string DisplayName
        {
            get;
            set;
        }

        [DataMember]
        public string AuthenticationScheme
        {
            get;
            set;
        }
    }
}