// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.Primitives.IColorSpectrumFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls.Primitives
{
  [Version(65536)]
  [Guid(2429019678, 36941, 17067, 180, 79, 230, 141, 191, 12, 222, 232)]
  [ExclusiveTo(typeof (ColorSpectrum))]
  internal interface IColorSpectrumFactory
  {
    ColorSpectrum CreateInstance([In] object outer, out object inner);
  }
}
