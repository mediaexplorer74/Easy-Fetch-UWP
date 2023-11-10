// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.GlobalObject
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core.Interop;
using System.Collections.Generic;

namespace NiL.JS.Core
{
  [Prototype(typeof (JSObject), true)]
  internal sealed class GlobalObject : JSObject
  {
    private Context _context;

    public GlobalObject(Context context)
    {
      this._attributes = JSValueAttributesInternal.SystemObject;
      this._context = context;
      this._fields = context._variables;
      this._valueType = JSValueType.Object;
      this._oValue = (object) this;
      this._objectPrototype = context.GlobalContext._globalPrototype;
    }

    protected internal override JSValue GetProperty(
      JSValue key,
      bool forWrite,
      PropertyScope memberScope)
    {
      return memberScope < PropertyScope.Super && key._valueType != JSValueType.Symbol ? this._context.GetVariable(key.ToString(), forWrite) : base.GetProperty(key, forWrite, memberScope);
    }

    protected internal override IEnumerator<KeyValuePair<string, JSValue>> GetEnumerator(
      bool hideNonEnumerable,
      EnumerationMode enumerationMode)
    {
      foreach (KeyValuePair<string, JSValue> variable in (IEnumerable<KeyValuePair<string, JSValue>>) this._context._variables)
      {
        if (variable.Value.Exists && (!hideNonEnumerable || (variable.Value._attributes & JSValueAttributesInternal.DoNotEnumerate) == JSValueAttributesInternal.None))
          yield return variable;
      }
      foreach (KeyValuePair<string, JSValue> variable in (IEnumerable<KeyValuePair<string, JSValue>>) this._context.GlobalContext._variables)
      {
        if (variable.Value.Exists && (!hideNonEnumerable || (variable.Value._attributes & JSValueAttributesInternal.DoNotEnumerate) == JSValueAttributesInternal.None))
          yield return variable;
      }
    }

    public override string ToString() => "[object global]";
  }
}
