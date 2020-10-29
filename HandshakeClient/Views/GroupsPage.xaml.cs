using HandshakeClient.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandshakeClient.Views
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class GroupsPage : ContentPage
  {
    #region Constructors

    public GroupsPage()
    {
      InitializeComponent();

      this.ViewModel = App.Resolve<GroupsViewModel>();
    }

    #endregion Constructors

    #region Properties

    internal GroupsViewModel ViewModel
    {
      get { return this.BindingContext as GroupsViewModel; }

      private set { this.BindingContext = value; }
    }

    #endregion Properties

    #region Methods

    protected override void OnAppearing()
    {
      this.ViewModel.Initialize();
    }

    private void ToolbarAddItemClicked(object sender, System.EventArgs e)
    {
      this.ViewModel.AddItemCommand.Execute(null);
    }

    #endregion Methods
  }
}