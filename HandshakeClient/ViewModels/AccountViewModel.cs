using Android.Graphics;
using HandshakeClient.Composite;
using HandshakeClient.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
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
    private string propMessage;
    private string propNickname;
    private string propSummary;

    #endregion Fields

    #region Constructors

    public AccountViewModel()
    {
      this.SaveCommand = new Command(async () => await this.SaveCommandExecute(), this.SaveCommandCanExecute);
      this.AvatarTappedCommand = new Command(async () => await this.AvatarTappedCommandExecute());

      this.PropertyChanged += this.AccountViewModelPropertyChanged;
    }

    #endregion Constructors

    #region Properties

    public ImageSource AvatarSource
    {
      get { return propAvatarSource; }
      set { SetProperty(ref propAvatarSource, value); }
    }

    public Command AvatarTappedCommand { get; }

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

    protected override void OnReadingDataModel(ProfileGetData data)
    {
      if(!string.IsNullOrEmpty(data.Avatar))
      {
        this.AvatarSource = new Uri("https://handshake.azurewebsites.net/file/" + data.Avatar);
      }
    }

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

    private async Task AvatarTappedCommandExecute()
    {
      await CrossMedia.Current.Initialize();
      if (!CrossMedia.Current.IsPickPhotoSupported)
      {
        this.Message = "This is not support on your device.";
        return;
      }
      else
      {
        var mediaOption = new PickMediaOptions()
        {
          PhotoSize = PhotoSize.Medium
        };
        this.mediaFile = await CrossMedia.Current.PickPhotoAsync();
        if (mediaFile == null) return;
        this.AvatarSource = ImageSource.FromStream(() => mediaFile.GetStream());
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

        if (this.mediaFile != null)
        {
          var file = new FileParameter(this.mediaFile.GetStream(), System.IO.Path.GetFileName(this.mediaFile.Path));
          var result = await App.Client.ProfileAvatarAsync(file);
          this.mediaFile = null;
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