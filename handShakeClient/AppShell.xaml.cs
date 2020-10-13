using HandshakeClient.Views;
using System;
using Xamarin.Forms;

namespace HandshakeClient
{
  public partial class AppShell : Shell
  {
    #region Constructors

    public AppShell()
    {
      InitializeComponent();
      RegisterRoutes();
      BindingContext = this;
    }

    #endregion Constructors

    #region Methods

    private void RegisterRoutes()
    {
      Routing.RegisterRoute(nameof(PostDetailPage), typeof(PostDetailPage));
      Routing.RegisterRoute(nameof(NewPostPage), typeof(NewPostPage));
      Routing.RegisterRoute(nameof(SignupPage), typeof(SignupPage));
    }

    #endregion Methods
  }
}