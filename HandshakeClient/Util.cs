using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HandshakeClient
{
  class Util
  {
    internal static bool IsGif(string path)
    {
      return string.Compare(Path.GetExtension(path), ".gif", true) == 0;
    }
  }
}
