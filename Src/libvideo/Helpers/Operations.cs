// Decompiled with JetBrains decompiler
// Type: VideoLibrary.Helpers.Operations
// Assembly: libvideo, Version=3.1.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 17817EC5-58AD-49FB-80DB-1FFC57084213
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\libvideo.dll

namespace VideoLibrary.Helpers
{
  internal struct Operations
  {
    public Operations(string reverse, string swap, string splice)
    {
      this.Reverse = reverse;
      this.Swap = swap;
      this.Splice = splice;
    }

    public string Reverse { get; }

    public string Swap { get; }

    public string Splice { get; }
  }
}
