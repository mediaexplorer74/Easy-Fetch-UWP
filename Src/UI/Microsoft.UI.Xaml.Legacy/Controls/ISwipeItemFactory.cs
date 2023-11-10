// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.ISwipeItemFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [Guid(2219562522, 5910, 16535, 187, 162, 117, 38, 218, 34, 222, 56)]
  [ExclusiveTo(typeof (SwipeItem))]
  [Version(65536)]
  internal interface ISwipeItemFactory
  {
    SwipeItem CreateInstance([In] object outer, out object inner);
  }
}
