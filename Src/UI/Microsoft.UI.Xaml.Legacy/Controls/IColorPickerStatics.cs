// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.IColorPickerStatics
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Microsoft.UI.Xaml.Controls
{
  [Guid(1741331431, 5492, 17690, 182, 223, 254, 87, 217, 208, 123, 69)]
  [ExclusiveTo(typeof (ColorPicker))]
  [Version(65536)]
  internal interface IColorPickerStatics
  {
    DependencyProperty ColorProperty { get; }

    DependencyProperty PreviousColorProperty { get; }

    DependencyProperty IsAlphaEnabledProperty { get; }

    DependencyProperty IsColorSpectrumVisibleProperty { get; }

    DependencyProperty IsColorPreviewVisibleProperty { get; }

    DependencyProperty IsColorSliderVisibleProperty { get; }

    DependencyProperty IsAlphaSliderVisibleProperty { get; }

    DependencyProperty IsMoreButtonVisibleProperty { get; }

    DependencyProperty IsColorChannelTextInputVisibleProperty { get; }

    DependencyProperty IsAlphaTextInputVisibleProperty { get; }

    DependencyProperty IsHexInputVisibleProperty { get; }

    DependencyProperty MinHueProperty { get; }

    DependencyProperty MaxHueProperty { get; }

    DependencyProperty MinSaturationProperty { get; }

    DependencyProperty MaxSaturationProperty { get; }

    DependencyProperty MinValueProperty { get; }

    DependencyProperty MaxValueProperty { get; }

    DependencyProperty ColorSpectrumShapeProperty { get; }

    DependencyProperty ColorSpectrumComponentsProperty { get; }
  }
}
