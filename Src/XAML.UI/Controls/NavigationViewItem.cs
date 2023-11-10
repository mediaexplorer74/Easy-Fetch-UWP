// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.NavigationViewItem
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.UI.Xaml.Controls
{
  [Version(65536)]
  [Threading]
  [Static(typeof (INavigationViewItemStatics), 65536)]
  [Composable]
  [MarshalingBehavior]
  public class NavigationViewItem : NavigationViewItemBase, INavigationViewItem
  {
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern NavigationViewItem();

    public extern IconElement Icon { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern double CompactPaneLength { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty IconProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty CompactPaneLengthProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }
  }
}
