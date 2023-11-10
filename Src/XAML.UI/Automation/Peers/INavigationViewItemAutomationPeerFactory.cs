// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Automation.Peers.INavigationViewItemAutomationPeerFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using Microsoft.UI.Xaml.Controls;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Automation.Peers
{
  [ExclusiveTo(typeof (NavigationViewItemAutomationPeer))]
  [Version(65536)]
  [Guid(197296989, 43576, 20375, 150, 100, 230, 252, 130, 29, 129, 236)]
  internal interface INavigationViewItemAutomationPeerFactory
  {
    NavigationViewItemAutomationPeer CreateInstanceWithOwner(
      [In] NavigationViewItem owner,
      [In] object outer,
      out object inner);
  }
}
