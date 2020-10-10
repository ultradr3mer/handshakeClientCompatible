using HandshakeClient.Composite;
using HandshakeClient.Enums;
using HandshakeClient.Services;
using HandshakeClient.Views;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
      this.Posts = new ObservableCollection<PostGetData>();
      this.LoadItemsCommand = new Command(async () => await this.ExecuteLoadItemsCommand());

      this.ItemTapped = new Command<PostGetData>(this.OnPostSelected);

      this.AddItemCommand = new Command(this.OnAddItem);
      this.locationCache = locationCache;
    }

    #endregion Constructors

    #region Properties

    public Command AddItemCommand { get; }
    public Command<PostGetData> ItemTapped { get; }
    public Command LoadItemsCommand { get; }

    public string Message
    {
      get { return this.propMessage; }
      set { this.SetProperty(ref this.propMessage, value); }
    }

    public ObservableCollection<PostGetData> Posts { get; }

    public PostGetData SelectedPost
    {
      get
      {
        return this.propSelectedPost;
      }

      set
      {
        this.SetProperty(ref this.propSelectedPost, value);
        this.OnPostSelected(value);
      }
    }

    #endregion Properties

    #region Methods

    public void OnAppearing()
    {
      this.IsBusy = true;
      this.SelectedPost = null;
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
          this.Posts.Add(item);
        }
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

    private async void OnAddItem(object obj)
    {
      await Shell.Current.GoToAsync(nameof(NewPostPage));
    }

    private async void OnPostSelected(PostGetData item)
    {
      if (item == null)
      {
        return;
      }

      await Shell.Current.GoToAsync($"{nameof(PostDetailPage)}?{nameof(PostDetailViewModel.Id)}={item.Id}");
    }

    #endregion Methods
  }
}