// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.INavigationViewItemHeaderFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [Version(65536)]
  [ExclusiveTo(typeof (NavigationViewItemHeader))]
  [Guid(4077934984, 30568, 17875, 139, 176, 109, 237, 158, 67, 169, 138)]
  internal interface INavigationViewItemHeaderFactory
  {
    NavigationViewItemHeader CreateInstance([In] object outer, out object inner);
  }
}
