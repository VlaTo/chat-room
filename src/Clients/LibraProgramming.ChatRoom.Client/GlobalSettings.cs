﻿using System;
using System.Diagnostics;
using IdentityModel;

namespace LibraProgramming.ChatRoom.Client
{
    public class GlobalSettings
    {
        public string ClientId
        {
            get;
            set;
        }

        public string ClientSecret
        {
            get;
            set;
        }

        public string AuthToken
        {
            get;
            set;
        }

        public string Callback
        {
            get;
        }

        public Uri BaseIdentityHostPath
        {
            get;
        }

        public bool SilentMode
        {
            get;
        }

        public static GlobalSettings Instance { get; } = new GlobalSettings();

        private GlobalSettings()
        {
            ClientId = "xamarin.application";
            //ClientSecret = "xamarin".ToSha256();
            ClientSecret = "xamarin";
            SilentMode = false;

            BaseIdentityHostPath = new Uri("https://localhost:5001");

            Callback = "xamarinformsclient://callback";
        }
    }
}