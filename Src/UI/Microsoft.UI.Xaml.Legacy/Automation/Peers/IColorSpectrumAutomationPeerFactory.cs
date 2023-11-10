// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Automation.Peers.IColorSpectrumAutomationPeerFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using Microsoft.UI.Xaml.Controls.Primitives;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Automation.Peers
{
  [Version(65536)]
  [Guid(180617441, 46915, 17558, 131, 122, 136, 137, 230, 172, 100, 150)]
  [ExclusiveTo(typeof (ColorSpectrumAutomationPeer))]
  internal interface IColorSpectrumAutomationPeerFactory
  {
    ColorSpectrumAutomationPeer CreateInstanceWithOwner(
      [In] ColorSpectrum owner,
      [In] object outer,
      out object inner);
  }
}
