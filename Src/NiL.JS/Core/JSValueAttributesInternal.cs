// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.JSValueAttributesInternal
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using System;

namespace NiL.JS.Core
{
  [Flags]
  internal enum JSValueAttributesInternal : uint
  {
    None = 0,
    DoNotEnumerate = 1,
    DoNotDelete = 2,
    ReadOnly = 4,
    Immutable = 8,
    NonConfigurable = 16, // 0x00000010
    Argument = 65536, // 0x00010000
    SystemObject = 131072, // 0x00020000
    ProxyPrototype = 262144, // 0x00040000
    Field = 524288, // 0x00080000
    Eval = 1048576, // 0x00100000
    Temporary = 2097152, // 0x00200000
    Cloned = 4194304, // 0x00400000
    Reassign = 33554432, // 0x02000000
    IntrinsicFunction = 67108864, // 0x04000000
    ConstructingObject = 134217728, // 0x08000000
  }
}
