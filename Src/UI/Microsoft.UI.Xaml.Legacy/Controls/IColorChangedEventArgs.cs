// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.IColorChangedEventArgs
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using Windows.Foundation.Metadata;
using Windows.UI;

namespace Microsoft.UI.Xaml.Controls
{
  [Guid(888610847, 43728, 19514, 185, 123, 42, 191, 54, 69, 85, 56)]
  [ExclusiveTo(typeof (ColorChangedEventArgs))]
  [Version(65536)]
  internal interface IColorChangedEventArgs
  {
    Color OldColor { get; }

    Color NewColor { get; }
  }
}
