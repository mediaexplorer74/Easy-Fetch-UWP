// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.IRatingControlStatics
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Microsoft.UI.Xaml.Controls
{
  [Guid(3496137625, 44421, 19484, 178, 196, 53, 221, 68, 50, 39, 93)]
  [ExclusiveTo(typeof (RatingControl))]
  [Version(65536)]
  internal interface IRatingControlStatics
  {
    DependencyProperty CaptionProperty { get; }

    DependencyProperty InitialSetValueProperty { get; }

    DependencyProperty IsClearEnabledProperty { get; }

    DependencyProperty IsReadOnlyProperty { get; }

    DependencyProperty MaxRatingProperty { get; }

    DependencyProperty PlaceholderValueProperty { get; }

    DependencyProperty ItemInfoProperty { get; }

    DependencyProperty ValueProperty { get; }
  }
}
