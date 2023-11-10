// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.ISwipeControlFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [Version(65536)]
  [ExclusiveTo(typeof (SwipeControl))]
  [Guid(3232408494, 53569, 19986, 167, 40, 95, 149, 181, 7, 231, 170)]
  internal interface ISwipeControlFactory
  {
    SwipeControl CreateInstance([In] object outer, out object inner);
  }
}
