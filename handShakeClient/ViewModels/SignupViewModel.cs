using HandshakeClient.Composite;
using HandshakeClient.Services;
using System;
using System.Net.Http;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HandshakeClient.ViewModels
{
  [QueryProperty(nameof(Id), nameof(Id))]
  public class SignupViewModel : BaseViewModel
  {
    #region Constructors

    public SignupViewModel()
    {
      this.SignupCommand = new Command(this.SignupCommandExecute, this.SignupCommandCanExecute);
      this.CancelCommand = new Command(this.CancelCommandExecute);

      this.PropertyChanged += this.SignupViewModelPropertyChanged;
    }

    #endregion Constructors

    #region Properties

    public Command CancelCommand { get; }

    public string Id
    {
      get;
      set;
    }

    public bool IsAccepted
    {
      get;
      set;
    }

    public string Message
    {
      get;
      set;
    }

    public string Nickname
    {
      get;
      set;
    }

    public string Password
    {
      get;
      set;
    }

    public string PasswordRepeat
    {
      get;
      set;
    }

    public Command SignupCommand
    {
      get;
    }

    public string Username
    {
      get;
      set;
    }

    #endregion Properties

    #region Methods

    private async void CancelCommandExecute()
    {
      await Shell.Current.GoToAsync($"..");
    }

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

      if (this.Password != this.PasswordRepeat)
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

        await Shell.Current.GoToAsync($"..");
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