using HandshakeClient.Composite;
using HandshakeClient.Services;
using HandshakeClient.Views;
using System;
using Xamarin.Forms;

namespace HandshakeClient.ViewModels
{
  public class GroupEntryViewModel : BaseViewModel<GroupGetData>
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

    public Guid Id { get; set; }

    #endregion Properties

    #region Methods

    protected override void OnReadingDataModel(GroupGetData data)
    {
      this.GroupIcon = SimpleFileTokenData.CreateUrl(data.Icon);
    }

    private async void TappedCommandExecute(object obj)
    {
      await Shell.Current.GoToAsync($"{nameof(GroupDetailPage)}?{nameof(GroupDetailViewModel.Id)}={this.Id}");
    }

    #endregion Methods
  }
}