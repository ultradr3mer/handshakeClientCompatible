using HandshakeClient.Composite;
using HandshakeClient.Enums;
using HandshakeClient.Extensions;
using HandshakeClient.Services;
using HandshakeClient.Views;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Unity;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HandshakeClient.ViewModels
{
  public class PostsViewModel : BaseViewModel
  {
    #region Fields

    private readonly LocationCache locationCache;
    private string propMessage;
    private PostGetData propSelectedPost;

    #endregion Fields

    #region Constructors

    public PostsViewModel(LocationCache locationCache)
    {
      this.Title = "Browse";
      this.Posts = new ObservableCollection<PostEntryViewModel>();
      this.LoadItemsCommand = new Command(async () => await this.ExecuteLoadItemsCommand());
      this.AddItemCommand = new Command(this.OnAddItem);
      this.locationCache = locationCache;
    }

    #endregion Constructors

    #region Properties

    public Command AddItemCommand { get; }
    public Command LoadItemsCommand { get; }

    public string Message
    {
      get { return this.propMessage; }
      set { this.SetProperty(ref this.propMessage, value); }
    }

    public ObservableCollection<PostEntryViewModel> Posts { get; }

    #endregion Properties

    #region Methods

    public void OnAppearing()
    {
      this.IsBusy = true;
    }

    private async Task ExecuteLoadItemsCommand()
    {
      this.IsBusy = true;

      try
      {
        this.Posts.Clear();
        Location location = await this.locationCache.GetCurrentLocation(TimePassed.AMomentAgo);
        System.Collections.Generic.ICollection<PostGetData> items = await App.Client.PostGetclosepostsAsync(location.Latitude, location.Longitude);
        foreach (PostGetData item in items)
        {
          this.Posts.Add(new PostEntryViewModel().GetWithDataModel(item));
        }
        this.Message = null;
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

    private async void OnAddItem(object obj)
    {
      await Shell.Current.GoToAsync(nameof(NewPostPage));
    }

    #endregion Methods
  }
}