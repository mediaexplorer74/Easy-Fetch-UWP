// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.IRatingItemImageInfoStatics
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Microsoft.UI.Xaml.Controls
{
  [Guid(2218281067, 19228, 16675, 186, 11, 115, 72, 77, 104, 195, 55)]
  [ExclusiveTo(typeof (RatingItemImageInfo))]
  [Version(65536)]
  internal interface IRatingItemImageInfoStatics
  {
    DependencyProperty DisabledImageProperty { get; }

    DependencyProperty ImageProperty { get; }

    DependencyProperty PlaceholderImageProperty { get; }

    DependencyProperty PointerOverImageProperty { get; }

    DependencyProperty PointerOverPlaceholderImageProperty { get; }

    DependencyProperty UnsetImageProperty { get; }
  }
}
