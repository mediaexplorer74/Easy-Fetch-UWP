// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.IIconSource
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml.Media;

namespace Microsoft.UI.Xaml.Controls
{
  [Version(65536)]
  [ExclusiveTo(typeof (IconSource))]
  [Guid(2464976213, 17899, 18340, 134, 60, 145, 178, 36, 4, 79, 154)]
  internal interface IIconSource
  {
    Brush Foreground { get; [param: In] set; }
  }
}
