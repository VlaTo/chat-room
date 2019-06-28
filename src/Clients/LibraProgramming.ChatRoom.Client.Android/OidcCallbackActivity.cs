﻿using System;
using Android.App;
using Android.OS;
using Android.Util;
using Android.Content;

namespace LibraProgramming.ChatRoom.Client.Droid
{

    [Activity(Label = "OidcCallbackActivity")]
    [IntentFilter(
        new[] {Intent.ActionView},
        Categories = new[] {Intent.CategoryDefault, Intent.CategoryBrowsable},
        DataScheme = "xamarinformsclients")]
    // DataHost = "callback")]
    public class OidcCallbackActivity : Activity
    {
        public static event Action<string> Callbacks;

        public OidcCallbackActivity()
        {
            Log.Debug("OidcCallbackActivity", "constructing OidcCallbackActivity");
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Callbacks?.Invoke(Intent.DataString);

            Finish();

            StartActivity(typeof(MainActivity));
        }
    }
}