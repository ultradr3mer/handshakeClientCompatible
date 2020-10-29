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

    public ImageSource IconNewCrop { get; set; }

    public ImageSource IconNewAnimated { get; set; }

    public ImageSource IconCurrent { get; set; }

    public Command AvatarTappedCommand { get; }

    public string Description { get; set; }

    public string Message { get; set; }

    public string Nickname { get; set; }

    public Command SaveCommand { get; }


    #endregion Properties

    #region Methods

    internal async Task Initialize()
    {
      if (!this.isInitialized)
      {
        ProfileGetData data = await App.Client.ProfileGetAsync();
        this.SetDataModel(data);
        this.isInitialized = true;
      }
    }

    protected override void OnReadingDataModel(ProfileGetData data)
    {
      this.IconCurrent = SimpleFileTokenData.CreateUrl(data.Avatar);
      this.IconNewAnimated = null;
      this.IconNewCrop = null;
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

        var source = ImageSource.FromStream(() => this.mediaFile.GetStream());
        if (Util.IsGif(this.mediaFile.Path))
        {
          this.IconCurrent = null;
          this.IconNewAnimated = source;
          this.IconNewCrop = null;
        }
        else
        {
          this.IconCurrent = null;
          this.IconNewAnimated = null;
          this.IconNewCrop = source;
        }
      }
    }

    private bool SaveCommandCanExecute(object o)
    {
      return !string.IsNullOrEmpty(this.Nickname) &&
             this.Nickname.Length > 3 &&
             !string.IsNullOrEmpty(this.Description) &&
             !this.IsBusy;
    }

    private async void SaveCommandExecute(object obj)
    {
      this.IsBusy = true;

      this.Message = string.Empty;

      try
      {
        ProfilePutData data = this.WriteToDataModel();
        await App.Client.ProfilePutAsync(data);

        FileParameter file = null;
        if (obj is Stream stream)
        {
          file = new FileParameter(stream, System.IO.Path.GetFileName(this.mediaFile.Path));
        }
        else if(this.IconNewAnimated != null)
        {
          file = new FileParameter(this.mediaFile.GetStream(), System.IO.Path.GetFileName(this.mediaFile.Path));
        }

        if (file != null)
        {
          var avatarData = await App.Client.ProfileAvatarAsync(file);
          this.mediaFile = null;

          this.IconCurrent = SimpleFileTokenData.CreateUrl(avatarData.LocalUrl);
          this.IconNewAnimated = null;
          this.IconNewCrop = null;
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