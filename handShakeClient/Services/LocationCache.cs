using HandshakeClient.Enums;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HandshakeClient.Services
{
  public class LocationCache
  {
    private LocationAndTime cachedLocation;

    internal async Task<Location> GetCurrentLocation(TimePassed accuracy)
    {
      if (!this.CheckIfNewLocationRequired(accuracy))
      {
        return this.cachedLocation.Location;
      }

      GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium);
      Location location = await Geolocation.GetLocationAsync(request);

      this.cachedLocation = new LocationAndTime()
      {
        Location = location,
        DateTime = System.DateTime.Now
      };

      return location;
    }

    public bool CheckIfNewLocationRequired(TimePassed accuracy)
    {
      if (this.cachedLocation == null)
      {
        return true;
      }

      DateTime now = System.DateTime.Now;

      switch (accuracy)
      {
        case TimePassed.JustNow:
          return this.cachedLocation.DateTime + new TimeSpan(0, 1, 0) < now;
        case TimePassed.AMomentAgo:
          return this.cachedLocation.DateTime + new TimeSpan(0, 10, 0) < now;
        default:
          throw new NotImplementedException();
      }
    }

    private class LocationAndTime
    {
      public Location Location { get; set; }
      public DateTime DateTime { get; set; }
    }
  }
}
