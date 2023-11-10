// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Automation.Peers.ITreeViewItemAutomationPeerFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using Microsoft.UI.Xaml.Controls;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Automation.Peers
{
  [Version(65536)]
  [ExclusiveTo(typeof (TreeViewItemAutomationPeer))]
  [Guid(1943242943, 7425, 16729, 130, 192, 43, 41, 150, 219, 253, 205)]
  internal interface ITreeViewItemAutomationPeerFactory
  {
    TreeViewItemAutomationPeer CreateInstanceWithOwner(
      [In] TreeViewItem owner,
      [In] object outer,
      out object inner);
  }
}
