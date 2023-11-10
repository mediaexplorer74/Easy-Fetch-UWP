// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.IRatingControl
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [ExclusiveTo(typeof (RatingControl))]
  [Version(65536)]
  [Guid(2816023719, 58831, 18787, 162, 78, 150, 115, 254, 95, 253, 212)]
  internal interface IRatingControl
  {
    string Caption { get; [param: In] set; }

    int InitialSetValue { get; [param: In] set; }

    bool IsClearEnabled { get; [param: In] set; }

    bool IsReadOnly { get; [param: In] set; }

    int MaxRating { get; [param: In] set; }

    double PlaceholderValue { get; [param: In] set; }

    RatingItemInfo ItemInfo { get; [param: In] set; }

    double Value { get; [param: In] set; }

    event TypedEventHandler<RatingControl, object> ValueChanged;
  }
}
