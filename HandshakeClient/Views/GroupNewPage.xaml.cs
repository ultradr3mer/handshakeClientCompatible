using HandshakeClient.ViewModels;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandshakeClient.Views
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class GroupNewPage : ContentPage
  {
    #region Constructors

    public GroupNewPage()
    {
      this.InitializeComponent();

      this.saveButton.Clicked += this.SaveButtonClickedAsync;

      this.ViewModel = App.Resolve<GroupNewViewModel>();
    }

    #endregion Constructors

    #region Properties

    internal GroupNewViewModel ViewModel
    {
      get { return this.BindingContext as GroupNewViewModel; }

      private set { this.BindingContext = value; }
    }

    #endregion Properties

    #region Methods

    private async void SaveButtonClickedAsync(object sender, EventArgs e)
    {
      Stream image = null;
      if (this.iconCrop.Source != null)
      {
        image = await this.iconCrop.GetImageAsJpegAsync(quality: 90, maxWidth: 400, maxHeight: 300);
      }
      this.ViewModel.SaveGroupCommand.Execute(image);
    }

    #endregion Methods
  }
}