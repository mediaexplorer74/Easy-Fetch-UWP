// Decompiled with JetBrains decompiler
// Type: VideoLibrary.VideoClient
// Assembly: libvideo, Version=3.1.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 17817EC5-58AD-49FB-80DB-1FFC57084213
// Assembly location: C:\Users\Admin\Desktop\RE\EasyFetch\libvideo.dll

using System;
using System.Net.Http;
using System.Threading.Tasks;

#nullable disable
namespace VideoLibrary
{
  public class VideoClient : IDisposable
  {
    private bool disposed = false;
    private readonly HttpClient client;

    public VideoClient() => this.client = this.MakeClient();

    ~VideoClient() => this.Dispose(false);

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

    private HttpClient MakeClient() => this.MakeClient(this.MakeHandler());

    protected virtual HttpMessageHandler MakeHandler()
    {
      return (HttpMessageHandler) new HttpClientHandler();
    }

    protected virtual HttpClient MakeClient(HttpMessageHandler handler)
    {
      return new HttpClient(handler)
      {
        Timeout = TimeSpan.FromMilliseconds((double) int.MaxValue)
      };
    }

    public byte[] GetBytes(Video video) => this.GetBytesAsync(video).GetAwaiter().GetResult();

    public async Task<byte[]> GetBytesAsync(Video video)
    {
      string uri = await video.GetUriAsync().ConfigureAwait(false);
      byte[] bytesAsync = await this.client.GetByteArrayAsync(uri).ConfigureAwait(false);
      uri = (string) null;
      return bytesAsync;
    }

    public System.IO.Stream Stream(Video video) => this.StreamAsync(video).GetAwaiter().GetResult();

    public async Task<System.IO.Stream> StreamAsync(Video video)
    {
      string uri = await video.GetUriAsync().ConfigureAwait(false);
      System.IO.Stream stream = await this.client.GetStreamAsync(uri).ConfigureAwait(false);
      uri = (string) null;
      return stream;
    }

    public async Task<long?> GetContentLengthAsync(string requestUri)
    {
      long? contentLength;
      using (HttpResponseMessage response = await this.HeadAsync(requestUri))
        contentLength = response.Content.Headers.ContentLength;
      return contentLength;
    }

    public async Task<HttpResponseMessage> HeadAsync(string requestUri)
    {
      HttpResponseMessage httpResponseMessage;
      using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Head, requestUri))
        httpResponseMessage = await this.client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
      return httpResponseMessage;
    }
  }
}
