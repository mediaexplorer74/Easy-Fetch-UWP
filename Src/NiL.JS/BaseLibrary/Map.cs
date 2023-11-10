// Decompiled with JetBrains decompiler
// Type: NiL.JS.BaseLibrary.Map
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Core.Interop;
using NiL.JS.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NiL.JS.BaseLibrary
{
  [RequireNewKeyword]
  public sealed class Map : IIterable
  {
    private Dictionary<object, object> _storage;

    public int size => this._storage.Count;

    public Map() => this._storage = new Dictionary<object, object>();

    public Map(IIterable iterable)
      : this()
    {
      if (iterable == null)
        return;
      foreach (JSValue jsValue1 in iterable.AsEnumerable())
      {
        if (jsValue1._valueType < JSValueType.Object)
          ExceptionHelper.ThrowTypeError(string.Format("Iterator value {0} is not an entry object", (object) jsValue1));
        JSValue jsValue2 = jsValue1["1"];
        Dictionary<object, object> storage = this._storage;
        object key = jsValue1["0"].Value;
        if (!(jsValue2.Value is JSValue jsValue3))
          jsValue3 = jsValue2;
        storage[key] = (object) jsValue3;
      }
    }

    public object get(object key)
    {
      key = key != null ? (key is JSValue jsValue ? jsValue.Value : (object) null) ?? key : (object) JSValue.@null;
      object obj = (object) null;
      this._storage.TryGetValue(key, out obj);
      return obj;
    }

    public Map set(object key, object value)
    {
      key = key != null ? (key is JSValue jsValue ? jsValue.Value : (object) null) ?? key : (object) JSValue.@null;
      this._storage[key] = value;
      return this;
    }

    public void clear() => this._storage.Clear();

    public bool delete(object key)
    {
      key = key != null ? (key is JSValue jsValue ? jsValue.Value : (object) null) ?? key : (object) JSValue.@null;
      return this._storage.Remove(key);
    }

    public bool has(object key)
    {
      key = key != null ? (key is JSValue jsValue ? jsValue.Value : (object) null) ?? key : (object) JSValue.@null;
      return this._storage.ContainsKey(key);
    }

    public void forEach(Function callback, JSValue thisArg)
    {
      foreach (KeyValuePair<object, object> keyValuePair in this._storage)
        callback.Call(thisArg, new Arguments()
        {
          keyValuePair.Key,
          keyValuePair.Value,
          (object) this
        });
    }

    public IIterator keys() => this._storage.Keys.AsIterable().iterator();

    public IIterator values() => this._storage.Keys.AsIterable().iterator();

    public IIterator iterator() => this._storage.Select<KeyValuePair<object, object>, Array>((Func<KeyValuePair<object, object>, Array>) (x => new Array()
    {
      JSValue.Marshal(x.Key),
      JSValue.Marshal(x.Value)
    })).GetEnumerator().AsIterator();
  }
}
