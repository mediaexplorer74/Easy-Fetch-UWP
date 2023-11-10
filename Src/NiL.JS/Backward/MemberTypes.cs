// Decompiled with JetBrains decompiler
// Type: NiL.JS.Backward.MemberTypes
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

namespace NiL.JS.Backward
{
  internal enum MemberTypes
  {
    Constructor = 1,
    Event = 2,
    Field = 4,
    Method = 8,
    Property = 16, // 0x00000010
    TypeInfo = 32, // 0x00000020
    Custom = 64, // 0x00000040
    NestedType = 128, // 0x00000080
    All = 191, // 0x000000BF
  }
}
