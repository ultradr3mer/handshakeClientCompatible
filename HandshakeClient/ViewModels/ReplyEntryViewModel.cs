using HandshakeClient.Composite;
using HandshakeClient.Extensions;
using HandshakeClient.Services;
using Xamarin.Forms;

namespace HandshakeClient.ViewModels
{
  public class ReplyEntryViewModel : BaseViewModel<PostReplyGetData>
  {
    #region Fields

    private ImageSource propAvatar;
    private string propContent;
    private string propReplyTitle;

    #endregion Fields

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

    public string ReplyTitle
    {
      get { return propReplyTitle; }
      set { SetProperty(ref propReplyTitle, value); }
    }

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