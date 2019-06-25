using System;

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
            set;
        }

        public Uri BaseIdentityHostPath
        {
            get;
        }

        public static GlobalSettings Instance { get; } = new GlobalSettings();

        private GlobalSettings()
        {
            BaseIdentityHostPath = new Uri("http://localhost:5000");
        }
    }
}