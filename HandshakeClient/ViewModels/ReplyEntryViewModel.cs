using HandshakeClient.Composite;
using HandshakeClient.Extensions;
using HandshakeClient.Services;

namespace HandshakeClient.ViewModels
{
  public class ReplyEntryViewModel : BaseViewModel<PostReplyGetData>
  {
    #region Fields

    private string propContent;
    private string propReplyTitle;

    #endregion Fields

    #region Properties

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
    }

    #endregion Methods
  }
}