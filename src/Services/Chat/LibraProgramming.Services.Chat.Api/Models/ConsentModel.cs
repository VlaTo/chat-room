using System.Collections.Generic;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Models
{
    public sealed class ScopeViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string DisplayName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Emphasize
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Required
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Checked
        {
            get;
            set;
        }
    }

    public sealed class ConsentModel : ConsentInputModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string ClientName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string ClientUrl
        {
            get;
            set;
        }

        public string ClientLogoUrl
        {
            get;
            set;
        }

        public bool AllowRememberConsent
        {
            get;
            set;
        }

        public IEnumerable<ScopeViewModel> IdentityScopes
        {
            get;
            set;
        }

        public IEnumerable<ScopeViewModel> ResourceScopes
        {
            get;
            set;
        }
    }
}