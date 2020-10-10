using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace HandshakeClient.Services
{
  public class CustomHttpClient : HttpClient
  {
    #region Constructors

    public CustomHttpClient(string username, string password) : base(new HttpClientHandler
    {
      Credentials = new NetworkCredential(username, password)
    })
    {
      //string datenbank = SecureStorage.GetAsync(CustomHttpClient.Datenbank).Result;
      //string mandant = SecureStorage.GetAsync(CustomHttpClient.Mandant).Result;
      //this.DefaultRequestHeaders.Add(CustomHttpClient.Datenbank, datenbank);
      //this.DefaultRequestHeaders.Add(CustomHttpClient.Mandant, mandant);

      byte[] authBytes = Encoding.UTF8.GetBytes(username + ":" + password);
      this.DefaultRequestHeaders.Add("Authorization", "BASIC " + Convert.ToBase64String(authBytes));
    }

    #endregion Constructors
  }

}
