// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.RatingItemFontInfo
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Microsoft.UI.Xaml.Controls
{
  [Composable]
  [Version(65536)]
  [Static(typeof (IRatingItemFontInfoStatics), 65536)]
  [Threading]
  [MarshalingBehavior]
  public class RatingItemFontInfo : RatingItemInfo, IRatingItemFontInfo
  {
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern RatingItemFontInfo();

    public extern string DisabledGlyph { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern string Glyph { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern string PointerOverGlyph { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern string PointerOverPlaceholderGlyph { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern string PlaceholderGlyph { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern string UnsetGlyph { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public static extern DependencyProperty DisabledGlyphProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty GlyphProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty PlaceholderGlyphProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty PointerOverGlyphProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty PointerOverPlaceholderGlyphProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty UnsetGlyphProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }
  }
}
