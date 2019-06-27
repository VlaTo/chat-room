using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraProgramming.ChatRoom.Client.Models.Data
{
    [Table(nameof(Message),Schema = "Chat")]
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id
        {
            get;
            set;
        }

        public long RoomId
        {
            get;
            set;
        }

        [DataType(DataType.Text)]
        public string Author
        {
            get;
            set;
        }

        [DataType(DataType.MultilineText)]
        public string Text
        {
            get;
            set;
        }

        [DataType(DataType.DateTime)]
        public DateTime Created
        {
            get;
            set;
        }

        [ForeignKey(nameof(RoomId))]
        public virtual Room Room
        {
            get;
            set;
        }
    }
}