// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.Primitives.IColorSpectrum
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Foundation.Numerics;
using Windows.UI;

namespace Microsoft.UI.Xaml.Controls.Primitives
{
  [ExclusiveTo(typeof (ColorSpectrum))]
  [Version(65536)]
  [Guid(3460756081, 62729, 20376, 130, 136, 228, 148, 47, 179, 133, 222)]
  internal interface IColorSpectrum
  {
    Color Color { get; [param: In] set; }

    Vector4 HsvColor { get; [param: In] set; }

    int MinHue { get; [param: In] set; }

    int MaxHue { get; [param: In] set; }

    int MinSaturation { get; [param: In] set; }

    int MaxSaturation { get; [param: In] set; }

    int MinValue { get; [param: In] set; }

    int MaxValue { get; [param: In] set; }

    ColorSpectrumShape Shape { get; [param: In] set; }

    ColorSpectrumComponents Components { get; [param: In] set; }

    event TypedEventHandler<ColorSpectrum, ColorChangedEventArgs> ColorChanged;
  }
}
