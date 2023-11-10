// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Interop.NativeReadOnlyList`1
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Extensions;
using System;
using System.Collections.Generic;

namespace NiL.JS.Core.Interop
{
  [Prototype(typeof (NiL.JS.BaseLibrary.Array))]
  public sealed class NativeReadOnlyList<T> : CustomType, IIterable
  {
    private readonly Number _lenObj;
    private readonly IReadOnlyList<T> _list;

    public NativeReadOnlyList(IReadOnlyList<T> list)
    {
      this._attributes |= JSValueAttributesInternal.Immutable;
      this._list = list ?? throw new ArgumentNullException(nameof (list));
      this._lenObj = (Number) 0;
    }

    protected internal override JSValue GetProperty(
      JSValue key,
      bool forWrite,
      PropertyScope memberScope)
    {
      if (memberScope < PropertyScope.Super && key._valueType != JSValueType.Symbol)
      {
        forWrite &= (this._attributes & JSValueAttributesInternal.Immutable) == JSValueAttributesInternal.None;
        if (key._valueType == JSValueType.String && string.CompareOrdinal("length", key._oValue.ToString()) == 0)
        {
          this._lenObj._iValue = this._list.Count;
          return (JSValue) this._lenObj;
        }
        bool flag = false;
        int index1 = 0;
        JSValue jsValue = key;
        if (jsValue._valueType >= JSValueType.Object)
          jsValue = jsValue.ToPrimitiveValue_String_Value();
        switch (jsValue._valueType)
        {
          case JSValueType.Integer:
            flag = jsValue._iValue >= 0;
            index1 = jsValue._iValue;
            break;
          case JSValueType.Double:
            flag = jsValue._dValue >= 0.0 && jsValue._dValue < (double) uint.MaxValue && (double) (long) jsValue._dValue == jsValue._dValue;
            if (flag)
            {
              index1 = (int) (uint) jsValue._dValue;
              break;
            }
            break;
          case JSValueType.String:
            string str = jsValue._oValue.ToString();
            if (str.Length > 0)
            {
              char ch = str[0];
              if ('0' <= ch && '9' >= ch)
              {
                int index2 = 0;
                double num;
                if (Tools.ParseNumber(jsValue._oValue.ToString(), ref index2, out num) && index2 == jsValue._oValue.ToString().Length && num >= 0.0 && num < (double) uint.MaxValue && (double) (long) num == num)
                {
                  flag = true;
                  index1 = (int) (uint) num;
                  break;
                }
                break;
              }
              break;
            }
            break;
        }
        if (flag && index1 >= 0 && index1 < this._list.Count)
          return Context.CurrentGlobalContext.ProxyValue((object) this._list[index1]);
      }
      return base.GetProperty(key, forWrite, memberScope);
    }

    public IIterator iterator() => this._list.GetEnumerator().AsIterator();
  }
}
