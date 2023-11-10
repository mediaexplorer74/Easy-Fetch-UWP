// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Media.IRevealBackgroundBrushFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Media
{
  [ExclusiveTo(typeof (RevealBackgroundBrush))]
  [Version(65536)]
  [Guid(2354494634, 677, 20293, 133, 6, 141, 57, 34, 143, 93, 62)]
  internal interface IRevealBackgroundBrushFactory
  {
    RevealBackgroundBrush CreateInstance([In] object outer, out object inner);
  }
}
