// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Functions.GeneratorResult
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core.Interop;

namespace NiL.JS.Core.Functions
{
  internal sealed class GeneratorResult : IIteratorResult
  {
    private JSValue _value;
    private bool _done;

    [Field]
    public JSValue value => this._value;

    [Field]
    public bool done => this._done;

    [Hidden]
    public GeneratorResult(JSValue value, bool done)
    {
      this._value = value;
      this._done = done;
    }
  }
}
