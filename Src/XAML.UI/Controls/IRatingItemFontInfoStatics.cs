// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.IRatingItemFontInfoStatics
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Microsoft.UI.Xaml.Controls
{
  [ExclusiveTo(typeof (RatingItemFontInfo))]
  [Guid(4213264129, 25652, 20203, 130, 255, 124, 229, 212, 11, 245, 31)]
  [Version(65536)]
  internal interface IRatingItemFontInfoStatics
  {
    DependencyProperty DisabledGlyphProperty { get; }

    DependencyProperty GlyphProperty { get; }

    DependencyProperty PlaceholderGlyphProperty { get; }

    DependencyProperty PointerOverGlyphProperty { get; }

    DependencyProperty PointerOverPlaceholderGlyphProperty { get; }

    DependencyProperty UnsetGlyphProperty { get; }
  }
}
