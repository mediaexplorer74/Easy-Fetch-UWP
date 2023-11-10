// Decompiled with JetBrains decompiler
// Type: NiL.JS.Extensions.EnumeratorToIteratorWrapper
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Core.Interop;
using System.Collections;

namespace NiL.JS.Extensions
{
  internal sealed class EnumeratorToIteratorWrapper : IterableProtocolBase, IIterator, IIterable
  {
    private IEnumerator _enumerator;
    private GlobalContext _context;

    [Hidden]
    public EnumeratorToIteratorWrapper(IEnumerator enumerator)
    {
      this._enumerator = enumerator;
      this._context = Context.CurrentGlobalContext;
    }

    public IIterator iterator() => (IIterator) this;

    public IIteratorResult next(Arguments arguments = null)
    {
      bool flag = this._enumerator.MoveNext();
      return (IIteratorResult) new EnumeratorResult(!flag, this._context.ProxyValue(flag ? this._enumerator.Current : (object) null));
    }

    public IIteratorResult @return() => (IIteratorResult) new EnumeratorResult(true, (JSValue) null);

    public IIteratorResult @throw(Arguments arguments = null) => (IIteratorResult) new EnumeratorResult(true, (JSValue) null);
  }
}
