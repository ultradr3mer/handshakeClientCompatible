using HandshakeClient.Composite;
using HandshakeClient.Extensions;
using HandshakeClient.Services;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace HandshakeClient.ViewModels
{
  [QueryProperty(nameof(Id), nameof(Id))]
  public class PostDetailViewModel : BaseViewModel
  {
    #region Fields

    private Guid idGuid;

    #endregion Fields

    #region Constructors

    public PostDetailViewModel()
    {
      this.Replys = new ObservableCollection<ReplyEntryViewModel>();
      this.RefreshCommand = new Command(this.RefreshCommandExecute);
      this.CancelCommand = new Command(this.CancelCommandExecute);
      this.PostReplyCommand = new Command(this.PostReplyCommandExecute, this.PostReplyCommandCanExecute);
      this.PropertyChanged += this.PostDetailViewModelPropertyChanged;
    }

    #endregion Constructors

    #region Properties

    public ImageSource Avatar { get; set; }

    public Command CancelCommand { get; }

    public string Content { get; set; }

    public string Id { get; set; }

    public ImageSource Image { get; set; }

    public bool IsPostingRepy { get; set; }

    public string Message { get; set; }

    public string NewReplyText { get; set; }

    public Command PostReplyCommand { get; }

    public string PostTitle { get; set; }

    public Command RefreshCommand { get; }

    public ObservableCollection<ReplyEntryViewModel> Replys { get; set; }

    #endregion Properties

    #region Methods

    public async void LoadItemById(Guid itemId)
    {
      this.IsBusy = true;

      try
      {
        PostDetailGetData post = await App.Client.PostGetAsync(itemId);

        this.Content = post.Content;
        this.PostTitle = $"{post.AuthorName} wrote {post.TimeAgo.ToStringForHumans()} ago";
        this.Avatar = SimpleFileTokenData.CreateUrl(post.Avatar);
        this.Image = SimpleFileTokenData.CreateUrl(post.Image);
        this.Replys.Clear();
        ObservableCollection<PostReplyGetData> replys = new ObservableCollection<PostReplyGetData>(post.Replys);
        foreach (PostReplyGetData item in replys)
        {
          this.Replys.Add(new ReplyEntryViewModel().GetWithDataModel(item));
        }
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

    public void Initialize()
    {
      this.IsBusy = true;
    }

    private async void CancelCommandExecute(object obj)
    {
      await Shell.Current.GoToAsync("..");
    }

    private void PostDetailViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      if (e.PropertyName == nameof(this.NewReplyText) || e.PropertyName == nameof(this.IsPostingRepy))
      {
        this.PostReplyCommand.ChangeCanExecute();
      }
      else if (e.PropertyName == nameof(this.Id))
      {
        this.idGuid = Guid.Parse(this.Id);
      }
    }

    private bool PostReplyCommandCanExecute()
    {
      return !string.IsNullOrEmpty(this.NewReplyText) && !this.IsPostingRepy;
    }

    private async void PostReplyCommandExecute()
    {
      if (this.IsPostingRepy)
      {
        return;
      }

      this.IsPostingRepy = true;

      try
      {
        ReplyPostData post = new ReplyPostData()
        {
          Content = this.NewReplyText,
          Post = this.idGuid
        };
        await App.Client.ReplyAsync(post);

        this.Replys.Clear();
        this.NewReplyText = string.Empty;
      }
      catch (Exception exception)
      {
        this.Message = exception.ToString();
      }
      finally
      {
        this.IsPostingRepy = false;
      }

      this.LoadItemById(this.idGuid);
    }

    private void RefreshCommandExecute()
    {
      this.LoadItemById(this.idGuid);
    }

    #endregion Methods
  }
}