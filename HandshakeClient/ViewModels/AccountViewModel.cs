using HandshakeClient.Composite;
using HandshakeClient.Services;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HandshakeClient.ViewModels
{
  public class AccountViewModel : BaseViewModel<ProfileGetData, ProfilePutData>
  {
    #region Fields

    private bool isInitialized = false;
    private string propMessage;
    private string propNickname;
    private string propSummary;

    #endregion Fields

    #region Constructors

    public AccountViewModel()
    {
      this.SaveCommand = new Command(async () => await this.SaveCommandExecute(), this.SaveCommandCanExecute);

      this.PropertyChanged += this.AccountViewModelPropertyChanged;
    }

    #endregion Constructors

    #region Properties

    public string Description
    {
      get { return this.propSummary; }
      set { this.SetProperty(ref this.propSummary, value); }
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

    public Command SaveCommand { get; }

    #endregion Properties

    #region Methods

    internal async Task Initialize()
    {
      if (this.isInitialized)
      {
        return;
      }

      ProfileGetData data = await App.Client.ProfileGetAsync();
      this.SetDataModel(data);

      this.isInitialized = true;
    }

    private void AccountViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      if (e.PropertyName == nameof(this.Description) || e.PropertyName == nameof(this.Nickname) || e.PropertyName == nameof(this.IsBusy))
      {
        this.SaveCommand.ChangeCanExecute();
      }
    }

    private bool SaveCommandCanExecute()
    {
      return !string.IsNullOrEmpty(this.Nickname) &&
            this.Nickname.Length > 3 &&
            !string.IsNullOrEmpty(this.Description) &&
            !this.IsBusy;
    }

    private async Task SaveCommandExecute()
    {
      this.IsBusy = true;

      this.Message = string.Empty;

      try
      {
        ProfilePutData data = this.WriteToDataModel();
        await App.Client.ProfilePutAsync(data);

        await Shell.Current.GoToAsync("..");
      }
      catch (ApiException exception)
      {
        this.Message = exception.ToString();
      }
      finally
      {
        this.IsBusy = true;
      }

      this.IsBusy = false;
    }

    #endregion Methods
  }
}