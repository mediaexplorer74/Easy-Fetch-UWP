// Decompiled with JetBrains decompiler
// Type: VideoLibrary.Client`1
// Assembly: libvideo, Version=3.1.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 17817EC5-58AD-49FB-80DB-1FFC57084213
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\libvideo.dll

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VideoLibrary.Helpers;

namespace VideoLibrary
{
  public class Client<T> : IService<T>, IAsyncService<T>, IDisposable where T : Video
  {
    private bool disposed = false;
    private readonly ServiceBase<T> baseService;
    private readonly HttpClient client;

    private Task<string> SourceFactory(string address) => this.client.GetStringAsync(address);

    internal Client(ServiceBase<T> baseService)
    {
      Require.NotNull<ServiceBase<T>>(baseService, nameof (baseService));
      this.baseService = baseService;
      this.client = baseService.MakeClient();
    }

    ~Client() => this.Dispose(false);

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      this.disposed = true;
      if (!disposing || this.client == null)
        return;
      this.client.Dispose();
    }

    public T GetVideo(string videoUri) => this.baseService.GetVideo(videoUri, new Func<string, Task<string>>(this.SourceFactory));

    public IEnumerable<T> GetAllVideos(string videoUri) => this.baseService.GetAllVideos(videoUri, new Func<string, Task<string>>(this.SourceFactory));

    public Task<T> GetVideoAsync(string videoUri) => this.baseService.GetVideoAsync(videoUri, new Func<string, Task<string>>(this.SourceFactory));

    public Task<IEnumerable<T>> GetAllVideosAsync(string videoUri) => this.baseService.GetAllVideosAsync(videoUri, new Func<string, Task<string>>(this.SourceFactory));
  }
}
