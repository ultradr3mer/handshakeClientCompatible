using HandshakeClient.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandshakeClient.Views
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class GroupDetailPage : ContentPage
  {
    #region Constructors

    public GroupDetailPage()
    {
      InitializeComponent();

      this.ViewModel = App.Resolve<GroupDetailViewModel>();
    }

    #endregion Constructors

    #region Properties

    public GroupDetailViewModel ViewModel
    {
      get { return this.BindingContext as GroupDetailViewModel; }
      set { this.BindingContext = value; }
    }

    #endregion Properties

    #region Methods

    protected override void OnAppearing()
    {
      this.ViewModel.Initialize();
    }

    #endregion Methods
  }
}