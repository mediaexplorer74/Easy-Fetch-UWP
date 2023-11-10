// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Media.IAcrylicBrush
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI;

namespace Microsoft.UI.Xaml.Media
{
  [Guid(2042351438, 52582, 20251, 168, 182, 205, 109, 41, 119, 193, 140)]
  [Version(65536)]
  [ExclusiveTo(typeof (AcrylicBrush))]
  internal interface IAcrylicBrush
  {
    AcrylicBackgroundSource BackgroundSource { get; [param: In] set; }

    Color TintColor { get; [param: In] set; }

    double TintOpacity { get; [param: In] set; }

    TimeSpan TintTransitionDuration { get; [param: In] set; }

    bool AlwaysUseFallback { get; [param: In] set; }
  }
}
