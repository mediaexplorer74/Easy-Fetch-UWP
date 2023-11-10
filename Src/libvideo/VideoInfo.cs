// Decompiled with JetBrains decompiler
// Type: VideoLibrary.VideoInfo
// Assembly: libvideo, Version=3.1.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 17817EC5-58AD-49FB-80DB-1FFC57084213
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\libvideo.dll

namespace VideoLibrary
{
  public class VideoInfo
  {
    public VideoInfo(string title, int? second, string author)
    {
      this.Title = title;
      this.LengthSeconds = second;
      this.Author = author;
    }

    public string Title { get; }

    public int? LengthSeconds { get; }

    public string Author { get; }
  }
}
