// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.IBitmapIconSourceFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [Guid(1695147462, 17590, 19665, 134, 205, 195, 24, 155, 18, 196, 59)]
  [ExclusiveTo(typeof (BitmapIconSource))]
  [Version(65536)]
  internal interface IBitmapIconSourceFactory
  {
    BitmapIconSource CreateInstance([In] object outer, out object inner);
  }
}
