// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.SwipeItems
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Microsoft.UI.Xaml.Controls
{
  [Version(65536)]
  [Composable]
  [MarshalingBehavior]
  [Threading]
  [Static(typeof (ISwipeItemsStatics), 65536)]
  public class SwipeItems : DependencyObject, ISwipeItems, IVector<SwipeItem>, IIterable<SwipeItem>
  {
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern SwipeItems();

    public extern SwipeMode Mode { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern SwipeItem GetAt([In] uint index);

    public extern uint Size { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern IVectorView<SwipeItem> GetView();

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern bool IndexOf([In] SwipeItem value, out uint index);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void SetAt([In] uint index, [In] SwipeItem value);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void InsertAt([In] uint index, [In] SwipeItem value);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void RemoveAt([In] uint index);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void Append([In] SwipeItem value);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void RemoveAtEnd();

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void Clear();

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern uint GetMany([In] uint startIndex, [Out] SwipeItem[] items);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void ReplaceAll([In] SwipeItem[] items);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern IIterator<SwipeItem> First();

    public static extern DependencyProperty ModeProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }
  }
}
