using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Models
{
    [DataContract]
    public class CredentialsModel
    {
        [Display(Name = "EmailField", Prompt = "EmailPrompt", Description = "EmailFieldDescription")]
        [Required(ErrorMessage = "EmailRequiredError")]
        [DataType(DataType.EmailAddress, ErrorMessage = "EmailInvalidError")]
        [DataMember]
        public string Email
        {
            get;
            set;
        }

        [Display(Name = "PasswordField", Prompt = "PasswordPrompt")]
        [Required(ErrorMessage = "PasswordRequiredError")]
        [DataType(DataType.Password, ErrorMessage = "PasswordInvalidError")]
        [DataMember]
        public string Password
        {
            get;
            set;
        }
    }
}