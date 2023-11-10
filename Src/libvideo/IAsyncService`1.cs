// Decompiled with JetBrains decompiler
// Type: VideoLibrary.IAsyncService`1
// Assembly: libvideo, Version=3.1.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 17817EC5-58AD-49FB-80DB-1FFC57084213
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\libvideo.dll

using System.Collections.Generic;
using System.Threading.Tasks;

namespace VideoLibrary
{
  internal interface IAsyncService<T> where T : Video
  {
    Task<T> GetVideoAsync(string uri);

    Task<IEnumerable<T>> GetAllVideosAsync(string uri);
  }
}
