// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Media.IAcrylicBrushStatics
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Microsoft.UI.Xaml.Media
{
  [Version(65536)]
  [ExclusiveTo(typeof (AcrylicBrush))]
  [Guid(663223673, 41946, 16959, 184, 26, 89, 145, 71, 151, 21, 34)]
  internal interface IAcrylicBrushStatics
  {
    DependencyProperty BackgroundSourceProperty { get; }

    DependencyProperty TintColorProperty { get; }

    DependencyProperty TintOpacityProperty { get; }

    DependencyProperty TintTransitionDurationProperty { get; }

    DependencyProperty AlwaysUseFallbackProperty { get; }
  }
}
