using HandshakeClient.Composite;
using HandshakeClient.Services;
using HandshakeClient.Views;
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
    private string propMessage;
    private string propPassword;
    private string propUsername;
    private bool firstOpened = true;

    #endregion Fields

    #region Constructors

    public LoginViewModel(AccountViewModel accountViewModel)
    {
      this.LoginCommand = new Command(this.LoginCommandExecute);
      this.SignupCommand = new Command(this.SignupCommandExecute);
      this.accountViewModel = accountViewModel;
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
          await Continue();
        }

        this.firstOpened = false;
      }
    }

    private static async Task Continue()
    {
      await Shell.Current.GoToAsync($"//{nameof(PostsPage)}");
    }

    private async void LoginCommandExecute()
    {
      this.IsBusy = true;
      this.Message = string.Empty;

      Client client = new Client(new CustomHttpClient(this.Username, this.Password));

      try
      {
        await this.accountViewModel.Initialize();
        this.Message = $"Signed in as {this.accountViewModel.Nickname}.";

        await SecureStorage.SetAsync(LoginViewModel.UsernameKey, this.Username);
        await SecureStorage.SetAsync(LoginViewModel.PasswordKey, this.Password);

        App.Client = client;

        await Continue();
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