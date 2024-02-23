// Decompiled with JetBrains decompiler
// Type: VideoLibrary.Helpers.UnscrambledQuery
// Assembly: libvideo, Version=3.1.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 17817EC5-58AD-49FB-80DB-1FFC57084213
// Assembly location: C:\Users\Admin\Desktop\RE\EasyFetch\libvideo.dll

#nullable disable
namespace VideoLibrary.Helpers
{
  internal readonly struct UnscrambledQuery
  {
    public UnscrambledQuery(string uri, bool encrypted)
    {
      this.Uri = uri;
      this.IsEncrypted = encrypted;
    }

    public string Uri { get; }

    public bool IsEncrypted { get; }
  }
}
