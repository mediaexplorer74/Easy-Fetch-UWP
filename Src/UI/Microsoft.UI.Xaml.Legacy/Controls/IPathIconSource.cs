// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.IPathIconSource
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml.Media;

namespace Microsoft.UI.Xaml.Controls
{
  [Version(65536)]
  [Guid(524747022, 45829, 18531, 139, 122, 21, 208, 141, 99, 60, 121)]
  [ExclusiveTo(typeof (PathIconSource))]
  internal interface IPathIconSource
  {
    Geometry Data { get; [param: In] set; }
  }
}
