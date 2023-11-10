// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.INavigationViewItem
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml.Controls;

namespace Microsoft.UI.Xaml.Controls
{
  [ExclusiveTo(typeof (NavigationViewItem))]
  [Guid(2249506319, 47030, 18513, 150, 10, 245, 227, 246, 159, 98, 73)]
  [Version(65536)]
  internal interface INavigationViewItem
  {
    IconElement Icon { get; [param: In] set; }

    double CompactPaneLength { get; }
  }
}
