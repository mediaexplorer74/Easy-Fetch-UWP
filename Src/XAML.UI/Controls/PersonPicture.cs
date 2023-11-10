// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.PersonPicture
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.ApplicationModel.Contacts;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Microsoft.UI.Xaml.Controls
{
  [Threading]
  [Static(typeof (IPersonPictureStatics), 65536)]
  [MarshalingBehavior]
  [Composable]
  [Version(65536)]
  public class PersonPicture : Control, IPersonPicture
  {
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern PersonPicture();

    public extern int BadgeNumber { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern string BadgeGlyph { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern ImageSource BadgeImageSource { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern string BadgeText { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern bool IsGroup { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern Contact Contact { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern string DisplayName { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern string Initials { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern bool PreferSmallImage { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern ImageSource ProfilePicture { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public static extern DependencyProperty BadgeNumberProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty BadgeGlyphProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty BadgeImageSourceProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty BadgeTextProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty IsGroupProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty ContactProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty DisplayNameProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty InitialsProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty PreferSmallImageProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public static extern DependencyProperty ProfilePictureProperty { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }
  }
}
