// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Media.IRevealBrushStatics
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Microsoft.UI.Xaml.Media
{
  [Version(65536)]
  [Guid(420423205, 29193, 19778, 168, 71, 26, 196, 187, 187, 52, 152)]
  [ExclusiveTo(typeof (RevealBrush))]
  internal interface IRevealBrushStatics
  {
    DependencyProperty ColorProperty { get; }

    DependencyProperty TargetThemeProperty { get; }

    DependencyProperty AlwaysUseFallbackProperty { get; }

    DependencyProperty StateProperty { get; }

    void SetState([In] UIElement element, [In] RevealBrushState value);

    RevealBrushState GetState([In] UIElement element);
  }
}
