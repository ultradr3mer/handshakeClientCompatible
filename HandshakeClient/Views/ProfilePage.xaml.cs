using HandshakeClient.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandshakeClient.Views
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class ProfilePage : ContentPage
  {
    public ProfilePage()
    {
      InitializeComponent();

      this.ViewModel = App.Resolve<ProfileViewModel>();
    }

    public ProfileViewModel ViewModel
    {
      get { return this.BindingContext as ProfileViewModel; }
      set { this.BindingContext = value; }
    }

    protected override void OnAppearing()
    {
      base.OnAppearing();

      this.ViewModel.InitializeAsync();
    }

  }
}