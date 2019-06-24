using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraProgramming.ChatRoom.Client.Models.Database
{
    [Table(nameof(Message), Schema = "Chat")]
    public sealed class Message : IEntity<long>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id
        {
            get;
            set;
        }

        [DataType(DataType.MultilineText)]
        public string Content
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

        [DataType(DataType.DateTime)]
        public DateTime Created
        {
            get;
            set;
        }
    }
}