// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Media.IRevealBorderBrushFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Media
{
  [Version(65536)]
  [Guid(2495763096, 62968, 17538, 162, 92, 103, 88, 80, 26, 134, 37)]
  [ExclusiveTo(typeof (RevealBorderBrush))]
  internal interface IRevealBorderBrushFactory
  {
    RevealBorderBrush CreateInstance([In] object outer, out object inner);
  }
}
