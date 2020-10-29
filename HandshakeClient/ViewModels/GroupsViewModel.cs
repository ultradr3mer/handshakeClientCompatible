using HandshakeClient.Composite;
using HandshakeClient.Extensions;
using HandshakeClient.Views;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace HandshakeClient.ViewModels
{
  internal class GroupsViewModel : BaseViewModel
  {
    #region Constructors

    public GroupsViewModel()
    {
      this.LoadItemsCommand = new Command(this.LoadItemsCommandExecute);
      this.AddItemCommand = new Command(this.AddItemCommandExecute);
    }

    #endregion Constructors

    #region Properties

    public Command AddItemCommand { get; }

    public ObservableCollection<GroupEntryViewModel> Groups { get; set; } = new ObservableCollection<GroupEntryViewModel>();

    public Command LoadItemsCommand { get; }

    public object Message { get; set; }

    #endregion Properties

    #region Methods

    internal void Initialize()
    {
      this.IsBusy = true;
    }

    private void AddItemCommandExecute(object obj)
    {
      Shell.Current.GoToAsync(nameof(GroupNewPage));
    }

    private async void LoadItemsCommandExecute(object obj)
    {
      this.IsBusy = true;

      try
      {
        this.Groups.Clear();
        System.Collections.Generic.ICollection<Services.GroupGetData> items = await App.Client.GroupGetallgroupsAsync();
        foreach (Services.GroupGetData item in items)
        {
          this.Groups.Add(new GroupEntryViewModel().GetWithDataModel(item));
        }
        this.Message = null;
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

    #endregion Methods
  }
}