using HandshakeClient.ViewModels;
using Xamarin.Forms;

namespace HandshakeClient.Views
{
  public partial class PostsPage : ContentPage
  {
    #region Constructors

    public PostsPage()
    {
      InitializeComponent();

      this.ViewModel = App.Resolve<PostsViewModel>();
    }

    #endregion Constructors

    #region Properties

    public PostsViewModel ViewModel
    {
      get { return this.BindingContext as PostsViewModel; }
      set { this.BindingContext = value; }
    }

    #endregion Properties

    #region Methods

    protected override void OnAppearing()
    {
      this.ViewModel.OnAppearing();
    }

    private void ToolbarAddItemClicked(object sender, System.EventArgs e)
    {
      // Invoke over the clicked event because the command is invoked twice.
      this.ViewModel.AddItemCommand.Execute(null);
    }

    #endregion Methods
  }
}