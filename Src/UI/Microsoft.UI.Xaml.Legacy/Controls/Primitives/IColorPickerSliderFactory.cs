// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.Primitives.IColorPickerSliderFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls.Primitives
{
  [Version(65536)]
  [Guid(114850210, 35847, 19230, 169, 64, 159, 188, 232, 244, 150, 56)]
  [ExclusiveTo(typeof (ColorPickerSlider))]
  internal interface IColorPickerSliderFactory
  {
    ColorPickerSlider CreateInstance([In] object outer, out object inner);
  }
}
