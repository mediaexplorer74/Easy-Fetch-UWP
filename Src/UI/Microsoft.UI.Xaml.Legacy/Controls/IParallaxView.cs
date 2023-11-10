// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.IParallaxView
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Microsoft.UI.Xaml.Controls
{
  [Guid(1803877588, 16515, 23371, 188, 64, 217, 32, 78, 25, 180, 25)]
  [Version(65536)]
  [ExclusiveTo(typeof (ParallaxView))]
  internal interface IParallaxView
  {
    UIElement Child { get; [param: In] set; }

    double HorizontalShift { get; [param: In] set; }

    double HorizontalSourceEndOffset { get; [param: In] set; }

    ParallaxSourceOffsetKind HorizontalSourceOffsetKind { get; [param: In] set; }

    double HorizontalSourceStartOffset { get; [param: In] set; }

    bool IsHorizontalShiftClamped { get; [param: In] set; }

    bool IsVerticalShiftClamped { get; [param: In] set; }

    double MaxHorizontalShiftRatio { get; [param: In] set; }

    double MaxVerticalShiftRatio { get; [param: In] set; }

    UIElement Source { get; [param: In] set; }

    double VerticalShift { get; [param: In] set; }

    double VerticalSourceEndOffset { get; [param: In] set; }

    ParallaxSourceOffsetKind VerticalSourceOffsetKind { get; [param: In] set; }

    double VerticalSourceStartOffset { get; [param: In] set; }

    void RefreshAutomaticHorizontalOffsets();

    void RefreshAutomaticVerticalOffsets();
  }
}
