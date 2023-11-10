// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Automation.Peers.TreeViewListAutomationPeer
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using Microsoft.UI.Xaml.Controls;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml.Automation.Peers;

namespace Microsoft.UI.Xaml.Automation.Peers
{
  [Composable]
  [Version(65536)]
  [Threading]
  [MarshalingBehavior]
  public class TreeViewListAutomationPeer : SelectorAutomationPeer, ITreeViewListAutomationPeer
  {
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern TreeViewListAutomationPeer([In] TreeViewList owner);
  }
}
