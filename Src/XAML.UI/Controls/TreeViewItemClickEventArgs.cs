// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.TreeViewItemClickEventArgs
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [Version(65536)]
  [Threading]
  [MarshalingBehavior]
  public sealed class TreeViewItemClickEventArgs : ITreeViewItemClickEventArgs
  {
    public extern object ClickedItem { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public extern bool IsHandled { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }
  }
}
