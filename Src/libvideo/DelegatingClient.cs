// Decompiled with JetBrains decompiler
// Type: VideoLibrary.DelegatingClient
// Assembly: libvideo, Version=3.1.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 17817EC5-58AD-49FB-80DB-1FFC57084213
// Assembly location: C:\Users\Admin\Desktop\RE\EasyFetch\libvideo.dll

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

#nullable disable
namespace VideoLibrary
{
  public class DelegatingClient : IDisposable
  {
    private bool disposed = false;
    private readonly HttpClient client;

    public DelegatingClient() => this.client = this.MakeClient();

    ~DelegatingClient() => this.Dispose(false);

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
      HttpClientHandler httpClientHandler = new HttpClientHandler();
      if (httpClientHandler.SupportsAutomaticDecompression)
        httpClientHandler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
      return (HttpMessageHandler) httpClientHandler;
    }

    protected virtual HttpClient MakeClient(HttpMessageHandler handler) => new HttpClient(handler);

    public HttpResponseMessage Get(string uri) => this.GetAsync(uri).GetAwaiter().GetResult();

    public byte[] GetByteArray(string uri) => this.GetByteArrayAsync(uri).GetAwaiter().GetResult();

    public Stream GetStream(string uri) => this.GetStreamAsync(uri).GetAwaiter().GetResult();

    public string GetString(string uri) => this.GetStringAsync(uri).GetAwaiter().GetResult();

    public Task<HttpResponseMessage> GetAsync(string uri) => this.client.GetAsync(uri);

    public Task<byte[]> GetByteArrayAsync(string uri) => this.client.GetByteArrayAsync(uri);

    public Task<Stream> GetStreamAsync(string uri) => this.client.GetStreamAsync(uri);

    public Task<string> GetStringAsync(string uri) => this.client.GetStringAsync(uri);
  }
}
