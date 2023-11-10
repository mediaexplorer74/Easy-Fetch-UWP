// Decompiled with JetBrains decompiler
// Type: NiL.JS.Extensions.IteratorAdapter
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using NiL.JS.Core.Interop;

namespace NiL.JS.Extensions
{
  internal sealed class IteratorAdapter : IterableProtocolBase, IIterator, IIterable
  {
    private JSValue iterator;

    [Hidden]
    public IteratorAdapter(JSValue iterator) => this.iterator = iterator;

    public IIteratorResult next(Arguments arguments = null) => (IIteratorResult) new IteratorItemAdapter(this.iterator[nameof (next)].As<Function>().Call(this.iterator, arguments));

    public IIteratorResult @return() => (IIteratorResult) new IteratorItemAdapter(this.iterator[nameof (@return)].As<Function>().Call(this.iterator, (Arguments) null));

    public IIteratorResult @throw(Arguments arguments = null) => (IIteratorResult) new IteratorItemAdapter(this.iterator[nameof (@throw)].As<Function>().Call(this.iterator, (Arguments) null));

    IIterator IIterable.iterator() => (IIterator) this;
  }
}
