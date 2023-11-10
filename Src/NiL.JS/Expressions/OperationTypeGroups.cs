// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.OperationTypeGroups
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

namespace NiL.JS.Expressions
{
  internal enum OperationTypeGroups
  {
    None = 0,
    Assign = 16, // 0x00000010
    Choice = 32, // 0x00000020
    LogicalOr = 48, // 0x00000030
    LogicalAnd = 64, // 0x00000040
    NullishCoalescing = 80, // 0x00000050
    BitwiseOr = 96, // 0x00000060
    BitwiseXor = 112, // 0x00000070
    BitwiseAnd = 128, // 0x00000080
    Logic1 = 144, // 0x00000090
    Logic2 = 160, // 0x000000A0
    Bit = 176, // 0x000000B0
    Arithmetic0 = 192, // 0x000000C0
    Arithmetic1 = 208, // 0x000000D0
    Power = 224, // 0x000000E0
    Unary0 = 240, // 0x000000F0
    Unary1 = 256, // 0x00000100
    Special = 4080, // 0x00000FF0
  }
}
