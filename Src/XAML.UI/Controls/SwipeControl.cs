// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.SwipeControl
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
  [Composable]
  [Threading]
  [MarshalingBehavior]
  [Version(65536)]
  [Static(typeof (ISwipeControlStatics), 65536)]
  public class SwipeControl : ContentControl, ISwipeControl
  {
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern SwipeControl();

    public extern SwipeItems LeftItems { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern SwipeItems RightItems { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern SwipeItems TopItems { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern SwipeItems BottomItems { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void Close();

    public static extern DependencyProperty LeftItemsProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty RightItemsProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty TopItemsProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty BottomItemsProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }
  }
}
