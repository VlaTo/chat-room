using System.Runtime.Serialization;

namespace LibraProgramming.Services.Chat.Domain
{
    [DataContract]
    public class UserJoinedOperationResult : OperationResult
    {
        [DataMember]
        public string RoomName
        {
            get;
            set;
        }

        [DataMember]
        public string UserName
        {
            get;
            set;
        }
    }
}
