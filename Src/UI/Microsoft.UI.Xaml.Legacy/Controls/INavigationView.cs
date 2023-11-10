// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.INavigationView
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.UI.Xaml.Controls
{
  [Guid(4060728853, 14618, 17098, 159, 198, 247, 157, 166, 90, 202, 49)]
  [Version(65536)]
  [ExclusiveTo(typeof (NavigationView))]
  internal interface INavigationView
  {
    bool IsPaneOpen { get; [param: In] set; }

    double CompactModeThresholdWidth { get; [param: In] set; }

    double ExpandedModeThresholdWidth { get; [param: In] set; }

    UIElement PaneFooter { get; [param: In] set; }

    object Header { get; [param: In] set; }

    DataTemplate HeaderTemplate { get; [param: In] set; }

    NavigationViewDisplayMode DisplayMode { get; }

    bool IsSettingsVisible { get; [param: In] set; }

    bool IsPaneToggleButtonVisible { get; [param: In] set; }

    bool AlwaysShowHeader { get; [param: In] set; }

    double CompactPaneLength { get; [param: In] set; }

    double OpenPaneLength { get; [param: In] set; }

    Style PaneToggleButtonStyle { get; [param: In] set; }

    object SelectedItem { get; [param: In] set; }

    IVector<object> MenuItems { get; }

    object MenuItemsSource { get; [param: In] set; }

    object SettingsItem { get; }

    AutoSuggestBox AutoSuggestBox { get; [param: In] set; }

    DataTemplate MenuItemTemplate { get; [param: In] set; }

    DataTemplateSelector MenuItemTemplateSelector { get; [param: In] set; }

    Style MenuItemContainerStyle { get; [param: In] set; }

    StyleSelector MenuItemContainerStyleSelector { get; [param: In] set; }

    object MenuItemFromContainer([In] DependencyObject container);

    DependencyObject ContainerFromMenuItem([In] object item);

    event TypedEventHandler<NavigationView, NavigationViewSelectionChangedEventArgs> SelectionChanged;

    event TypedEventHandler<NavigationView, NavigationViewItemInvokedEventArgs> ItemInvoked;

    event TypedEventHandler<NavigationView, NavigationViewDisplayModeChangedEventArgs> DisplayModeChanged;
  }
}
