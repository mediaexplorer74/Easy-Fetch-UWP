// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.RatingControl
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.UI.Xaml.Controls
{
  [Composable]
  [Threading]
  [Version(65536)]
  [MarshalingBehavior]
  [Static(typeof (IRatingControlStatics), 65536)]
  public class RatingControl : Control, IRatingControl
  {
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern RatingControl();

    public extern string Caption { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern int InitialSetValue { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern bool IsClearEnabled { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern bool IsReadOnly { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern int MaxRating { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern double PlaceholderValue { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern RatingItemInfo ItemInfo { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern double Value { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern event TypedEventHandler<RatingControl, object> ValueChanged;

    public static extern DependencyProperty CaptionProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty InitialSetValueProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty IsClearEnabledProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty IsReadOnlyProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty MaxRatingProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty PlaceholderValueProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty ItemInfoProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty ValueProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }
  }
}
