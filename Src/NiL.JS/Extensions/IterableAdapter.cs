// Decompiled with JetBrains decompiler
// Type: NiL.JS.Extensions.IterableAdapter
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using NiL.JS.Core.Interop;

namespace NiL.JS.Extensions
{
  internal sealed class IterableAdapter : IterableProtocolBase, IIterable
  {
    private JSValue source;

    [Hidden]
    public IterableAdapter(JSValue source) => this.source = source.IsBox ? source._oValue as JSValue : source;

    public IIterator iterator()
    {
      JSValue property = this.source.GetProperty((JSValue) Symbol.iterator, false, PropertyScope.Common);
      if (property._valueType != JSValueType.Function)
        return (IIterator) null;
      JSValue iterator = property.As<Function>().Call(this.source, (Arguments) null);
      return iterator == null ? (IIterator) null : (IIterator) new IteratorAdapter(iterator);
    }
  }
}
