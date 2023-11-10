// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.ISwipeItemsFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [ExclusiveTo(typeof (SwipeItems))]
  [Guid(1204052206, 54698, 17503, 179, 30, 80, 192, 118, 192, 17, 184)]
  [Version(65536)]
  internal interface ISwipeItemsFactory
  {
    SwipeItems CreateInstance([In] object outer, out object inner);
  }
}
