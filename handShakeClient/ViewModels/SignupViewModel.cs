using HandshakeClient.Composite;
using HandshakeClient.Services;
using HandshakeClient.Views;
using System;
using System.Net.Http;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HandshakeClient.ViewModels
{
  [QueryProperty(nameof(Id), nameof(Id))]
  public class SignupViewModel : BaseViewModel
  {
    #region Fields

    private string propId;
    private bool propIsAccepted;
    private string propMessage;
    private string propNickname;
    private string propPassword;
    private string propPasswordRepeat;
    private string propUsername;

    #endregion Fields

    #region Constructors

    public SignupViewModel()
    {
      this.SignupCommand = new Command(this.SignupCommandExecute, this.SignupCommandCanExecute);

      this.PropertyChanged += this.SignupViewModelPropertyChanged;
    }

    #endregion Constructors

    #region Properties

    public string Id
    {
      get { return this.propId; }
      set { this.SetProperty(ref this.propId, value); }
    }

    public bool IsAccepted
    {
      get { return propIsAccepted; }
      set { SetProperty(ref propIsAccepted, value); }
    }

    public string Message
    {
      get { return this.propMessage; }
      set { this.SetProperty(ref this.propMessage, value); }
    }

    public string Nickname
    {
      get { return this.propNickname; }
      set { this.SetProperty(ref this.propNickname, value); }
    }

    public string Password
    {
      get { return this.propPassword; }
      set { this.SetProperty(ref this.propPassword, value); }
    }

    public string PasswordRepeat
    {
      get { return this.propPasswordRepeat; }
      set { this.SetProperty(ref this.propPasswordRepeat, value); }
    }

    public Command SignupCommand
    {
      get;
    }

    public string Username
    {
      get { return this.propUsername; }
      set { this.SetProperty(ref this.propUsername, value); }
    }

    #endregion Properties

    #region Methods

    private bool SignupCommandCanExecute()
    {
      return !string.IsNullOrEmpty(this.Id)
        && !string.IsNullOrEmpty(this.Password)
        && !string.IsNullOrEmpty(this.PasswordRepeat)
        && !string.IsNullOrEmpty(this.Username)
        && this.IsAccepted;
    }

    private async void SignupCommandExecute()
    {
      this.Message = string.Empty;

      Client client = new Client(new HttpClient());

      if (!Guid.TryParse(this.Id, out Guid inviteCode))
      {
        this.Message = "Invite code invalid.";
        return;
      }

      if(this.Password != this.PasswordRepeat)
      {
        this.Message = "Passwords must match.";
        return;
      }

      this.IsBusy = true;

      try
      {
        UserPostData data = new UserPostData()
        {
          InviteCode = inviteCode,
          Username = this.Username,
          Nickname = this.Nickname,
          Password = this.Password
        };

        await client.SignupPostAsync(data);

        await SecureStorage.SetAsync(LoginViewModel.UsernameKey, this.Username);
        await SecureStorage.SetAsync(LoginViewModel.PasswordKey, this.Password);

        App.Client = client;

        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
      }
      catch (ApiException exception)
      {
        this.Message = exception.ToString();
      }
      finally
      {
        this.IsBusy = false;
      }
    }

    private void SignupViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      this.SignupCommand.ChangeCanExecute();
    }

    #endregion Methods
  }
}