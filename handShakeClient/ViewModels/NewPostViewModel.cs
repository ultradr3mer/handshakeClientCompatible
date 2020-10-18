using HandshakeClient.Composite;
using HandshakeClient.Services;
using System;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HandshakeClient.ViewModels
{
  public class NewPostViewModel : BaseViewModel
  {
    #region Fields

    private Location propLocation;
    private string propMessage;
    private string propPlaceholder;
    private string propText;
    private readonly LocationCache locationCache;

    #endregion Fields

    #region Constructors

    public NewPostViewModel(LocationCache locationCache)
    {
      this.SaveCommand = new Command(this.SaveCommandExecute, this.SaveCommandCanExecute);
      this.CancelCommand = new Command(this.OnCancel);
      this.PropertyChanged += this.NewPostViewModelPropertyChanged;
      this.locationCache = locationCache;
    }

    private void NewPostViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == nameof(NewPostViewModel.Text)
        || e.PropertyName == nameof(NewPostViewModel.IsBusy))
      {
        this.SaveCommand.ChangeCanExecute();
      }
    }

    #endregion Constructors

    #region Properties

    public Command CancelCommand { get; }

    public string Message
    {
      get { return this.propMessage; }
      set { this.SetProperty(ref this.propMessage, value); }
    }

    public string Placeholder
    {
      get { return this.propPlaceholder; }
      set { this.SetProperty(ref this.propPlaceholder, value); }
    }

    public Command SaveCommand { get; }

    public string Text
    {
      get { return this.propText; }
      set { this.SetProperty(ref this.propText, value); }
    }

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

    private async void OnCancel()
    {
      await Shell.Current.GoToAsync("..");
    }

    private bool SaveCommandCanExecute()
    {
      return !string.IsNullOrWhiteSpace(this.Text)
          && !this.IsBusy;
    }

    private async void SaveCommandExecute()
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

        await App.Client.PostPostAsync(data);

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