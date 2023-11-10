// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.Primitives.ColorPickerSlider
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.UI.Xaml.Controls.Primitives
{
  [Version(65536)]
  [MarshalingBehavior]
  [Static(typeof (IColorPickerSliderStatics), 65536)]
  [Threading]
  [Composable]
  public class ColorPickerSlider : Slider, IColorPickerSlider
  {
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern ColorPickerSlider();

    public extern ColorPickerHsvChannel ColorChannel { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public static extern DependencyProperty ColorChannelProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }
  }
}
