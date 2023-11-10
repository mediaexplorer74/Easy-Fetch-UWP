// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.XamlBooleanToVisibilityConverter
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Interop;

namespace Microsoft.UI.Xaml.Controls
{
  [Version(65536)]
  [Threading]
  [Activatable(65536)]
  [MarshalingBehavior]
  public sealed class XamlBooleanToVisibilityConverter : IValueConverter
  {
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern XamlBooleanToVisibilityConverter();

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern object Convert(
      [In] object value,
      [In] TypeName targetType,
      [In] object parameter,
      [In] string language);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern object ConvertBack(
      [In] object value,
      [In] TypeName targetType,
      [In] object parameter,
      [In] string language);
  }
}
