// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.NavigationViewDisplayModeChangedEventArgs
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.CompilerServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [MarshalingBehavior]
  [Threading]
  [Version(65536)]
  public sealed class NavigationViewDisplayModeChangedEventArgs : 
    INavigationViewDisplayModeChangedEventArgs
  {
    public extern NavigationViewDisplayMode DisplayMode { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }
  }
}
