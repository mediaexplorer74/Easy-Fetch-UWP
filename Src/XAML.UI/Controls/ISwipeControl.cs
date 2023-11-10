// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.ISwipeControl
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [Version(65536)]
  [Guid(2665732463, 26372, 18467, 170, 21, 28, 20, 59, 197, 60, 246)]
  [ExclusiveTo(typeof (SwipeControl))]
  internal interface ISwipeControl
  {
    SwipeItems LeftItems { get; [param: In] set; }

    SwipeItems RightItems { get; [param: In] set; }

    SwipeItems TopItems { get; [param: In] set; }

    SwipeItems BottomItems { get; [param: In] set; }

    void Close();
  }
}
