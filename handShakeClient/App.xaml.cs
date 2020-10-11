using HandshakeClient.Services;
using Unity;
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
      MainPage = new AppShell();
      container.RegisterSingleton<LocationCache>();
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
