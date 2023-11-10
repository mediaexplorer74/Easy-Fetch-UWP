// Decompiled with JetBrains decompiler
// Type: VideoLibrary.ServiceBase`1
// Assembly: libvideo, Version=3.1.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 17817EC5-58AD-49FB-80DB-1FFC57084213
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\libvideo.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace VideoLibrary
{
  public abstract class ServiceBase<T> : IService<T>, IAsyncService<T> where T : Video
  {
    internal virtual T VideoSelector(IEnumerable<T> videos) => videos.First<T>();

    public T GetVideo(string videoUri) => this.GetVideoAsync(videoUri).GetAwaiter().GetResult();

    internal T GetVideo(string videoUri, Func<string, Task<string>> sourceFactory) => this.GetVideoAsync(videoUri, sourceFactory).GetAwaiter().GetResult();

    public IEnumerable<T> GetAllVideos(string videoUri) => this.GetAllVideosAsync(videoUri).GetAwaiter().GetResult();

    internal IEnumerable<T> GetAllVideos(string videoUri, Func<string, Task<string>> sourceFactory) => this.GetAllVideosAsync(videoUri, sourceFactory).GetAwaiter().GetResult();

    public async Task<T> GetVideoAsync(string videoUri)
    {
      T videoAsync;
      using (Client<T> wrapped = Client.For<T>(this))
        videoAsync = await wrapped.GetVideoAsync(videoUri).ConfigureAwait(false);
      return videoAsync;
    }

    internal async Task<T> GetVideoAsync(string videoUri, Func<string, Task<string>> sourceFactory)
    {
      IEnumerable<T> videos = await this.GetAllVideosAsync(videoUri, sourceFactory).ConfigureAwait(false);
      return this.VideoSelector(videos);
    }

    public async Task<IEnumerable<T>> GetAllVideosAsync(string videoUri)
    {
      IEnumerable<T> allVideosAsync;
      using (Client<T> wrapped = Client.For<T>(this))
        allVideosAsync = await wrapped.GetAllVideosAsync(videoUri).ConfigureAwait(false);
      return allVideosAsync;
    }

    internal abstract Task<IEnumerable<T>> GetAllVideosAsync(
      string videoUri,
      Func<string, Task<string>> sourceFactory);

    internal HttpClient MakeClient() => this.MakeClient(this.MakeHandler());

    protected virtual HttpMessageHandler MakeHandler()
    {
      CookieContainer cookieContainer = new CookieContainer();
      cookieContainer.Add(new Uri("https://youtube.com/"), new Cookie("CONSENT", "YES+cb", "/", ".youtube.com"));
      HttpClientHandler httpClientHandler = new HttpClientHandler()
      {
        UseCookies = true,
        CookieContainer = cookieContainer
      };
      if (httpClientHandler.SupportsAutomaticDecompression)
        httpClientHandler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
      return (HttpMessageHandler) httpClientHandler;
    }

    protected virtual HttpClient MakeClient(HttpMessageHandler handler)
    {
      new HttpClient(handler).DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.114 Safari/537.36 Edg/89.0.774.76");
      return new HttpClient(handler);
    }
  }
}
