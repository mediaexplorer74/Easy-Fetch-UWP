// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.DEPControlsClass
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Interop;
using Windows.UI.Xaml.Markup;

namespace Microsoft.UI.Xaml.Controls
{
  [MarshalingBehavior]
  [Threading]
  [Activatable(65536)]
  [Version(65536)]
  [Static(typeof (IDEPControlsClassStatics), 65536)]
  public sealed class DEPControlsClass : IXamlMetadataProvider
  {
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern DEPControlsClass();

    [DefaultOverload]
    [Overload("GetXamlType")]
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern IXamlType GetXamlType([In] TypeName type);

    [Overload("GetXamlTypeByFullName")]
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern IXamlType GetXamlType([In] string fullName);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern XmlnsDefinition[] GetXmlnsDefinitions();

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void Initialize();

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetupRevealForFullWindowMedia([In] UIElement descendantOfFullWindow);
  }
}
