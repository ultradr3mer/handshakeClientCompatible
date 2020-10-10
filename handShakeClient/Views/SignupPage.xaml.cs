using HandshakeClient.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandshakeClient.Views
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class SignupPage : ContentPage
  {
    #region Constructors

    public SignupPage()
    {
      InitializeComponent();
      this.BindingContext = new SignupViewModel();
    }

    #endregion Constructors
  }
}