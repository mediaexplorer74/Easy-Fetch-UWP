// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Media.IRevealBrushFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Media
{
  [Guid(2643687886, 58272, 19119, 190, 55, 234, 157, 157, 212, 49, 4)]
  [ExclusiveTo(typeof (RevealBrush))]
  [Version(65536)]
  internal interface IRevealBrushFactory
  {
    RevealBrush CreateInstance([In] object outer, out object inner);
  }
}
