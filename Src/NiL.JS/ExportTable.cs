// Decompiled with JetBrains decompiler
// Type: NiL.JS.ExportTable
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using System;
using System.Collections;
using System.Collections.Generic;

namespace NiL.JS
{
  public sealed class ExportTable : IEnumerable<KeyValuePair<string, JSValue>>, IEnumerable
  {
    private Dictionary<string, JSValue> _items = new Dictionary<string, JSValue>();

    public JSValue this[string key]
    {
      get
      {
        if (string.IsNullOrWhiteSpace(key) || !Parser.ValidateName(key, 0, false, true, false))
          ExceptionHelper.Throw((Exception) new ArgumentException());
        JSValue undefined = JSValue.undefined;
        return !this._items.TryGetValue(key, out undefined) ? JSValue.undefined : undefined;
      }
      internal set => this._items[key] = value;
    }

    public int Count => this._items.Count;

    public JSValue Default
    {
      get
      {
        JSValue undefined = JSValue.undefined;
        return !this._items.TryGetValue("", out undefined) ? JSValue.undefined : undefined;
      }
    }

    public JSObject CreateExportList()
    {
      JSObject exportList = JSObject.CreateObject(true);
      foreach (KeyValuePair<string, JSValue> keyValuePair in this._items)
      {
        if (keyValuePair.Key != "")
          exportList._fields[keyValuePair.Key] = keyValuePair.Value;
        else
          exportList._fields["default"] = keyValuePair.Value;
      }
      return exportList;
    }

    public IEnumerator<KeyValuePair<string, JSValue>> GetEnumerator() => (IEnumerator<KeyValuePair<string, JSValue>>) this._items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
