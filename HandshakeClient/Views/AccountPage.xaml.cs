using HandshakeClient.ViewModels;
using Xamarin.Forms;

namespace HandshakeClient.Views
{
  public partial class AccountPage : ContentPage
  {
    #region Constructors

    public AccountPage()
    {
      this.InitializeComponent();

      this.ViewModel = new AccountViewModel();
    }

    #endregion Constructors

    #region Properties

    public AccountViewModel ViewModel
    {
      get { return this.BindingContext as AccountViewModel; }
      set { this.BindingContext = value; }
    }

    #endregion Properties

    #region Methods

    protected override void OnAppearing()
    {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
      this.ViewModel.Initialize();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }

    #endregion Methods
  }
}