// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.BitmapIconSource
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Microsoft.UI.Xaml.Controls
{
  [Composable]
  [Version(65536)]
  [MarshalingBehavior]
  [Static(typeof (IBitmapIconSourceStatics), 65536)]
  [Threading]
  public class BitmapIconSource : IconSource, IBitmapIconSource
  {
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern BitmapIconSource();

    public extern Uri UriSource { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern bool ShowAsMonochrome { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public static extern DependencyProperty UriSourceProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty ShowAsMonochromeProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }
  }
}
