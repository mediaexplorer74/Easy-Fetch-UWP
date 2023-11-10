// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.ITreeViewNode
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Microsoft.UI.Xaml.Controls
{
  [Guid(3226242771, 39666, 20085, 163, 41, 116, 151, 161, 16, 231, 166)]
  [Version(65536)]
  [ExclusiveTo(typeof (TreeViewNode))]
  internal interface ITreeViewNode
  {
    object Data { get; [param: In] set; }

    TreeViewNode ParentNode { get; [param: In] set; }

    bool IsExpanded { get; [param: In] set; }

    bool HasItems { get; }

    int Depth { get; }

    bool HasUnrealizedItems { get; [param: In] set; }

    event TypedEventHandler<TreeViewNode, DependencyPropertyChangedEventArgs> IsExpandedChanged;
  }
}
