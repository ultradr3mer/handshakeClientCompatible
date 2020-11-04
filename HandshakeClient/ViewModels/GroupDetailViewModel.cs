using HandshakeClient.Composite;
using HandshakeClient.Services;
using System;
using Xamarin.Forms;

namespace HandshakeClient.ViewModels
{
  [QueryProperty(nameof(Id), nameof(Id))]
  [QueryProperty(nameof(Name), nameof(Name))]
  public class GroupDetailViewModel : BaseViewModel<GroupGetData>
  {
    #region Fields

    private Guid? idGuid;

    #endregion Fields

    #region Constructors

    public GroupDetailViewModel()
    {
      this.PropertyChanged += this.GroupDetailsViewModelPropertyChanged;

      this.RefreshCommand = new Command(this.RefreshCommandExecute);
    }

    #endregion Constructors

    #region Properties

    public string Description { get; set; }

    public ImageSource Icon { get; set; }

    public string Id { get; set; }

    public string Message { get; set; }

    public string Name { get; set; }

    public Command RefreshCommand { get; }

    #endregion Properties

    #region Methods

    public async void LoadItem()
    {
      this.IsBusy = true;

      try
      {
        GroupGetData group = await App.Client.GroupGetAsync(this.idGuid, this.Name);
        this.SetDataModel(group);
        this.Icon = SimpleFileTokenData.CreateUrl(group.Icon);
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

    internal void Initialize()
    {
      this.IsBusy = true;
    }

    private void GroupDetailsViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      if (e.PropertyName == nameof(this.Id))
      {
        this.idGuid = Guid.Parse(this.Id);
      }
    }

    private void RefreshCommandExecute()
    {
      this.LoadItem();
    }

    #endregion Methods
  }
}