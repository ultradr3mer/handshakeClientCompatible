using HandshakeClient.Composite;
using HandshakeClient.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HandshakeClient.ViewModels
{
  public class AccountViewModel : BaseViewModel<ProfileGetData, ProfilePutData>
  {
    #region Fields

    private bool isInitialized = false;
    private MediaFile mediaFile;
    private ImageSource propAvatarSource;
    private bool propIsCropViewEnabled;
    private string propMessage;
    private string propNickname;
    private string propSummary;

    #endregion Fields

    #region Constructors

    public AccountViewModel()
    {
      this.SaveCommand = new Command(this.SaveCommandExecute, this.SaveCommandCanExecute);
      this.AvatarTappedCommand = new Command(this.AvatarTappedCommandExecute);

      this.PropertyChanged += this.AccountViewModelPropertyChanged;
    }

    #endregion Constructors

    #region Properties

    public ImageSource AvatarSource
    {
      get { return this.propAvatarSource; }
      set { this.SetProperty(ref this.propAvatarSource, value); }
    }

    public Command AvatarTappedCommand { get; }

    public string Description
    {
      get { return this.propSummary; }
      set { this.SetProperty(ref this.propSummary, value); }
    }

    public bool IsCropViewEnabled
    {
      get { return this.propIsCropViewEnabled; }
      set { this.SetProperty(ref this.propIsCropViewEnabled, value); }
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
      ProfileGetData data = await App.Client.ProfileGetAsync();
      this.SetDataModel(data);

      this.isInitialized = true;
    }

    protected override void OnReadingDataModel(ProfileGetData data)
    {
      this.AvatarSource = SimpleFileTokenData.CreateUrl(data.Avatar);
    }

    private void AccountViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      if (e.PropertyName == nameof(this.Description) || e.PropertyName == nameof(this.Nickname) || e.PropertyName == nameof(this.IsBusy))
      {
        this.SaveCommand.ChangeCanExecute();
      }
    }

    private async void AvatarTappedCommandExecute()
    {
      await CrossMedia.Current.Initialize();
      if (!CrossMedia.Current.IsPickPhotoSupported)
      {
        this.Message = "This is not support on your device.";
        return;
      }
      else
      {
        this.mediaFile = await CrossMedia.Current.PickPhotoAsync();
        if (this.mediaFile == null)
        {
          return;
        }

        this.AvatarSource = ImageSource.FromStream(() => this.mediaFile.GetStream());

        this.IsCropViewEnabled = true;
      }
    }

    private bool SaveCommandCanExecute(object o)
    {
      return !string.IsNullOrEmpty(this.Nickname) &&
            this.Nickname.Length > 3 &&
            !string.IsNullOrEmpty(this.Description) &&
            !this.IsBusy;
    }

    private async void SaveCommandExecute(object o)
    {
      this.IsBusy = true;

      this.Message = string.Empty;

      try
      {
        ProfilePutData data = this.WriteToDataModel();
        await App.Client.ProfilePutAsync(data);

        if (this.mediaFile != null)
        {
          FileParameter file = new FileParameter((Stream)o, System.IO.Path.GetFileName(this.mediaFile.Path));
          await App.Client.ProfileAvatarAsync(file);
          this.mediaFile = null;
          this.IsCropViewEnabled = false;
        }

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