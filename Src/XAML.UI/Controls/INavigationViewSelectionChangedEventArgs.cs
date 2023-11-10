// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.INavigationViewSelectionChangedEventArgs
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [Version(65536)]
  [Guid(1520765344, 14942, 20308, 137, 108, 152, 184, 95, 129, 149, 7)]
  [ExclusiveTo(typeof (NavigationViewSelectionChangedEventArgs))]
  internal interface INavigationViewSelectionChangedEventArgs
  {
    object SelectedItem { get; }

    bool IsSettingsSelected { get; }
  }
}
