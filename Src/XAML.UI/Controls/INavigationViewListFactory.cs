// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.INavigationViewListFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [Guid(4209939777, 50111, 18367, 185, 4, 97, 85, 244, 223, 107, 78)]
  [ExclusiveTo(typeof (NavigationViewList))]
  [Version(65536)]
  internal interface INavigationViewListFactory
  {
    NavigationViewList CreateInstance([In] object outer, out object inner);
  }
}
