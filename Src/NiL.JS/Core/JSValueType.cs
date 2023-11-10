// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.JSValueType
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

namespace NiL.JS.Core
{
  public enum JSValueType
  {
    NotExists = 0,
    NotExistsInObject = 1,
    Undefined = 3,
    Boolean = 7,
    Integer = 11, // 0x0000000B
    Double = 19, // 0x00000013
    String = 35, // 0x00000023
    Symbol = 67, // 0x00000043
    Object = 131, // 0x00000083
    Function = 259, // 0x00000103
    Date = 515, // 0x00000203
    Property = 1027, // 0x00000403
    SpreadOperatorResult = 2051, // 0x00000803
  }
}
