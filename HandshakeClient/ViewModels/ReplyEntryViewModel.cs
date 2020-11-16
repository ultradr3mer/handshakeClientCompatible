using HandshakeClient.Composite;
using HandshakeClient.Extensions;
using HandshakeClient.Services;
using HandshakeClient.Views;
using System;
using Xamarin.Forms;

namespace HandshakeClient.ViewModels
{
  public class ReplyEntryViewModel : BaseViewModel<PostReplyGetData>
  {
    #region Properties

    public ImageSource Avatar { get; set; }
    public Command AvatarTapped { get; private set; }
    public string Content { get; set; }
    public string ReplyTitle { get; set; }
    public Guid Author { get; set; }

    #endregion Properties

    #region Methods

    protected override void OnReadingDataModel(PostReplyGetData data)
    {
      this.ReplyTitle = $"{data.AuthorName} wrote {data.TimeAgo.ToStringForHumans()} ago";
      this.Avatar = SimpleFileTokenData.CreateUrl(data.Avatar);
      this.AvatarTapped = new Command(this.AvatarTappedExecute);
    }

    private async void AvatarTappedExecute(object obj)
    {
      await Shell.Current.GoToAsync($"{nameof(ProfilePage)}?{ProfileViewModel.IdQueryId}={this.Author}");
    }

    #endregion Methods
  }
}