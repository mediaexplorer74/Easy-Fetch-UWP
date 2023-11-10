// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.ISwipeItemStatics
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Microsoft.UI.Xaml.Controls
{
  [Version(65536)]
  [ExclusiveTo(typeof (SwipeItem))]
  [Guid(3501477526, 10086, 19859, 181, 219, 245, 225, 190, 131, 44, 44)]
  internal interface ISwipeItemStatics
  {
    DependencyProperty IconSourceProperty { get; }

    DependencyProperty TextProperty { get; }

    DependencyProperty BackgroundProperty { get; }

    DependencyProperty ForegroundProperty { get; }

    DependencyProperty CommandProperty { get; }

    DependencyProperty CommandParameterProperty { get; }

    DependencyProperty BehaviorOnInvokedProperty { get; }
  }
}
