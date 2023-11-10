// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.IPersonPictureStatics
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Microsoft.UI.Xaml.Controls
{
  [Guid(3689679234, 15376, 17977, 150, 20, 170, 91, 124, 220, 50, 201)]
  [ExclusiveTo(typeof (PersonPicture))]
  [Version(65536)]
  internal interface IPersonPictureStatics
  {
    DependencyProperty BadgeNumberProperty { get; }

    DependencyProperty BadgeGlyphProperty { get; }

    DependencyProperty BadgeImageSourceProperty { get; }

    DependencyProperty BadgeTextProperty { get; }

    DependencyProperty IsGroupProperty { get; }

    DependencyProperty ContactProperty { get; }

    DependencyProperty DisplayNameProperty { get; }

    DependencyProperty InitialsProperty { get; }

    DependencyProperty PreferSmallImageProperty { get; }

    DependencyProperty ProfilePictureProperty { get; }
  }
}
