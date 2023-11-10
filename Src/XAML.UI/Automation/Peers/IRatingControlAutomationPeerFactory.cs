// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Automation.Peers.IRatingControlAutomationPeerFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using Microsoft.UI.Xaml.Controls;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Automation.Peers
{
  [ExclusiveTo(typeof (RatingControlAutomationPeer))]
  [Version(65536)]
  [Guid(4051300978, 38982, 17970, 139, 156, 190, 111, 168, 211, 201, 186)]
  internal interface IRatingControlAutomationPeerFactory
  {
    RatingControlAutomationPeer CreateInstanceWithOwner(
      [In] RatingControl owner,
      [In] object outer,
      out object inner);
  }
}
