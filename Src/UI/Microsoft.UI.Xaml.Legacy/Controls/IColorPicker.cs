// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.IColorPicker
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI;

namespace Microsoft.UI.Xaml.Controls
{
  [Guid(1647502193, 23652, 17355, 139, 53, 127, 130, 221, 227, 103, 79)]
  [Version(65536)]
  [ExclusiveTo(typeof (ColorPicker))]
  internal interface IColorPicker
  {
    Color Color { get; [param: In] set; }

    IReference<Color> PreviousColor { get; [param: In] set; }

    bool IsAlphaEnabled { get; [param: In] set; }

    bool IsColorSpectrumVisible { get; [param: In] set; }

    bool IsColorPreviewVisible { get; [param: In] set; }

    bool IsColorSliderVisible { get; [param: In] set; }

    bool IsAlphaSliderVisible { get; [param: In] set; }

    bool IsMoreButtonVisible { get; [param: In] set; }

    bool IsColorChannelTextInputVisible { get; [param: In] set; }

    bool IsAlphaTextInputVisible { get; [param: In] set; }

    bool IsHexInputVisible { get; [param: In] set; }

    int MinHue { get; [param: In] set; }

    int MaxHue { get; [param: In] set; }

    int MinSaturation { get; [param: In] set; }

    int MaxSaturation { get; [param: In] set; }

    int MinValue { get; [param: In] set; }

    int MaxValue { get; [param: In] set; }

    ColorSpectrumShape ColorSpectrumShape { get; [param: In] set; }

    ColorSpectrumComponents ColorSpectrumComponents { get; [param: In] set; }

    event TypedEventHandler<ColorPicker, ColorChangedEventArgs> ColorChanged;
  }
}
