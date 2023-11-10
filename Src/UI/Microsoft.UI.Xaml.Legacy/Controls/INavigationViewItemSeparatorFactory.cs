// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.INavigationViewItemSeparatorFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [ExclusiveTo(typeof (NavigationViewItemSeparator))]
  [Version(65536)]
  [Guid(1909406310, 56198, 18912, 129, 84, 95, 211, 86, 174, 222, 207)]
  internal interface INavigationViewItemSeparatorFactory
  {
    NavigationViewItemSeparator CreateInstance([In] object outer, out object inner);
  }
}
