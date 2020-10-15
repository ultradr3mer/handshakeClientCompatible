﻿using HandshakeClient.Services;
using System;

namespace HandshakeClient.Extensions
{
  public static class TimespanExtension
  {
    #region Methods

    public static string ToStringForHumans(this TimeSpan value)
    {
      if (value < new TimeSpan(0, 0, 1, 0))
      {
        return "less than a minute";
      }

      if (value < new TimeSpan(0, 1, 0, 0))
      {
        return value.Minutes + (value.Minutes > 1 ? " minutes" : " minute");
      }

      if (value < new TimeSpan(1, 0, 0, 0))
      {
        return value.Hours + (value.Hours > 1 ? " hours" : " hour");
      }

      return (int)value.TotalDays + (value.TotalDays > 1 ? " days" : " day");
    }

    public static string ToStringForHumans(this SimpleTimeSpan value)
    {
      return new TimeSpan(value.TotalDays, value.Hours, value.Minutes, value.Seconds).ToStringForHumans();
    }

    #endregion Methods
  }
}