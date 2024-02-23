// Decompiled with JetBrains decompiler
// Type: VideoLibrary.IService`1
// Assembly: libvideo, Version=3.1.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 17817EC5-58AD-49FB-80DB-1FFC57084213
// Assembly location: C:\Users\Admin\Desktop\RE\EasyFetch\libvideo.dll

using System.Collections.Generic;

#nullable disable
namespace VideoLibrary
{
  internal interface IService<T> where T : Video
  {
    T GetVideo(string uri);

    IEnumerable<T> GetAllVideos(string uri);
  }
}
