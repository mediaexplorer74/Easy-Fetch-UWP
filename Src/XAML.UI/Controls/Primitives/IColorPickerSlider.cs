// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.Primitives.IColorPickerSlider
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls.Primitives
{
  [Guid(2486783363, 57567, 19551, 187, 205, 129, 85, 244, 2, 4, 79)]
  [ExclusiveTo(typeof (ColorPickerSlider))]
  [Version(65536)]
  internal interface IColorPickerSlider
  {
    ColorPickerHsvChannel ColorChannel { get; [param: In] set; }
  }
}
