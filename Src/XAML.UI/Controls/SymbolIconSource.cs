// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.SymbolIconSource
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.UI.Xaml.Controls
{
  [MarshalingBehavior]
  [Threading]
  [Version(65536)]
  [Composable]
  [Static(typeof (ISymbolIconSourceStatics), 65536)]
  public class SymbolIconSource : IconSource, ISymbolIconSource
  {
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern SymbolIconSource();

    public extern Symbol Symbol { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public static extern DependencyProperty SymbolProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }
  }
}
