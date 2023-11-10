// Decompiled with JetBrains decompiler
// Type: NiL.JS.Extensions.EnumerableToIterableWrapper
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Core.Interop;
using System.Collections;

namespace NiL.JS.Extensions
{
  internal sealed class EnumerableToIterableWrapper : IterableProtocolBase, IIterable
  {
    private IEnumerable enumerable;

    [Hidden]
    public EnumerableToIterableWrapper(IEnumerable enumerable) => this.enumerable = enumerable;

    public IIterator iterator() => (IIterator) new EnumeratorToIteratorWrapper(this.enumerable.GetEnumerator());
  }
}
