using HandshakeClient.Composite;
using HandshakeClient.Extensions;
using HandshakeClient.Services;
using HandshakeClient.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        ICollection<GroupGetData> items = await App.Client.GroupGetallgroupsAsync();
        this.Groups.Clear();
        this.Groups.AddRangeSequential(items.Select(g => new GroupEntryViewModel().GetWithDataModel(g)));
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