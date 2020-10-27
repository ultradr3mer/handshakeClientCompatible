using HandshakeClient.Composite;
using HandshakeClient.Extensions;
using HandshakeClient.Services;
using Xamarin.Forms;

namespace HandshakeClient.ViewModels
{
  public class ReplyEntryViewModel : BaseViewModel<PostReplyGetData>
  {
    #region Properties

    public ImageSource Avatar { get; set; }

    public string Content { get; set; }

    public string ReplyTitle { get; set; }

    #endregion Properties

    #region Methods

    protected override void OnReadingDataModel(PostReplyGetData data)
    {
      this.ReplyTitle = $"{data.AuthorName} wrote {data.TimeAgo.ToStringForHumans()} ago";
      this.Avatar = SimpleFileTokenData.CreateUrl(data.Avatar);
    }

    #endregion Methods
  }
}