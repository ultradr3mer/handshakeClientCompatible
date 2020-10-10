using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HandshakeClient.Composite
{
  public class BaseViewModel : INotifyPropertyChanged
  {
    #region Fields

    private bool isBusy = false;
    private string title = string.Empty;

    #endregion Fields

    #region Events

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion Events

    #region Properties

    public bool IsBusy
    {
      get { return this.isBusy; }
      set { this.SetProperty(ref this.isBusy, value); }
    }

    public string Title
    {
      get { return this.title; }
      set { this.SetProperty(ref this.title, value); }
    }

    #endregion Properties

    #region Methods

    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
      PropertyChangedEventHandler changed = PropertyChanged;
      if (changed == null)
      {
        return;
      }

      changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
        Action onChanged = null)
    {
      if (EqualityComparer<T>.Default.Equals(backingStore, value))
      {
        return false;
      }

      backingStore = value;
      onChanged?.Invoke();
      this.OnPropertyChanged(propertyName);
      return true;
    }

    #endregion Methods
  }
}