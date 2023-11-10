// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Automation.Peers.ITreeViewListAutomationPeerFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using Microsoft.UI.Xaml.Controls;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Automation.Peers
{
  [Guid(16095202, 63505, 18266, 191, 230, 41, 15, 231, 7, 250, 135)]
  [Version(65536)]
  [ExclusiveTo(typeof (TreeViewListAutomationPeer))]
  internal interface ITreeViewListAutomationPeerFactory
  {
    TreeViewListAutomationPeer CreateInstanceWithOwner(
      [In] TreeViewList owner,
      [In] object outer,
      out object inner);
  }
}
