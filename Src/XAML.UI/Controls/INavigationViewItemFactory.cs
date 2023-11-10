// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.INavigationViewItemFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [ExclusiveTo(typeof (NavigationViewItem))]
  [Version(65536)]
  [Guid(2537282378, 32264, 20342, 146, 60, 241, 43, 214, 133, 232, 109)]
  internal interface INavigationViewItemFactory
  {
    NavigationViewItem CreateInstance([In] object outer, out object inner);
  }
}
