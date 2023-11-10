// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Automation.Peers.TreeViewItemAutomationPeer
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using Microsoft.UI.Xaml.Controls;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;

namespace Microsoft.UI.Xaml.Automation.Peers
{
  [Version(65536)]
  [MarshalingBehavior]
  [Threading]
  [Composable]
  public class TreeViewItemAutomationPeer : 
    ListViewItemAutomationPeer,
    IExpandCollapseProvider,
    ITreeViewItemAutomationPeer
  {
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern TreeViewItemAutomationPeer([In] TreeViewItem owner);

    public extern ExpandCollapseState ExpandCollapseState { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void Collapse();

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void Expand();
  }
}
