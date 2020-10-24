using HandshakeClient.Composite;
using HandshakeClient.Services;
using HandshakeClient.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HandshakeClient.ViewModels
{
  public class LoginViewModel : BaseViewModel
  {
    #region Fields

    public const string PasswordKey = "password";
    public const string UsernameKey = "username";
    private readonly AccountViewModel accountViewModel;
    private bool firstOpened = true;
    private string propMessage;
    private string propPassword;
    private string propUsername;

    #endregion Fields

    #region Constructors

    public LoginViewModel(AccountViewModel accountViewModel)
    {
      this.LoginCommand = new Command(this.LoginCommandExecute, this.LoginCommandCanExecute);
      this.SignupCommand = new Command(this.SignupCommandExecute);
      this.accountViewModel = accountViewModel;

      this.PropertyChanged += this.LoginViewModel_PropertyChanged;
    }

    private void LoginViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      if(e.PropertyName == nameof(Password) || e.PropertyName == nameof(Username))
      {
        this.LoginCommand.ChangeCanExecute();
      }
    }

    #endregion Constructors

    #region Properties

    public Command LoginCommand { get; }

    public string Message
    {
      get { return this.propMessage; }
      set { this.SetProperty(ref this.propMessage, value); }
    }

    public string Password
    {
      get { return this.propPassword; }
      set { this.SetProperty(ref this.propPassword, value); }
    }

    public Command SignupCommand { get; }

    public string Username
    {
      get { return this.propUsername; }
      set { this.SetProperty(ref this.propUsername, value); }
    }

    #endregion Properties

    #region Methods

    internal async void Initialize()
    {
      this.Username = await SecureStorage.GetAsync(LoginViewModel.UsernameKey);
      this.Password = await SecureStorage.GetAsync(LoginViewModel.PasswordKey);

      if (this.firstOpened)
      {
        if (!string.IsNullOrEmpty(this.Username) && !string.IsNullOrEmpty(this.Password))
        {
          App.Client = new Client(new CustomHttpClient(this.Username, this.Password));
          await this.Continue();
        }

        this.firstOpened = false;
      }
    }

    private async Task Continue()
    {
      this.Message = $"Signed in as {this.Username}";
      await Shell.Current.GoToAsync($"//{nameof(PostsPage)}");
    }

    private bool LoginCommandCanExecute()
    {
      return !string.IsNullOrWhiteSpace(this.Username) && !string.IsNullOrWhiteSpace(this.Password);
    }

    private async void LoginCommandExecute()
    {
      this.IsBusy = true;
      this.Message = $"Signing in as {this.Username}...";

      App.Client = new Client(new CustomHttpClient(this.Username, this.Password));

      try
      {
        await this.accountViewModel.Initialize();

        await SecureStorage.SetAsync(LoginViewModel.UsernameKey, this.Username);
        await SecureStorage.SetAsync(LoginViewModel.PasswordKey, this.Password);

        this.Message = string.Empty;

        await this.Continue();
      }
      catch (ApiException exception)
      {
        await SecureStorage.SetAsync(LoginViewModel.UsernameKey, string.Empty);
        await SecureStorage.SetAsync(LoginViewModel.PasswordKey, string.Empty);

        this.Message = exception.ToString();
      }
      finally
      {
        this.IsBusy = false;
      }
    }

    private async void SignupCommandExecute(object obj)
    {
      await Shell.Current.GoToAsync(nameof(SignupPage));
    }

    #endregion Methods
  }
}