// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Interop.CustomType
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using System;
using System.Collections.Generic;

namespace NiL.JS.Core.Interop
{
  public abstract class CustomType : JSObject
  {
    protected CustomType()
    {
      this._valueType = JSValueType.Object;
      this._oValue = (object) this;
      this._attributes |= JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.SystemObject;
    }

    [CLSCompliant(false)]
    [DoNotEnumerate]
    public new JSValue toString(Arguments args) => (JSValue) this.ToString();

    [Hidden]
    public override string ToString() => this._oValue != this || this._valueType < JSValueType.Object ? base.ToString() : this.GetType().ToString();

    [Hidden]
    protected internal override IEnumerator<KeyValuePair<string, JSValue>> GetEnumerator(
      bool hideNonEnum,
      EnumerationMode enumerationMode)
    {
      CustomType customType = this;
      if (customType._fields != null)
      {
        foreach (KeyValuePair<string, JSValue> field in (IEnumerable<KeyValuePair<string, JSValue>>) customType._fields)
        {
          if (field.Value.Exists && (!hideNonEnum || (field.Value._attributes & JSValueAttributesInternal.DoNotEnumerate) == JSValueAttributesInternal.None))
            yield return field;
        }
      }
    }
  }
}
