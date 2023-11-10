// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Automation.Peers.IColorPickerSliderAutomationPeerFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using Microsoft.UI.Xaml.Controls.Primitives;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Automation.Peers
{
  [ExclusiveTo(typeof (ColorPickerSliderAutomationPeer))]
  [Guid(441829246, 40406, 17827, 144, 66, 180, 2, 0, 254, 161, 168)]
  [Version(65536)]
  internal interface IColorPickerSliderAutomationPeerFactory
  {
    ColorPickerSliderAutomationPeer CreateInstanceWithOwner(
      [In] ColorPickerSlider owner,
      [In] object outer,
      out object inner);
  }
}
