// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.Primitives.IColorSpectrumStatics
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Microsoft.UI.Xaml.Controls.Primitives
{
  [ExclusiveTo(typeof (ColorSpectrum))]
  [Guid(2422992508, 11502, 20112, 150, 139, 240, 165, 189, 105, 27, 73)]
  [Version(65536)]
  internal interface IColorSpectrumStatics
  {
    DependencyProperty ColorProperty { get; }

    DependencyProperty HsvColorProperty { get; }

    DependencyProperty MinHueProperty { get; }

    DependencyProperty MaxHueProperty { get; }

    DependencyProperty MinSaturationProperty { get; }

    DependencyProperty MaxSaturationProperty { get; }

    DependencyProperty MinValueProperty { get; }

    DependencyProperty MaxValueProperty { get; }

    DependencyProperty ShapeProperty { get; }

    DependencyProperty ComponentsProperty { get; }
  }
}
