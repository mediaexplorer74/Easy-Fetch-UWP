// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.ITreeViewItemClickEventArgs
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [ExclusiveTo(typeof (TreeViewItemClickEventArgs))]
  [Guid(1193981217, 578, 17040, 147, 99, 171, 79, 231, 4, 82, 125)]
  [Version(65536)]
  internal interface ITreeViewItemClickEventArgs
  {
    object ClickedItem { get; }

    bool IsHandled { [param: In] set; get; }
  }
}
