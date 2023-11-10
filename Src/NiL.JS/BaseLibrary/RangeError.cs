// Decompiled with JetBrains decompiler
// Type: NiL.JS.BaseLibrary.RangeError
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Core.Interop;

namespace NiL.JS.BaseLibrary
{
  [Prototype(typeof (Error))]
  public sealed class RangeError : Error
  {
    [DoNotEnumerate]
    public RangeError()
    {
    }

    [DoNotEnumerate]
    public RangeError(Arguments args)
      : base(args[0].ToString())
    {
    }

    [DoNotEnumerate]
    public RangeError(string message)
      : base(message)
    {
    }
  }
}
