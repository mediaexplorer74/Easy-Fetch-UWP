// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.IRatingItemFontInfo
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [ExclusiveTo(typeof (RatingItemFontInfo))]
  [Guid(88548329, 31113, 19804, 157, 1, 167, 235, 135, 111, 16, 112)]
  [Version(65536)]
  internal interface IRatingItemFontInfo
  {
    string DisabledGlyph { get; [param: In] set; }

    string Glyph { get; [param: In] set; }

    string PointerOverGlyph { get; [param: In] set; }

    string PointerOverPlaceholderGlyph { get; [param: In] set; }

    string PlaceholderGlyph { get; [param: In] set; }

    string UnsetGlyph { get; [param: In] set; }
  }
}
