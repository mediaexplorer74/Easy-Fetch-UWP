// Decompiled with JetBrains decompiler
// Type: VideoLibrary.Helpers.Html
// Assembly: libvideo, Version=3.1.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 17817EC5-58AD-49FB-80DB-1FFC57084213
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\libvideo.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace VideoLibrary.Helpers
{
  internal static class Html
  {
    public static string GetNode(string name, string source) => WebUtility.HtmlDecode(Text.StringBetween("<" + name + ">", "</" + name + ">", source));

    public static IEnumerable<string> GetUrisFromManifest(string source)
    {
      string str = "<BaseURL>";
      string closing = "</BaseURL>";
      int startIndex = source.IndexOf(str);
      if (startIndex == -1)
        throw new NotSupportedException();
      return ((IEnumerable<string>) source.Substring(startIndex).Split(new string[1]
      {
        str
      }, StringSplitOptions.RemoveEmptyEntries)).Select<string, string>((Func<string, string>) (v => v.Substring(0, v.IndexOf(closing))));
    }
  }
}
