// Decompiled with JetBrains decompiler
// Type: VideoLibrary.Helpers.Require
// Assembly: libvideo, Version=3.1.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 17817EC5-58AD-49FB-80DB-1FFC57084213
// Assembly location: C:\Users\Admin\Desktop\RE\EasyFetch\libvideo.dll

using System;

#nullable disable
namespace VideoLibrary.Helpers
{
  internal static class Require
  {
    public static void NotNull<T>(T obj, string name) where T : class
    {
      if ((object) obj == null)
        throw new ArgumentNullException(name);
    }
  }
}
