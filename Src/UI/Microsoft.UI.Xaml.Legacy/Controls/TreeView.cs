// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.TreeView
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.UI.Xaml.Controls
{
  [Threading]
  [Static(typeof (ITreeViewStatics), 65536)]
  [MarshalingBehavior]
  [Composable]
  [Version(65536)]
  public class TreeView : Control, ITreeView
  {
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern TreeView();

    public extern TreeViewList ListControl { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public extern TreeViewNode RootNode { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern TreeViewSelectionMode SelectionMode { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern IVector<object> SelectedItems { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void ExpandNode([In] TreeViewNode value);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void CollapseNode([In] TreeViewNode value);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void SelectAll();

    public extern event TypedEventHandler<TreeView, TreeViewItemClickEventArgs> ItemClicked;

    public extern event TypedEventHandler<TreeView, TreeViewExpandingEventArgs> Expanding;

    public static extern DependencyProperty SelectionModeProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }
  }
}
