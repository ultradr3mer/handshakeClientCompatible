using HandshakeClient.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HandshakeClient.Views
{
  public partial class PostNewPage : ContentPage
  {
    #region Fields

    private Command saveButtonCommand;

    #endregion Fields

    #region Constructors

    public PostNewPage()
    {
      InitializeComponent();
      this.ViewModel = App.Resolve<PostNewViewModel>();
      this.ViewModel.SaveCommand.CanExecuteChanged += this.SaveCommandCanExecuteChanged;

      this.saveButtonCommand = new Command(this.SaveButtonCommandExecute, this.SaveButtonCommandCanExecute);
      this.SaveButton.Command = this.saveButtonCommand;
    }

    #endregion Constructors

    #region Properties

    public PostNewViewModel ViewModel
    {
      get { return this.BindingContext as PostNewViewModel; }

      set { this.BindingContext = value; }
    }

    #endregion Properties

    #region Methods

    protected override void OnAppearing()
    {
      base.OnAppearing();

      this.ViewModel.Initialize();
    }

    private void ImageCropSizeChanged(object sender, EventArgs e)
    {
      this.imageCrop.HeightRequest = this.imageCrop.Width / 4.0 * 3.0;
    }

    private bool SaveButtonCommandCanExecute()
    {
      return this.ViewModel.SaveCommand.CanExecute(null);
    }

    private async void SaveButtonCommandExecute()
    {
      System.IO.Stream image = null;
      if (this.imageCrop.Source != null)
      {
        image = await this.imageCrop.GetImageAsJpegAsync(quality: 90, maxWidth: 400, maxHeight: 300);
      }
      this.ViewModel.SaveCommand.Execute(image);
    }

    private void SaveCommandCanExecuteChanged(object sender, EventArgs e)
    {
      this.saveButtonCommand.ChangeCanExecute();
    }

    #endregion Methods
  }
}