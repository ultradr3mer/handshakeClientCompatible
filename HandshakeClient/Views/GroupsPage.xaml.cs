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

      this.BindingContext = App.Resolve<GroupsViewModel>();
    }

    #endregion Constructors
  }
}