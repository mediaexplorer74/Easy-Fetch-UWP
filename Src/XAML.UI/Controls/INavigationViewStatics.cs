// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.INavigationViewStatics
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Microsoft.UI.Xaml.Controls
{
  [ExclusiveTo(typeof (NavigationView))]
  [Version(65536)]
  [Guid(909805255, 29402, 17440, 184, 113, 21, 217, 208, 212, 87, 85)]
  internal interface INavigationViewStatics
  {
    DependencyProperty IsPaneOpenProperty { get; }

    DependencyProperty CompactModeThresholdWidthProperty { get; }

    DependencyProperty ExpandedModeThresholdWidthProperty { get; }

    DependencyProperty PaneFooterProperty { get; }

    DependencyProperty HeaderProperty { get; }

    DependencyProperty HeaderTemplateProperty { get; }

    DependencyProperty DisplayModeProperty { get; }

    DependencyProperty IsSettingsVisibleProperty { get; }

    DependencyProperty IsPaneToggleButtonVisibleProperty { get; }

    DependencyProperty AlwaysShowHeaderProperty { get; }

    DependencyProperty CompactPaneLengthProperty { get; }

    DependencyProperty OpenPaneLengthProperty { get; }

    DependencyProperty PaneToggleButtonStyleProperty { get; }

    DependencyProperty MenuItemsProperty { get; }

    DependencyProperty MenuItemsSourceProperty { get; }

    DependencyProperty SelectedItemProperty { get; }

    DependencyProperty SettingsItemProperty { get; }

    DependencyProperty AutoSuggestBoxProperty { get; }

    DependencyProperty MenuItemTemplateProperty { get; }

    DependencyProperty MenuItemTemplateSelectorProperty { get; }

    DependencyProperty MenuItemContainerStyleProperty { get; }

    DependencyProperty MenuItemContainerStyleSelectorProperty { get; }
  }
}
