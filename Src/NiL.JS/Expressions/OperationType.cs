// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.OperationType
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

namespace NiL.JS.Expressions
{
  internal enum OperationType
  {
    None = 0,
    Assignment = 16, // 0x00000010
    Conditional = 32, // 0x00000020
    LogicalOr = 48, // 0x00000030
    LogicalAnd = 64, // 0x00000040
    NullishCoalescing = 80, // 0x00000050
    Or = 96, // 0x00000060
    Xor = 112, // 0x00000070
    And = 128, // 0x00000080
    Equal = 144, // 0x00000090
    NotEqual = 145, // 0x00000091
    StrictEqual = 146, // 0x00000092
    StrictNotEqual = 147, // 0x00000093
    InstanceOf = 160, // 0x000000A0
    In = 161, // 0x000000A1
    More = 162, // 0x000000A2
    Less = 163, // 0x000000A3
    MoreOrEqual = 164, // 0x000000A4
    LessOrEqual = 165, // 0x000000A5
    SignedShiftLeft = 176, // 0x000000B0
    SignedShiftRight = 177, // 0x000000B1
    UnsignedShiftRight = 178, // 0x000000B2
    Addition = 192, // 0x000000C0
    Substract = 193, // 0x000000C1
    Multiply = 208, // 0x000000D0
    Modulo = 209, // 0x000000D1
    Division = 210, // 0x000000D2
    Power = 224, // 0x000000E0
    Negative = 240, // 0x000000F0
    Positive = 241, // 0x000000F1
    LogicalNot = 242, // 0x000000F2
    Not = 243, // 0x000000F3
    TypeOf = 244, // 0x000000F4
    Delete = 245, // 0x000000F5
    Incriment = 256, // 0x00000100
    Decriment = 257, // 0x00000101
    Call = 4080, // 0x00000FF0
    New = 4082, // 0x00000FF2
    Yield = 4084, // 0x00000FF4
  }
}
