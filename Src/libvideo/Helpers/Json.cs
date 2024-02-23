// Decompiled with JetBrains decompiler
// Type: VideoLibrary.Helpers.Json
// Assembly: libvideo, Version=3.1.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 17817EC5-58AD-49FB-80DB-1FFC57084213
// Assembly location: C:\Users\Admin\Desktop\RE\EasyFetch\libvideo.dll

using System;
using System.Text;

#nullable disable
namespace VideoLibrary.Helpers
{
  internal static class Json
  {
    public static string GetKey(string key, string source)
    {
      string target;
      return Json.GetKey(key, source, out target) ? target : (string) null;
    }

    public static bool TryGetKey(string key, string source, out string target)
    {
      return Json.GetKey(key, source, out target);
    }

    public static string Extract(string source)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int num = 0;
      char ch1 = char.MinValue;
      foreach (char ch2 in source)
      {
        stringBuilder.Append(ch2);
        if (ch2 == '{' && ch1 != '\\')
          ++num;
        else if (ch2 == '}' && ch1 != '\\')
          --num;
        if (num != 0)
          ch1 = ch2;
        else
          break;
      }
      return stringBuilder.ToString();
    }

    private static bool GetKey(string key, string source, out string target)
    {
      string str1 = "\"" + key + "\"";
      int startIndex1 = 0;
      int startIndex2;
      string str2;
      int index1;
      do
      {
        int start1;
        string str3;
        int index2;
        do
        {
          int num1 = source.IndexOf(str1, startIndex1, StringComparison.Ordinal);
          if (num1 != -1)
          {
            startIndex1 = num1 + str1.Length;
            int start2 = startIndex1;
            int num2 = source.SkipWhitespace(start2);
            str3 = source;
            index2 = num2;
            start1 = index2 + 1;
          }
          else
            goto label_1;
        }
        while (str3[index2] != ':');
        int num = source.SkipWhitespace(start1);
        str2 = source;
        index1 = num;
        startIndex2 = index1 + 1;
      }
      while (str2[index1] != '"');
      goto label_4;
label_1:
      target = string.Empty;
      return false;
label_4:
      int index3 = startIndex2;
      while (source[index3 - 1] == '\\' && source[index3] == '"' || source[index3] != '"')
        ++index3;
      target = source.Substring(startIndex2, index3 - startIndex2);
      return true;
    }
  }
}
