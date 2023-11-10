// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.ISymbolIconSource
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml.Controls;

namespace Microsoft.UI.Xaml.Controls
{
  [Version(65536)]
  [ExclusiveTo(typeof (SymbolIconSource))]
  [Guid(1650300322, 46769, 16522, 178, 137, 234, 178, 236, 186, 98, 215)]
  internal interface ISymbolIconSource
  {
    Symbol Symbol { get; [param: In] set; }
  }
}
