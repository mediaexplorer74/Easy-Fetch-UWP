// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.IRatingItemFontInfoFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [Version(65536)]
  [ExclusiveTo(typeof (RatingItemFontInfo))]
  [Guid(2516844118, 40607, 16565, 186, 225, 68, 129, 187, 115, 188, 210)]
  internal interface IRatingItemFontInfoFactory
  {
    RatingItemFontInfo CreateInstance([In] object outer, out object inner);
  }
}
