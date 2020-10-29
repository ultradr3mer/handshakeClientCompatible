using HandshakeClient.Composite;
using HandshakeClient.Services;
using Plugin.Media;
using System;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HandshakeClient.ViewModels
{
  internal class GroupNewViewModel : BaseViewModel<GroupPostData>
  {
    #region Fields

    private readonly LocationCache locationCache;
    private Plugin.Media.Abstractions.MediaFile mediaFile;

    #endregion Fields

    #region Constructors

    public GroupNewViewModel(LocationCache locationCache)
    {
      this.locationCache = locationCache;

      this.PickIconCommand = new Command(this.PickIconCommandExecute);
      this.SaveGroupCommand = new Command(this.SaveGroupCommandExecuteAsync);
    }

    #endregion Constructors

    #region Properties

    public string Description { get; set; }

    public ImageSource IconAnimated { get; set; }

    public ImageSource IconCrop { get; set; }

    public string Message { get; set; }

    public string Name { get; set; }

    public Command PickIconCommand { get; }

    public Command PickIconGifCommand { get; }

    public Command SaveGroupCommand { get; }

    #endregion Properties

    #region Methods

    private async void PickIconCommandExecute()
    {
      await CrossMedia.Current.Initialize();
      if (!CrossMedia.Current.IsPickPhotoSupported)
      {
        this.Message = "This is not support on your device.";
        return;
      }

      this.mediaFile = await CrossMedia.Current.PickPhotoAsync();
      if (this.mediaFile == null)
      {
        return;
      }

      ImageSource stream = ImageSource.FromStream(() => this.mediaFile.GetStream());
      if (Util.IsGif(this.mediaFile.Path))
      {
        this.IconAnimated = stream;
        this.IconCrop = null;
      }
      else
      {
        this.IconAnimated = null;
        this.IconCrop = stream;
      }
    }

    private async void SaveGroupCommandExecuteAsync(object obj)
    {
      this.IsBusy = true;
      this.Message = string.Empty;

      try
      {
        Location location = await this.locationCache.GetCurrentLocation(Enums.TimePassed.JustNow);

        GroupPostData data = new GroupPostData()
        {
          Name = this.Name,
          Description = this.Description
        };

        GroupPostResultData postData = await App.Client.GroupPostAsync(data);

        if (obj is Stream stream)
        {
          FileParameter file = new FileParameter(stream, Path.GetFileName(this.mediaFile.Path));
          await App.Client.GroupIconAsync(postData.Id, file);
        }
        else if (this.mediaFile != null)
        {
          FileParameter file = new FileParameter(this.mediaFile.GetStream(), Path.GetFileName(this.mediaFile.Path));
          await App.Client.GroupIconAsync(postData.Id, file);
        }

        await Shell.Current.GoToAsync("..");
      }
      catch (Exception exception)
      {
        this.Message = exception.ToString();
      }
      finally
      {
        this.IsBusy = false;
      }
    }

    #endregion Methods
  }
}