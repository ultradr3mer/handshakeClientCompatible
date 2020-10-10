using HandshakeClient.Composite;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HandshakeClient.ViewModels
{
  public class AboutViewModel : BaseViewModel
  {
    public AboutViewModel()
    {
      Title = "About";
      OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://github.com/ultradr3mer?tab=repositories&q=handshake"));
    }

    public ICommand OpenWebCommand { get; }
  }
}