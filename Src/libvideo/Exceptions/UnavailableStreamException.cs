// Decompiled with JetBrains decompiler
// Type: VideoLibrary.Exceptions.UnavailableStreamException
// Assembly: libvideo, Version=3.1.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 17817EC5-58AD-49FB-80DB-1FFC57084213
// Assembly location: C:\Users\Admin\Desktop\RE\EasyFetch\libvideo.dll

using System;

#nullable disable
namespace VideoLibrary.Exceptions
{
  public class UnavailableStreamException : Exception
  {
    public UnavailableStreamException()
    {
    }

    public UnavailableStreamException(string message)
      : base(message)
    {
    }

    public UnavailableStreamException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
