// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.IColorPickerFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [Guid(2880309247, 44751, 18461, 146, 4, 32, 28, 56, 148, 205, 26)]
  [Version(65536)]
  [ExclusiveTo(typeof (ColorPicker))]
  internal interface IColorPickerFactory
  {
    ColorPicker CreateInstance([In] object outer, out object inner);
  }
}
