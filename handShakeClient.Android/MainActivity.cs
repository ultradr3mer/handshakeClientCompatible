﻿using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using HandshakeClient.Views;
using HandshakeClient.ViewModels;
using FFImageLoading.Forms.Droid;
using Xamarin.Essentials;
using Android.Graphics;

namespace HandshakeClient.Droid
{
  [Activity(Label = "HandshakeClient", 
  Icon = "@mipmap/icon", 
  Theme = "@style/MainTheme", 
  MainLauncher = true,
  ScreenOrientation = ScreenOrientation.Portrait,
  ConfigurationChanges = ConfigChanges.ScreenSize |
                          ConfigChanges.Orientation |
                          ConfigChanges.UiMode |
                          ConfigChanges.ScreenLayout |
                          ConfigChanges.SmallestScreenSize)]
	[IntentFilter(new[] { Android.Content.Intent.ActionView },
  Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable },
  DataHost = "handshake.azurewebsites.net",
  DataPathPrefixes = new[] { signupPrefix },
  DataSchemes = new[] { "http", "https" })]
  public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
  {
    #region Fields

    private const string signupPrefix = "/signup";

    #endregion Fields

    #region Methods

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
    {
      Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

      base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }

    protected override void OnCreate(Bundle savedInstanceState)
    {
      TabLayoutResource = HandshakeClient.Droid.Resource.Layout.Tabbar;
      ToolbarResource = HandshakeClient.Droid.Resource.Layout.Toolbar;

      base.OnCreate(savedInstanceState);

      Window.SetStatusBarColor(Android.Graphics.Color.Argb(255, 0, 0, 0));
      Platform.Init(this, savedInstanceState);
      global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

      string initialNavigation = null;

      if (Intent.Data != null)
      {
        var path = Intent.Data.Path;

        if(path == signupPrefix)
        {
          var id = Intent.Data.GetQueryParameter("id");
          initialNavigation = $"//{nameof(LoginPage)}/{nameof(SignupPage)}?{nameof(SignupViewModel.Id)}={id}";
        }
      }

      LoadApplication(new App(initialNavigation));

      Window.SetStatusBarColor(Color.Argb(255, 196, 164, 29));
      CachedImageRenderer.Init(true);
    }

    #endregion Methods
  }
}
