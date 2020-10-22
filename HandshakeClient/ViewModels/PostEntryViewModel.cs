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
    #region Fields

    private ImageSource propAvatar;
    private string propContent;
    private Guid propId;
    private ImageSource propImage;
    private string propPostTitle;

    private int propReplyCount;

    #endregion Fields

    #region Constructors

    public PostEntryViewModel()
    {
      this.Tapped = new Command(this.TappedExecute);
    }

    #endregion Constructors

    #region Properties

    public ImageSource Avatar
    {
      get { return propAvatar; }
      set { SetProperty(ref propAvatar, value); }
    }

    public string Content
    {
      get { return propContent; }
      set { SetProperty(ref propContent, value); }
    }

    public Guid Id
    {
      get { return propId; }
      set { SetProperty(ref propId, value); }
    }

    public ImageSource Image
    {
      get { return propImage; }
      set { SetProperty(ref propImage, value); }
    }

    public string PostTitle
    {
      get { return this.propPostTitle; }
      set { this.SetProperty(ref this.propPostTitle, value); }
    }

    public int ReplyCount
    {
      get { return propReplyCount; }
      set { SetProperty(ref propReplyCount, value); }
    }

    public Command Tapped { get; }

    #endregion Properties

    #region Methods

    protected override void OnReadingDataModel(PostGetData data)
    {
      this.PostTitle = $"{data.AuthorName} wrote {data.TimeAgo.ToStringForHumans()} ago";
      this.Avatar = SimpleFileTokenData.CreateUrl(data.Avatar);
      this.Image = SimpleFileTokenData.CreateUrl(data.Image);
    }

    private async void TappedExecute()
    {
      await Shell.Current.GoToAsync($"{nameof(PostDetailPage)}?{nameof(PostDetailViewModel.Id)}={this.Id}");
    }

    #endregion Methods
  }
}