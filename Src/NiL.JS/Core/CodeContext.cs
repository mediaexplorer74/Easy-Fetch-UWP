// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.CodeContext
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using System;

namespace NiL.JS.Core
{
  [Flags]
  public enum CodeContext
  {
    None = 0,
    Strict = 1,
    Conditional = 4,
    InLoop = 8,
    InWith = 16, // 0x00000010
    InEval = 32, // 0x00000020
    InExpression = 64, // 0x00000040
    InClassDefinition = 128, // 0x00000080
    InClassConstructor = 256, // 0x00000100
    InStaticMember = 512, // 0x00000200
    InGenerator = 1024, // 0x00000400
    InFunction = 2048, // 0x00000800
    InAsync = 4096, // 0x00001000
    InExport = 8192, // 0x00002000
    AllowDirectives = 16384, // 0x00004000
  }
}
