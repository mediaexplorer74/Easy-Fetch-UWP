// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.SwipeItem
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Microsoft.UI.Xaml.Controls
{
  [Version(65536)]
  [Static(typeof (ISwipeItemStatics), 65536)]
  [Composable]
  [Threading]
  [MarshalingBehavior]
  public class SwipeItem : DependencyObject, ISwipeItem
  {
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern SwipeItem();

    public extern string Text { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern IconSource IconSource { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern Brush Background { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern Brush Foreground { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern ICommand Command { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern object CommandParameter { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern SwipeBehaviorOnInvoked BehaviorOnInvoked { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern event TypedEventHandler<SwipeItem, SwipeItemInvokedEventArgs> Invoked;

    public static extern DependencyProperty IconSourceProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty TextProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty BackgroundProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty ForegroundProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty CommandProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty CommandParameterProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty BehaviorOnInvokedProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }
  }
}
