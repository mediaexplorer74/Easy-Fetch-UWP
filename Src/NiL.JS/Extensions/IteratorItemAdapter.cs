// Decompiled with JetBrains decompiler
// Type: NiL.JS.Extensions.IteratorItemAdapter
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Core.Interop;

namespace NiL.JS.Extensions
{
  internal sealed class IteratorItemAdapter : IterableProtocolBase, IIteratorResult
  {
    private JSValue result;

    [Hidden]
    public IteratorItemAdapter(JSValue result) => this.result = result;

    public JSValue value => Tools.InvokeGetter(this.result[nameof (value)], this.result);

    public bool done => (bool) Tools.InvokeGetter(this.result[nameof (done)], this.result);
  }
}
