// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Automation.Peers.IPersonPictureAutomationPeerFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using Microsoft.UI.Xaml.Controls;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Automation.Peers
{
  [Version(65536)]
  [ExclusiveTo(typeof (PersonPictureAutomationPeer))]
  [Guid(2841583469, 9508, 17572, 151, 253, 17, 129, 19, 1, 0, 172)]
  internal interface IPersonPictureAutomationPeerFactory
  {
    PersonPictureAutomationPeer CreateInstanceWithOwner(
      [In] PersonPicture owner,
      [In] object outer,
      out object inner);
  }
}
