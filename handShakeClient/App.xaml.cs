using FFImageLoading.Transformations;
using HandshakeClient.Services;
using HandshakeClient.Themes;
using HandshakeClient.ViewModels;
using System.Collections.Generic;
using Unity;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandshakeClient
{
  public partial class App : Application
  {
    public static bool IsUserLoggedIn { get; internal set; }
    public static Client Client { get; internal set; }

    private static UnityContainer container = new UnityContainer();
    private string initialNavigation;

    public App()
    {
      InitializeComponent();

      var theme = AppInfo.RequestedTheme;

      ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
      if (mergedDictionaries != null)
      {
        mergedDictionaries.Clear();


        switch (theme)
        {
          case AppTheme.Dark:
            mergedDictionaries.Add(new OrangeDarkTheme());
            break;
          case AppTheme.Light:
          default:
            mergedDictionaries.Add(new OrangeTheme());
            break;
        }
      }

      MainPage = new AppShell();
      container.RegisterSingleton<LocationCache>();
      container.RegisterSingleton<AccountViewModel>();
    }

    public App(string initialNavigation) : this()
    {
      this.initialNavigation = initialNavigation;
    }

    protected override void OnStart()
    {
      if (!string.IsNullOrEmpty(initialNavigation))
      {
        Shell.Current.GoToAsync(this.initialNavigation);
      }
    }

    protected override void OnSleep()
    {
    }

    protected override void OnResume()
    {
    }

    public static T Resolve<T>()
    {
      return container.Resolve<T>();
    }
  }
}
