// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.IPersonPictureFactory
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace Microsoft.UI.Xaml.Controls
{
  [Version(65536)]
  [Guid(1326985997, 1046, 19346, 191, 211, 191, 87, 128, 180, 106, 177)]
  [ExclusiveTo(typeof (PersonPicture))]
  internal interface IPersonPictureFactory
  {
    PersonPicture CreateInstance([In] object outer, out object inner);
  }
}
