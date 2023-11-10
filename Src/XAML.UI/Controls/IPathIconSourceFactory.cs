// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.IPathIconSourceFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [ExclusiveTo(typeof (PathIconSource))]
  [Guid(2407499193, 21063, 20283, 131, 63, 227, 132, 191, 126, 156, 131)]
  [Version(65536)]
  internal interface IPathIconSourceFactory
  {
    PathIconSource CreateInstance([In] object outer, out object inner);
  }
}
