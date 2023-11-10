// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.IRatingItemInfoFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [Version(65536)]
  [Guid(2969387990, 53228, 17352, 154, 197, 11, 13, 94, 37, 216, 97)]
  [ExclusiveTo(typeof (RatingItemInfo))]
  internal interface IRatingItemInfoFactory
  {
    RatingItemInfo CreateInstance([In] object outer, out object inner);
  }
}
