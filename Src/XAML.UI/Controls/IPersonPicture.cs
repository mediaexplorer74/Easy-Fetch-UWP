// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.IPersonPicture
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.ApplicationModel.Contacts;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml.Media;

namespace Microsoft.UI.Xaml.Controls
{
  [ExclusiveTo(typeof (PersonPicture))]
  [Version(65536)]
  [Guid(1814236013, 3445, 16473, 145, 188, 123, 23, 77, 29, 115, 20)]
  internal interface IPersonPicture
  {
    int BadgeNumber { get; [param: In] set; }

    string BadgeGlyph { get; [param: In] set; }

    ImageSource BadgeImageSource { get; [param: In] set; }

    string BadgeText { get; [param: In] set; }

    bool IsGroup { get; [param: In] set; }

    Contact Contact { get; [param: In] set; }

    string DisplayName { get; [param: In] set; }

    string Initials { get; [param: In] set; }

    bool PreferSmallImage { get; [param: In] set; }

    ImageSource ProfilePicture { get; [param: In] set; }
  }
}
