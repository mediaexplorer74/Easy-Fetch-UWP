// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.IRatingItemImageInfo
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml.Media;

namespace Microsoft.UI.Xaml.Controls
{
  [ExclusiveTo(typeof (RatingItemImageInfo))]
  [Version(65536)]
  [Guid(623120012, 57554, 18702, 184, 248, 150, 200, 238, 5, 18, 184)]
  internal interface IRatingItemImageInfo
  {
    ImageSource DisabledImage { get; [param: In] set; }

    ImageSource Image { get; [param: In] set; }

    ImageSource PlaceholderImage { get; [param: In] set; }

    ImageSource PointerOverImage { get; [param: In] set; }

    ImageSource PointerOverPlaceholderImage { get; [param: In] set; }

    ImageSource UnsetImage { get; [param: In] set; }
  }
}
