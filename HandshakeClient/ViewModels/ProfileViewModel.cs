using HandshakeClient.Composite;
using HandshakeClient.Services;
using System;
using System.Threading.Tasks;
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

    public Uri Avatar { get; set; }

    public Guid Id { get; set; }

    public string IdString { get; set; }

    public string Message { get; set; }

    public string Nickname { get; set; }

    #endregion Properties

    #region Methods

    internal async Task InitializeAsync()
    {
      this.IsBusy = true;

      try
      {
        var profile = await App.Client.ProfileGetAsync(this.Id);

        this.SetDataModel(profile);
        this.Avatar = SimpleFileTokenData.CreateUrl(profile.Avatar);
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