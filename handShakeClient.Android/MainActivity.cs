﻿using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;

namespace handShakeClient.Droid
{
  [Activity(Label = "handShakeClient", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
  public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
  {
    protected override void OnCreate(Bundle savedInstanceState)
    {
      TabLayoutResource = Xaminals.Droid.Resource.Layout.Tabbar;
      ToolbarResource = Xaminals.Droid.Resource.Layout.Toolbar;

      base.OnCreate(savedInstanceState);

      Window.SetStatusBarColor(Android.Graphics.Color.Argb(255, 0, 0, 0));
      global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
      Xamarin.Essentials.Platform.Init(this, savedInstanceState);
      LoadApplication(new App());
    }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
    {
      Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

      base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }
  }
}
