using HandshakeClient.Composite;
using HandshakeClient.Extensions;
using HandshakeClient.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace HandshakeClient.ViewModels
{
  [QueryProperty(nameof(IdString), ProfileViewModel.IdQueryId)]
  public class ProfileViewModel : BaseViewModel<ProfileGetData>
  {
    #region Fields

    public const string IdQueryId = "Id";

    #endregion Fields

    #region Constructors

    public ProfileViewModel()
    {
      this.PropertyChanged += this.ProfileViewModel_PropertyChanged;
    }

    #endregion Constructors

    #region Properties

    public ImageSource Avatar { get; set; }
    public string Description { get; set; }
    public ObservableCollection<GroupEntryViewModel> Groups { get; set; } = new ObservableCollection<GroupEntryViewModel>();
    public Guid Id { get; set; }
    public string IdString { get; set; }
    public string Message { get; set; }
    public string Nickname { get; set; }

    #endregion Properties

    #region Methods

    internal async void Initialize()
    {
      this.IsBusy = true;

      try
      {
        ProfileGetData profile = await App.Client.ProfileGetAsync(this.Id);

        this.SetDataModel(profile);
        this.Avatar = SimpleFileTokenData.CreateUrl(profile.Avatar);
        this.Groups.Clear();
        this.Groups.AddRangeSequential(profile.Groups.Select(g => new GroupEntryViewModel().GetWithDataModel(g)));

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

    private void ProfileViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      if (e.PropertyName == nameof(this.IdString))
      {
        this.Id = Guid.Parse(this.IdString);
      }
    }

    #endregion Methods
  }
}