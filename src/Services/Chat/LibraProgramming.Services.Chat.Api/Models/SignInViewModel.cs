using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Models
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SignInViewModel : SignInInputModel
    {
        /// <summary>
        /// 
        /// </summary>
        public bool AllowRememberMe
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool EnableLocalLogin
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<ExternalProvider> ExternalProviders
        {
            get;
            set;
        }

        public bool IsExternalLoginOnly => false == EnableLocalLogin && ExternalProviders.Any();

        public IEnumerable<ExternalProvider> VisibleProviders =>
            ExternalProviders.Where(provider => false == String.IsNullOrEmpty(provider.DisplayName));

        public string ExternalAuthenticationScheme =>
            IsExternalLoginOnly ? ExternalProviders.FirstOrDefault()?.AuthenticationScheme : null;
    }
}