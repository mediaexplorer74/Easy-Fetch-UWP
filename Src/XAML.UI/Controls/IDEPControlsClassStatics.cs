// Decompiled with JetBrains decompiler
// Type: Microsoft.UI.Xaml.Controls.IDEPControlsClassStatics
// Assembly: Microsoft.UI.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 5D34FD20-A2B7-40F5-936E-445C45E7C376
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\Microsoft.UI.Xaml.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Microsoft.UI.Xaml.Controls
{
  [Version(65536)]
  [ExclusiveTo(typeof (DEPControlsClass))]
  [Guid(2164519385, 42341, 18864, 129, 119, 11, 22, 238, 161, 60, 252)]
  internal interface IDEPControlsClassStatics
  {
    void Initialize();

    void SetupRevealForFullWindowMedia([In] UIElement descendantOfFullWindow);
  }
}
