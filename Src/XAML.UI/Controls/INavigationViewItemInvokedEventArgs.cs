// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.INavigationViewItemInvokedEventArgs
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [Version(65536)]
  [Guid(692676642, 21970, 18938, 150, 75, 241, 219, 175, 239, 133, 195)]
  [ExclusiveTo(typeof (NavigationViewItemInvokedEventArgs))]
  internal interface INavigationViewItemInvokedEventArgs
  {
    object InvokedItem { get; }

    bool IsSettingsInvoked { get; }
  }
}
