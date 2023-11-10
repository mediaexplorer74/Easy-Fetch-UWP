// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Media.IAcrylicBrushFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Media
{
  [Guid(2174952808, 63180, 16403, 131, 99, 146, 138, 226, 59, 122, 96)]
  [ExclusiveTo(typeof (AcrylicBrush))]
  [Version(65536)]
  internal interface IAcrylicBrushFactory
  {
    AcrylicBrush CreateInstance([In] object outer, out object inner);
  }
}
