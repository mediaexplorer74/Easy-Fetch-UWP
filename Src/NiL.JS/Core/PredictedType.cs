// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.PredictedType
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

namespace NiL.JS.Core
{
  public enum PredictedType
  {
    Unknown = 0,
    Ambiguous = 16, // 0x00000010
    Undefined = 32, // 0x00000020
    Bool = 48, // 0x00000030
    Number = 64, // 0x00000040
    Int = 65, // 0x00000041
    Double = 66, // 0x00000042
    String = 80, // 0x00000050
    Object = 96, // 0x00000060
    Function = 97, // 0x00000061
    Class = 98, // 0x00000062
    Group = 240, // 0x000000F0
    Full = 255, // 0x000000FF
  }
}
