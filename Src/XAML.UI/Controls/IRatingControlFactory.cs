// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.IRatingControlFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [Guid(416814870, 50498, 19659, 179, 71, 94, 98, 197, 219, 120, 45)]
  [Version(65536)]
  [ExclusiveTo(typeof (RatingControl))]
  internal interface IRatingControlFactory
  {
    RatingControl CreateInstance([In] object outer, out object inner);
  }
}
