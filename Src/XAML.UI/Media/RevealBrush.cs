// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Media.RevealBrush
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.UI.Xaml.Media
{
  [Threading]
  [Version(65536)]
  [Static(typeof (IRevealBrushStatics), 65536)]
  [Composable]
  [MarshalingBehavior]
  public class RevealBrush : XamlCompositionBrushBase, IRevealBrush
  {
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    protected extern RevealBrush();

    public extern Color Color { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern ApplicationTheme TargetTheme { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern bool AlwaysUseFallback { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public static extern DependencyProperty ColorProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty TargetThemeProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty AlwaysUseFallbackProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty StateProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetState([In] UIElement element, [In] RevealBrushState value);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern RevealBrushState GetState([In] UIElement element);
  }
}
