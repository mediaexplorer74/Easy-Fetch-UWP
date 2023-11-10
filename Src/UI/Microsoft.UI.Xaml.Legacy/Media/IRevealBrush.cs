// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Media.IRevealBrush
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.Xaml;

namespace Microsoft.UI.Xaml.Media
{
  [ExclusiveTo(typeof (RevealBrush))]
  [Version(65536)]
  [Guid(540451053, 33393, 17304, 144, 25, 37, 135, 32, 147, 241, 62)]
  internal interface IRevealBrush
  {
    Color Color { get; [param: In] set; }

    ApplicationTheme TargetTheme { get; [param: In] set; }

    bool AlwaysUseFallback { get; [param: In] set; }
  }
}
