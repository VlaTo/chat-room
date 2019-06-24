using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraProgramming.ChatRoom.Client.Models.Data
{
    [Table(nameof(Room))]
    public class Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id
        {
            get;
            set;
        }

        [DataType(DataType.Text)]
        public string Title
        {
            get;
            set;
        }

        [DataType(DataType.MultilineText)]
        public string Description
        {
            get;
            set;
        }

        public virtual ICollection<Message> Messages
        {
            get;
            set;
        }
    }
}