// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.IRevealListViewItemPresenterFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [Guid(737125565, 34133, 19158, 168, 209, 36, 47, 232, 226, 189, 119)]
  [Version(65536)]
  [ExclusiveTo(typeof (RevealListViewItemPresenter))]
  internal interface IRevealListViewItemPresenterFactory
  {
    RevealListViewItemPresenter CreateInstance([In] object outer, out object inner);
  }
}
