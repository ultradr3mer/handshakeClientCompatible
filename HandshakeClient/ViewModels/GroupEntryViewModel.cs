using HandshakeClient.Composite;
using HandshakeClient.Services;
using Xamarin.Forms;

namespace HandshakeClient.ViewModels
{
  internal class GroupEntryViewModel : BaseViewModel<GroupGetData>
  {
    #region Constructors

    public GroupEntryViewModel()
    {
      this.TappedCommand = new Command(this.TappedCommandExecute);
    }

    #endregion Constructors

    #region Properties

    public string Description { get; set; }

    public ImageSource GroupIcon { get; set; }

    public string Name { get; set; }

    public Command TappedCommand { get; }

    #endregion Properties

    #region Methods

    protected override void OnReadingDataModel(GroupGetData data)
    {
      this.GroupIcon = SimpleFileTokenData.CreateUrl(data.Icon);
    }

    private void TappedCommandExecute(object obj)
    {
    }

    #endregion Methods
  }
}