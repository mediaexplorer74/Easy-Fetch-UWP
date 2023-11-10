// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.IFontIconSourceStatics
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Microsoft.UI.Xaml.Controls
{
  [Guid(2349744687, 33385, 17329, 185, 90, 239, 7, 14, 134, 119, 12)]
  [Version(65536)]
  [ExclusiveTo(typeof (FontIconSource))]
  internal interface IFontIconSourceStatics
  {
    DependencyProperty GlyphProperty { get; }

    DependencyProperty FontSizeProperty { get; }

    DependencyProperty FontFamilyProperty { get; }

    DependencyProperty FontWeightProperty { get; }

    DependencyProperty FontStyleProperty { get; }

    DependencyProperty IsTextScaleFactorEnabledProperty { get; }

    DependencyProperty MirroredWhenRightToLeftProperty { get; }
  }
}
