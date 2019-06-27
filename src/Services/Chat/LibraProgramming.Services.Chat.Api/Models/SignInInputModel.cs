using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class SignInInputModel : CredentialsModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "RememberMeField", Prompt = "RememberMePrompt")]
        [DataMember]
        public bool RememberMe
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