// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.ITreeViewNodeFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [ExclusiveTo(typeof (TreeViewNode))]
  [Guid(1858481552, 43545, 16714, 138, 69, 10, 85, 137, 168, 54, 245)]
  [Version(65536)]
  internal interface ITreeViewNodeFactory
  {
    TreeViewNode CreateInstance([In] object outer, out object inner);
  }
}
