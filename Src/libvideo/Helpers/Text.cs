// Decompiled with JetBrains decompiler
// Type: VideoLibrary.Helpers.Text
// Assembly: libvideo, Version=3.1.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 17817EC5-58AD-49FB-80DB-1FFC57084213
// Assembly location: C:\Users\Admin\Desktop\RE\EasyFetch\libvideo.dll

#nullable disable
namespace VideoLibrary.Helpers
{
  internal static class Text
  {
    public static string StringBetween(string prefix, string suffix, string parent)
    {
      int startIndex = parent.IndexOf(prefix) + prefix.Length;
      if (startIndex < prefix.Length)
        return string.Empty;
      int num = parent.IndexOf(suffix, startIndex);
      if (num == -1)
        num = parent.Length;
      return parent.Substring(startIndex, num - startIndex);
    }

    public static int SkipWhitespace(this string text, int start)
    {
      int index = start;
      while (char.IsWhiteSpace(text[index]))
        ++index;
      return index;
    }
  }
}
