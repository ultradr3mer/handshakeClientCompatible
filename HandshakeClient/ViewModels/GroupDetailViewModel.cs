using HandshakeClient.Composite;
using HandshakeClient.Extensions;
using HandshakeClient.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace HandshakeClient.ViewModels
{
  [QueryProperty(nameof(Id), nameof(Id))]
  [QueryProperty(nameof(Name), nameof(Name))]
  public class GroupDetailViewModel : BaseViewModel<GroupGetData>
  {
    #region Fields

    private readonly AccountViewModel accountViewModel;

    private Guid? idGuid;

    #endregion Fields

    #region Constructors

    public GroupDetailViewModel(AccountViewModel accountViewModel)
    {
      this.PropertyChanged += this.GroupDetailsViewModelPropertyChanged;

      this.RefreshCommand = new Command(this.RefreshCommandExecute);
      this.JoinLeaveCommand = new Command(this.JoinLeaveCommandExecute);

      this.accountViewModel = accountViewModel;
    }

    #endregion Constructors

    #region Properties

    public string Description { get; set; }
    public ImageSource Icon { get; set; }
    public string Id { get; set; }
    public bool IsJoined { get; set; }
    public Command JoinLeaveCommand { get; }
    public string JoinLeaveText { get; set; }
    public string Message { get; set; }
    public string Name { get; set; }
    public ObservableCollection<PostEntryViewModel> Posts { get; set; } = new ObservableCollection<PostEntryViewModel>();
    public Command RefreshCommand { get; }

    #endregion Properties

    #region Methods

    public async void LoadItem()
    {
      this.IsBusy = true;

      try
      {
        await this.accountViewModel.Initialize();
        GroupGetData group = await App.Client.GroupGetAsync(this.idGuid, this.Name);
        this.IsJoined = group.Users.Any(o => o.Id == this.accountViewModel.Id);
        this.UpdateJoinLeaveText();
        this.SetDataModel(group);
        this.Posts.Clear();
        this.Posts.AddRangeSequential(group.Posts.Select(p => new PostEntryViewModel().GetWithDataModel(p)));
        this.Icon = SimpleFileTokenData.CreateUrl(group.Icon);
        this.idGuid = group.Id;
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
      else if (e.PropertyName == nameof(this.IsJoined))
      {
        this.UpdateJoinLeaveText();
      }
    }

    private async void JoinLeaveCommandExecute(object obj)
    {
      this.IsBusy = true;

      try
      {
        if (this.IsJoined)
        {
          await App.Client.GroupDissociateuserAsync(this.idGuid);
          this.IsJoined = false;
        }
        else
        {
          await App.Client.GroupAssociateuserAsync(this.idGuid);
          this.IsJoined = true;
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

    private void RefreshCommandExecute()
    {
      this.LoadItem();
    }

    private void UpdateJoinLeaveText()
    {
      this.JoinLeaveText = this.IsJoined ? "Leave" : "Join";
    }

    #endregion Methods
  }
}