// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.IRatingItemImageInfoFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [Version(65536)]
  [ExclusiveTo(typeof (RatingItemImageInfo))]
  [Guid(647889906, 55929, 18311, 159, 74, 36, 166, 250, 86, 205, 225)]
  internal interface IRatingItemImageInfoFactory
  {
    RatingItemImageInfo CreateInstance([In] object outer, out object inner);
  }
}
