// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.DEPControlsThemeResources
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Microsoft.UI.Xaml.Controls
{
  [Static(typeof (IDEPControlsThemeResourcesStatics), 65536)]
  [MarshalingBehavior]
  [Version(65536)]
  [Activatable(65536)]
  [Threading]
  public sealed class DEPControlsThemeResources : ResourceDictionary, IDEPControlsThemeResources
  {
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern DEPControlsThemeResources();

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void EnsureRevealLights([In] UIElement element);
  }
}
