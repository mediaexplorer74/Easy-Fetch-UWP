// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.ISwipeItem
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Microsoft.UI.Xaml.Controls
{
  [Guid(2205080284, 17864, 18944, 144, 160, 113, 7, 250, 137, 74, 26)]
  [ExclusiveTo(typeof (SwipeItem))]
  [Version(65536)]
  internal interface ISwipeItem
  {
    string Text { get; [param: In] set; }

    IconSource IconSource { get; [param: In] set; }

    Brush Background { get; [param: In] set; }

    Brush Foreground { get; [param: In] set; }

    ICommand Command { get; [param: In] set; }

    object CommandParameter { get; [param: In] set; }

    SwipeBehaviorOnInvoked BehaviorOnInvoked { get; [param: In] set; }

    event TypedEventHandler<SwipeItem, SwipeItemInvokedEventArgs> Invoked;
  }
}
