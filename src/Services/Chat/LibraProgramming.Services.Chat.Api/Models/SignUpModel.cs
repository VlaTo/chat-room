using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Models
{
    [DataContract]
    public class SignUpModel : CredentialsModel
    {
        [Display(Name = "UserNameField", Prompt = "UserNamePrompt")]
        [DataMember]
        [DataType(DataType.Text)]
        public string UserName
        {
            get;
            set;
        }

        [Display(Name = "ConfirmPasswordField", Prompt = "ConfirmPasswordPrompt")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "ConfirmPasswordRequiredError")]
        [DataMember]
        [DataType(DataType.Password, ErrorMessage = "ConfirmPasswordInvalidError")]
        [Compare(nameof(Password), ErrorMessage = "ConfirmPasswordMismatchError")]
        public string ConfirmPassword
        {
            get;
            set;
        }

        [Required]
        [DataMember]
        public bool AgreeWithPrivacyPolicy
        {
            get;
            set;
        }
    }
}