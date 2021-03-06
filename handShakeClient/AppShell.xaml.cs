﻿using HandshakeClient.Views;
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
      Routing.RegisterRoute(nameof(PostNewPage), typeof(PostNewPage));
      Routing.RegisterRoute(nameof(SignupPage), typeof(SignupPage));
      Routing.RegisterRoute(nameof(GroupNewPage), typeof(GroupNewPage));
      Routing.RegisterRoute(nameof(GroupDetailPage), typeof(GroupDetailPage));
      Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
    }

    #endregion Methods
  }
}