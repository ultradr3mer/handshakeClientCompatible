using HandshakeClient.Composite;
using HandshakeClient.Services;
using Plugin.Media;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HandshakeClient.ViewModels
{
  public class NewPostViewModel : BaseViewModel
  {
    #region Fields

    private readonly LocationCache locationCache;
    private string fileName;

    #endregion Fields

    #region Constructors

    public NewPostViewModel(LocationCache locationCache)
    {
      this.SaveCommand = new Command(this.SaveCommandExecute, this.SaveCommandCanExecute);
      this.CancelCommand = new Command(this.OnCancel);
      this.PickImageCommand = new Command(this.PickImageCommandExecute, this.PickImageCommandCanExecute);
      this.PropertyChanged += this.NewPostViewModelPropertyChanged;
      this.locationCache = locationCache;
    }

    #endregion Constructors

    #region Properties

    public Command CancelCommand { get; }

    public string Message { get; set; }

    public Command PickImageCommand { get; }

    public string Placeholder { get; set; }

    public ImageSource PostImage { get; set; }

    public Command SaveCommand { get; }

    public string Text { get; set; }

    #endregion Properties

    #region Methods

    internal void Initialize()
    {
      string placehoder = GetRandomPlaceholder();
      this.Placeholder = placehoder;
    }

    private static string GetRandomPlaceholder()
    {
      string[] placehoders = new string[]
      {
        "I like unicorns.",
        "Nahrwal swimming in the ocean",
        "First !!1!!!!",
        "We must construct additional pylons!",
        "I yoinked my mind!",
        "Ya yeet!",
      };
      Random rnd = new Random();
      string placehoder = placehoders[rnd.Next(0, placehoders.Length - 1)];
      return placehoder;
    }

    private void NewPostViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == nameof(NewPostViewModel.Text)
        || e.PropertyName == nameof(NewPostViewModel.IsBusy))
      {
        this.SaveCommand.ChangeCanExecute();
        this.PickImageCommand.ChangeCanExecute();
      }
    }

    private async void OnCancel()
    {
      await Shell.Current.GoToAsync("..");
    }

    private bool PickImageCommandCanExecute()
    {
      return !this.IsBusy;
    }

    private async void PickImageCommandExecute()
    {
      await CrossMedia.Current.Initialize();
      if (!CrossMedia.Current.IsPickPhotoSupported)
      {
        this.Message = "This is not support on your device.";
        return;
      }

      var mediaFile = await CrossMedia.Current.PickPhotoAsync();
      if (mediaFile == null)
      {
        return;
      }

      this.PostImage = ImageSource.FromStream(() => mediaFile.GetStream());
      this.fileName = mediaFile.Path;
    }

    private bool SaveCommandCanExecute(object o)
    {
      return !string.IsNullOrWhiteSpace(this.Text)
          && !this.IsBusy;
    }

    private async void SaveCommandExecute(object o)
    {
      this.IsBusy = true;
      this.Message = string.Empty;

      try
      {
        var location = await this.locationCache.GetCurrentLocation(Enums.TimePassed.JustNow);

        PostPostData data = new PostPostData()
        {
          Content = Text,
          Latitude = location.Latitude,
          Longitude = location.Longitude
        };

        var postData = await App.Client.PostPostAsync(data);

        if (o is Stream stream)
        {
          FileParameter file = new FileParameter(stream, System.IO.Path.GetFileName(this.fileName));
          await App.Client.PostImageAsync(postData.Id, file);
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