// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.IBitmapIconSource
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [ExclusiveTo(typeof (BitmapIconSource))]
  [Version(65536)]
  [Guid(3370335687, 54446, 19079, 148, 127, 172, 77, 11, 207, 90, 243)]
  internal interface IBitmapIconSource
  {
    Uri UriSource { get; [param: In] set; }

    bool ShowAsMonochrome { get; [param: In] set; }
  }
}
