// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.IFontIconSource
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using Windows.UI.Text;
using Windows.UI.Xaml.Media;

namespace Microsoft.UI.Xaml.Controls
{
  [Version(65536)]
  [Guid(571583642, 29923, 17389, 182, 28, 33, 175, 199, 223, 77, 31)]
  [ExclusiveTo(typeof (FontIconSource))]
  internal interface IFontIconSource
  {
    string Glyph { get; [param: In] set; }

    double FontSize { get; [param: In] set; }

    FontFamily FontFamily { get; [param: In] set; }

    FontWeight FontWeight { get; [param: In] set; }

    FontStyle FontStyle { get; [param: In] set; }

    bool IsTextScaleFactorEnabled { get; [param: In] set; }

    bool MirroredWhenRightToLeft { get; [param: In] set; }
  }
}
