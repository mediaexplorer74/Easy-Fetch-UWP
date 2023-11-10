// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Interop.ExpandoObjectWrapper
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace NiL.JS.Core.Interop
{
  internal sealed class ExpandoObjectWrapper : JSObject
  {
    private readonly ExpandoObject _target;

    public ExpandoObjectWrapper(ExpandoObject target)
    {
      this._valueType = JSValueType.Object;
      this._oValue = (object) this;
      this._target = target;
    }

    protected internal override JSValue GetProperty(
      JSValue key,
      bool forWrite,
      PropertyScope propertyScope)
    {
      if (key.ValueType == JSValueType.Symbol || propertyScope >= PropertyScope.Super)
        return base.GetProperty(key, forWrite, propertyScope);
      string key1 = key.ToString();
      IDictionary<string, object> target = (IDictionary<string, object>) this._target;
      if (forWrite)
        return (JSValue) new ExpandoObjectWrapper.ValueWrapper(this, key1);
      return !target.ContainsKey(key1) ? JSValue.undefined : JSValue.Marshal(target[key1]);
    }

    protected internal override void SetProperty(
      JSValue key,
      JSValue value,
      PropertyScope propertyScope,
      bool throwOnError)
    {
      if (key.ValueType == JSValueType.Symbol || propertyScope >= PropertyScope.Super)
        base.SetProperty(key, value, propertyScope, throwOnError);
      ((IDictionary<string, object>) this._target)[key.ToString()] = value.Value;
    }

    protected internal override bool DeleteProperty(JSValue key) => key.ValueType == JSValueType.Symbol ? base.DeleteProperty(key) : ((IDictionary<string, object>) this._target).Remove(key.ToString());

    protected internal override IEnumerator<KeyValuePair<string, JSValue>> GetEnumerator(
      bool hideNonEnum,
      EnumerationMode enumeratorMode)
    {
      switch (enumeratorMode)
      {
        case EnumerationMode.KeysOnly:
          return ((IDictionary<string, object>) this._target).Keys.Select<string, KeyValuePair<string, JSValue>>((Func<string, KeyValuePair<string, JSValue>>) (x => new KeyValuePair<string, JSValue>(x, (JSValue) null))).GetEnumerator();
        case EnumerationMode.RequireValues:
          return this._target.Select<KeyValuePair<string, object>, KeyValuePair<string, JSValue>>((Func<KeyValuePair<string, object>, KeyValuePair<string, JSValue>>) (x => new KeyValuePair<string, JSValue>(x.Key, JSValue.Marshal(x.Value)))).GetEnumerator();
        default:
          return this._target.Select<KeyValuePair<string, object>, KeyValuePair<string, JSValue>>((Func<KeyValuePair<string, object>, KeyValuePair<string, JSValue>>) (x => new KeyValuePair<string, JSValue>(x.Key, (JSValue) new ExpandoObjectWrapper.ValueWrapper(this, x.Key)))).GetEnumerator();
      }
    }

    private sealed class ValueWrapper : JSValue
    {
      private readonly string _key;
      private readonly ExpandoObjectWrapper _owner;

      public ValueWrapper(ExpandoObjectWrapper owner, string key)
      {
        this._owner = owner;
        this._key = key;
        this._attributes |= JSValueAttributesInternal.Reassign;
        object obj = (object) null;
        if (!((IDictionary<string, object>) owner._target).TryGetValue(key, out obj))
          return;
        base.Assign(JSValue.Marshal(obj));
      }

      public override void Assign(JSValue value)
      {
        this._owner.SetProperty((JSValue) this._key, value, false);
        base.Assign(value);
      }
    }
  }
}
