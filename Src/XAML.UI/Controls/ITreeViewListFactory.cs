// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.ITreeViewListFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [Guid(680485426, 16850, 18167, 177, 245, 105, 28, 98, 82, 100, 181)]
  [ExclusiveTo(typeof (TreeViewList))]
  [Version(65536)]
  internal interface ITreeViewListFactory
  {
    TreeViewList CreateInstance([In] object outer, out object inner);
  }
}
