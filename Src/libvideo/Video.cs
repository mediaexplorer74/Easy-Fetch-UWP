// Decompiled with JetBrains decompiler
// Type: VideoLibrary.Video
// Assembly: libvideo, Version=3.1.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 17817EC5-58AD-49FB-80DB-1FFC57084213
// Assembly location: C:\Users\Admin\Desktop\RE\EasyFetch\libvideo.dll

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

#nullable disable
namespace VideoLibrary
{
  public abstract class Video
  {
    internal Video()
    {
    }

    public abstract string Uri { get; }

    public abstract string Title { get; }

    public abstract VideoInfo Info { get; }

    public abstract WebSites WebSite { get; }

    public virtual VideoFormat Format => VideoFormat.Unknown;

    public virtual Task<string> GetUriAsync() => Task.FromResult<string>(this.Uri);

    public byte[] GetBytes() => this.GetBytesAsync().GetAwaiter().GetResult();

    public async Task<byte[]> GetBytesAsync()
    {
      byte[] bytesAsync;
      using (VideoClient client = new VideoClient())
        bytesAsync = await client.GetBytesAsync(this).ConfigureAwait(false);
      return bytesAsync;
    }

    public System.IO.Stream Stream() => this.StreamAsync().GetAwaiter().GetResult();

    public async Task<System.IO.Stream> StreamAsync()
    {
      System.IO.Stream stream;
      using (VideoClient client = new VideoClient())
        stream = await client.StreamAsync(this).ConfigureAwait(false);
      return stream;
    }

    public System.IO.Stream Head() => this.HeadAsync().GetAwaiter().GetResult();

    public async Task<System.IO.Stream> HeadAsync()
    {
      System.IO.Stream stream;
      using (VideoClient client = new VideoClient())
        stream = await client.StreamAsync(this).ConfigureAwait(false);
      return stream;
    }

    public virtual string FileExtension
    {
      get
      {
        switch (this.Format)
        {
          case VideoFormat.Mp4:
            return ".mp4";
          case VideoFormat.WebM:
            return ".webm";
          case VideoFormat.Unknown:
            return string.Empty;
          default:
            throw new NotImplementedException(string.Format("Format {0} is unrecognized! Please file an issue at libvideo on GitHub.", (object) this.Format));
        }
      }
    }

    public string FullName
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder(this.Title).Append(this.FileExtension);
        foreach (char invalidFileNameChar in Path.GetInvalidFileNameChars())
          stringBuilder.Replace(invalidFileNameChar, '_');
        return stringBuilder.ToString();
      }
    }
  }
}
