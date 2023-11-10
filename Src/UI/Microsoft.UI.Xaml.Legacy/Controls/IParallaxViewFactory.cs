// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.IParallaxViewFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [ExclusiveTo(typeof (ParallaxView))]
  [Version(65536)]
  [Guid(3840644674, 16014, 23078, 148, 242, 145, 33, 209, 33, 185, 21)]
  internal interface IParallaxViewFactory
  {
    ParallaxView CreateInstance([In] object outer, out object inner);
  }
}
