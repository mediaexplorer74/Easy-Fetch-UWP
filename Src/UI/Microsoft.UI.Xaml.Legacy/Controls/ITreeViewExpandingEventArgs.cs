// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.ITreeViewExpandingEventArgs
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [Version(65536)]
  [Guid(3319921251, 16724, 18898, 162, 31, 195, 65, 118, 96, 94, 56)]
  [ExclusiveTo(typeof (TreeViewExpandingEventArgs))]
  internal interface ITreeViewExpandingEventArgs
  {
    TreeViewNode Node { get; }
  }
}
