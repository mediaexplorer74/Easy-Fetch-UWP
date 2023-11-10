// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.ITreeViewFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [Guid(3427952579, 27753, 18894, 180, 69, 117, 58, 206, 231, 148, 137)]
  [Version(65536)]
  [ExclusiveTo(typeof (TreeView))]
  internal interface ITreeViewFactory
  {
    TreeView CreateInstance([In] object outer, out object inner);
  }
}
