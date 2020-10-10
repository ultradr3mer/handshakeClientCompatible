using HandshakeClient.ViewModels;
using Xamarin.Forms;

namespace HandshakeClient.Views
{
  public partial class NewPostPage : ContentPage
  {
    #region Constructors

    public NewPostPage()
    {
      InitializeComponent();
      BindingContext = App.Resolve<NewPostViewModel>();
    }

    #endregion Constructors

    #region Properties

    public NewPostViewModel ViewModel
    {
      get { return this.BindingContext as NewPostViewModel; }
      set { this.BindingContext = value; }
    }

    #endregion Properties

    #region Methods

    protected override void OnAppearing()
    {
      base.OnAppearing();

      this.ViewModel.Initialize();
    }

    #endregion Methods
  }
}