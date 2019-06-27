using System;
using Microsoft.AspNetCore.Server.IISIntegration;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core
{
    internal static class AccountOptions
    {
        public static readonly bool AllowLocalLogin = true;

        public static readonly bool AllowRememberMe = true;

        public static readonly string WindowsAuthenticationScheme = IISDefaults.AuthenticationScheme;

        public static readonly TimeSpan RememberMeSigninDuration = TimeSpan.FromDays(30.0d);

        public static readonly bool IncludeWindowsGroups = true;
    }
}