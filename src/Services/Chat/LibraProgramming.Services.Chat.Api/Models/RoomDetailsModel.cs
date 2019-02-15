using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Models
{
    [DataContract(Name = "room")]
    public class RoomDetailsModel
    {
        [Required]
        [MinLength(3)]
        [DataMember(Name = "name")]
        public string Name
        {
            get;
            set;
        }

        [DataMember(Name = "description")]
        public string Description
        {
            get;
            set;
        }
    }
}