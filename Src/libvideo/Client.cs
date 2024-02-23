// Decompiled with JetBrains decompiler
// Type: VideoLibrary.Client
// Assembly: libvideo, Version=3.1.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 17817EC5-58AD-49FB-80DB-1FFC57084213
// Assembly location: C:\Users\Admin\Desktop\RE\EasyFetch\libvideo.dll

#nullable disable
namespace VideoLibrary
{
  public static class Client
  {
    public static Client<T> For<T>(ServiceBase<T> baseService) where T : Video
    {
      return new Client<T>(baseService);
    }
  }
}
