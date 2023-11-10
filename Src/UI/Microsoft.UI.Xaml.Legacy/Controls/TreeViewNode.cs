// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.TreeViewNode
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Interop;

namespace Microsoft.UI.Xaml.Controls
{
  [Version(65536)]
  [Static(typeof (ITreeViewNodeStatics), 65536)]
  [Bindable]
  [MarshalingBehavior]
  [Threading]
  [Composable]
  public class TreeViewNode : 
    DependencyObject,
    IBindableObservableVector,
    IBindableIterable,
    IBindableVector,
    ITreeViewNode
  {
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern TreeViewNode();

    public extern event BindableVectorChangedEventHandler VectorChanged;

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern object GetAt([In] uint index);

    public extern uint Size { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern IBindableVectorView GetView();

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern bool IndexOf([In] object value, out uint index);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void SetAt([In] uint index, [In] object value);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void InsertAt([In] uint index, [In] object value);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void RemoveAt([In] uint index);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void Append([In] object value);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void RemoveAtEnd();

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void Clear();

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern IBindableIterator First();

    public extern object Data { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern TreeViewNode ParentNode { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern bool IsExpanded { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern bool HasItems { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public extern int Depth { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public extern bool HasUnrealizedItems { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern event TypedEventHandler<TreeViewNode, DependencyPropertyChangedEventArgs> IsExpandedChanged;

    public static extern DependencyProperty DataProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty DepthProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty IsExpandedProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty HasItemsProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }
  }
}
