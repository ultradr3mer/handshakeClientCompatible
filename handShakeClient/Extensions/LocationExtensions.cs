using System;
using System.Numerics;
using Xamarin.Essentials;

namespace HandshakeClient.Extensions
{
  /// <summary>
  /// The <see cref="LocationExtensions"/> provides extension methods for the <see cref="Location"/> class.
  /// </summary>
  public static class LocationExtensions
  {
    #region Methods

    public static Vector3 CalcPos(this Location source, double radius)
    {
      var lat = source.Latitude;
      var lon = source.Longitude;

      var phi = (90 - lat) * (Math.PI / 180);
      var theta = (lon + 180) * (Math.PI / 180);

      var x = -((radius) * Math.Sin(phi) * Math.Cos(theta));
      var z = ((radius) * Math.Sin(phi) * Math.Sin(theta));
      var y = ((radius) * Math.Cos(phi));

      return new Vector3((float)x, (float)y, (float)z);
    }

    #endregion Methods
  }
}