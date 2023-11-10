// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.ITreeView
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [Version(65536)]
  [Guid(2471742556, 56686, 17724, 174, 221, 12, 58, 201, 147, 151, 137)]
  [ExclusiveTo(typeof (TreeView))]
  internal interface ITreeView
  {
    TreeViewList ListControl { get; }

    TreeViewNode RootNode { get; [param: In] set; }

    TreeViewSelectionMode SelectionMode { get; [param: In] set; }

    IVector<object> SelectedItems { get; }

    void ExpandNode([In] TreeViewNode value);

    void CollapseNode([In] TreeViewNode value);

    void SelectAll();

    event TypedEventHandler<TreeView, TreeViewItemClickEventArgs> ItemClicked;

    event TypedEventHandler<TreeView, TreeViewExpandingEventArgs> Expanding;
  }
}
