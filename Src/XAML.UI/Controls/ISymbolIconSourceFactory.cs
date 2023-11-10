// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.ISymbolIconSourceFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [Version(65536)]
  [Guid(2770774704, 16688, 18695, 176, 73, 33, 249, 36, 12, 122, 79)]
  [ExclusiveTo(typeof (SymbolIconSource))]
  internal interface ISymbolIconSourceFactory
  {
    SymbolIconSource CreateInstance([In] object outer, out object inner);
  }
}
