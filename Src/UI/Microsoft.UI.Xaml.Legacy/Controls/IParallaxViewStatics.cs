// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.IParallaxViewStatics
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Microsoft.UI.Xaml.Controls
{
  [Version(65536)]
  [ExclusiveTo(typeof (ParallaxView))]
  [Guid(2285298844, 3598, 22488, 147, 149, 240, 169, 160, 209, 187, 39)]
  internal interface IParallaxViewStatics
  {
    DependencyProperty ChildProperty { get; }

    DependencyProperty HorizontalSourceEndOffsetProperty { get; }

    DependencyProperty HorizontalSourceOffsetKindProperty { get; }

    DependencyProperty HorizontalSourceStartOffsetProperty { get; }

    DependencyProperty MaxHorizontalShiftRatioProperty { get; }

    DependencyProperty HorizontalShiftProperty { get; }

    DependencyProperty IsHorizontalShiftClampedProperty { get; }

    DependencyProperty IsVerticalShiftClampedProperty { get; }

    DependencyProperty SourceProperty { get; }

    DependencyProperty VerticalSourceEndOffsetProperty { get; }

    DependencyProperty VerticalSourceOffsetKindProperty { get; }

    DependencyProperty VerticalSourceStartOffsetProperty { get; }

    DependencyProperty MaxVerticalShiftRatioProperty { get; }

    DependencyProperty VerticalShiftProperty { get; }
  }
}
