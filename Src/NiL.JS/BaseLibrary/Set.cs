// Decompiled with JetBrains decompiler
// Type: NiL.JS.BaseLibrary.Set
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Core.Interop;
using NiL.JS.Extensions;
using System.Collections.Generic;

namespace NiL.JS.BaseLibrary
{
  [RequireNewKeyword]
  public sealed class Set : IIterable
  {
    private HashSet<object> _storage;

    public int size => this._storage.Count;

    public Set() => this._storage = new HashSet<object>();

    public Set(IIterable iterable)
      : this()
    {
      if (iterable == null)
        return;
      foreach (JSValue jsValue1 in iterable.AsEnumerable())
      {
        HashSet<object> storage = this._storage;
        if (!(jsValue1.Value is JSValue jsValue2))
          jsValue2 = jsValue1;
        storage.Add((object) jsValue2);
      }
    }

    public Set add(object item)
    {
      this._storage.Add(item);
      return this;
    }

    public void clear() => this._storage.Clear();

    public bool delete(object key) => this._storage.Remove(key);

    public bool has(object key) => this._storage.Contains(key);

    public void forEach(Function callback, JSValue thisArg)
    {
      foreach (object obj in this._storage)
      {
        Arguments arguments = new Arguments()
        {
          obj,
          (JSValue) null,
          (object) this
        };
        arguments[1] = arguments[0];
        callback.Call(thisArg, arguments);
      }
    }

    public IIterator keys() => this._storage.AsIterable().iterator();

    public IIterator values() => this._storage.AsIterable().iterator();

    public IIterator iterator() => this._storage.GetEnumerator().AsIterator();
  }
}
