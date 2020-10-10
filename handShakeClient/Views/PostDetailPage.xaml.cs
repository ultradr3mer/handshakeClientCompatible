using HandshakeClient.ViewModels;
using Xamarin.Forms;

namespace HandshakeClient.Views
{
  public partial class PostDetailPage : ContentPage
  {
    #region Constructors

    public PostDetailPage()
    {
      InitializeComponent();
      this.ViewModel = new PostDetailViewModel();
    }

    #endregion Constructors

    #region Properties

    public PostDetailViewModel ViewModel
    {
      get { return this.BindingContext as PostDetailViewModel; }
      set { this.BindingContext = value; }
    }

    #endregion Properties

    #region Methods

    protected override void OnAppearing()
    {
      this.ViewModel.OnAppearing();
    }

    #endregion Methods
  }
}