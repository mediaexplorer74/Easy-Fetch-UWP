// Decompiled with JetBrains decompiler
// Type: VideoLibrary.Exceptions.BadQueryException
// Assembly: libvideo, Version=3.1.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 17817EC5-58AD-49FB-80DB-1FFC57084213
// Assembly location: C:\Users\Admin\Desktop\RE\EasyFetch\libvideo.dll

using System;

#nullable disable
namespace VideoLibrary.Exceptions
{
  internal class BadQueryException : Exception
  {
    public BadQueryException()
    {
    }

    public BadQueryException(string message)
      : base(message)
    {
    }

    public BadQueryException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
