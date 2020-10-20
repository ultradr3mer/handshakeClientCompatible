using System;

namespace HandshakeClient.Services
{
  public static class SimpleFileTokenData
  {
    #region Fields

    private const string BaseUrl = "https://handshake.azurewebsites.net/file/";

    #endregion Fields

    #region Methods

    public static Uri CreateUrl(string url)
    {
      return string.IsNullOrEmpty(url) ? null : new Uri(BaseUrl + url);
    }

    #endregion Methods
  }
}