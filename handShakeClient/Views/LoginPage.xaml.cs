using HandshakeClient.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandshakeClient.Views
{
  public partial class LoginPage : ContentPage
  {
    #region Constructors

    public LoginPage()
    {
      InitializeComponent();
      this.ViewModel = new LoginViewModel();
    }

    #endregion Constructors

    #region Properties

    public LoginViewModel ViewModel
    {
      get { return this.BindingContext as LoginViewModel; }
      set { this.BindingContext = value; }
    }

    #endregion Properties

    #region Methods

    protected override void OnAppearing()
    {
      base.OnAppearing();

      this.ViewModel.Initialize();
    }

    #endregion Methods
  }
}