using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Models
{
    [DataContract]
    public class ConsentInputModel
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Button
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public IEnumerable<string> ScopesConsented
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool RememberConsent
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataType(DataType.Url)]
        [DataMember]
        public string ReturnUrl
        {
            get;
            set;
        }
    }
}