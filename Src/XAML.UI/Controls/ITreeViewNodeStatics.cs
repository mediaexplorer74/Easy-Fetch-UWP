// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.ITreeViewNodeStatics
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Microsoft.UI.Xaml.Controls
{
  [ExclusiveTo(typeof (TreeViewNode))]
  [Version(65536)]
  [Guid(1568831230, 57809, 19297, 158, 129, 199, 251, 24, 154, 41, 142)]
  internal interface ITreeViewNodeStatics
  {
    DependencyProperty DataProperty { get; }

    DependencyProperty DepthProperty { get; }

    DependencyProperty IsExpandedProperty { get; }

    DependencyProperty HasItemsProperty { get; }
  }
}
