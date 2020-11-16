using HandshakeClient.Composite;
using HandshakeClient.Extensions;
using HandshakeClient.Services;
using HandshakeClient.Views;
using System;
using Xamarin.Forms;

namespace HandshakeClient.ViewModels
{
  public class PostEntryViewModel : BaseViewModel<PostGetData>
  {
    #region Constructors

    public PostEntryViewModel()
    {
      this.Tapped = new Command(this.TappedExecute);
      this.AvatarTapped = new Command(this.AvatarTappedExecute);
    }

    #endregion Constructors

    #region Properties

    public ImageSource Avatar { get; set; }
    public Command AvatarTapped { get; }
    public string Content { get; set; }
    public Guid Id { get; set; }
    public ImageSource Image { get; set; }
    public string PostTitle { get; set; }
    public int ReplyCount { get; set; }
    public Command Tapped { get; }
    public Guid AuthorId { get; set; }

    #endregion Properties

    #region Methods

    protected override void OnReadingDataModel(PostGetData data)
    {
      this.PostTitle = $"{data.AuthorName} wrote {data.TimeAgo.ToStringForHumans()} ago";
      this.Avatar = SimpleFileTokenData.CreateUrl(data.Avatar);
      this.Image = SimpleFileTokenData.CreateUrl(data.Image);
    }

    private async void AvatarTappedExecute(object obj)
    {
      await Shell.Current.GoToAsync($"{nameof(ProfilePage)}?{ProfileViewModel.IdQueryId}={this.AuthorId}");
    }

    private async void TappedExecute()
    {
      await Shell.Current.GoToAsync($"{nameof(PostDetailPage)}?{nameof(PostDetailViewModel.Id)}={this.Id}");
    }

    #endregion Methods
  }
}