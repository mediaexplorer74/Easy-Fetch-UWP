// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.ISwipeControlStatics
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Microsoft.UI.Xaml.Controls
{
  [ExclusiveTo(typeof (SwipeControl))]
  [Version(65536)]
  [Guid(3850037308, 8726, 18199, 158, 246, 70, 98, 51, 76, 101, 0)]
  internal interface ISwipeControlStatics
  {
    DependencyProperty LeftItemsProperty { get; }

    DependencyProperty RightItemsProperty { get; }

    DependencyProperty TopItemsProperty { get; }

    DependencyProperty BottomItemsProperty { get; }
  }
}
