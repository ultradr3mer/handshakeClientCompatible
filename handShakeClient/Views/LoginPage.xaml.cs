using HandshakeClient.ViewModels;
using System;
using Xamarin.Forms;

namespace HandshakeClient.Views
{
  public partial class LoginPage : ContentPage
  {

    #region Constructors

    public LoginPage()
    {
      InitializeComponent();
      this.ViewModel = App.Resolve<LoginViewModel>();
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

    private void EnterPressed(object sender, EventArgs e)
    {
      this.ViewModel.LoginCommand.Execute(null);
    }

    private void SignupItemClicked(object sender, System.EventArgs e)
    {
      this.ViewModel.SignupCommand.Execute(null);
    }

    #endregion Methods
  }
}