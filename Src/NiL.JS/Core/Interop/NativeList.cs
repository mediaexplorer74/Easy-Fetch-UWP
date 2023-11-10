// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Interop.NativeList
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core.Functions;
using NiL.JS.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace NiL.JS.Core.Interop
{
  [Prototype(typeof (NiL.JS.BaseLibrary.Array))]
  public sealed class NativeList : CustomType, IIterable
  {
    private readonly Number _lenObj;
    private readonly IList _data;
    private readonly Type _elementType;

    public override object Value
    {
      get => (object) this._data;
      protected set
      {
      }
    }

    [Hidden]
    public NativeList()
    {
      this._data = (IList) new List<object>();
      this._elementType = typeof (object);
      this._lenObj = new Number(0);
    }

    [Hidden]
    public NativeList(IList data)
    {
      this._data = data;
      this._elementType = data.GetType().GetElementType();
      if ((object) this._elementType == null)
      {
        Type type = NiL.JS.Backward.Backward.GetInterface(data.GetType(), typeof (IList<>).Name);
        this._elementType = (object) type == null ? typeof (object) : TypeExtensions.GetGenericArguments(type)[0];
      }
      this._lenObj = new Number(data.Count);
    }

    public void push(Arguments args)
    {
      for (int index = 0; index < args._iValue; ++index)
        this._data.Add(Tools.convertJStoObj(args[index], this._elementType, true));
    }

    public JSValue pop()
    {
      if (this._data.Count == 0)
      {
        JSValue.notExists._valueType = JSValueType.NotExistsInObject;
        return JSValue.notExists;
      }
      object data = this._data[this._data.Count - 1];
      this._data.RemoveAt(this._data.Count - 1);
      return data is IList ? (JSValue) new NativeList(data as IList) : Context.CurrentGlobalContext.ProxyValue(data);
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
          this._lenObj._iValue = this._data.Count;
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
        if (flag && index1 >= 0 && index1 < this._data.Count)
          return (JSValue) new NativeList.Element(this, index1);
      }
      return base.GetProperty(key, forWrite, memberScope);
    }

    protected internal override void SetProperty(
      JSValue key,
      JSValue value,
      PropertyScope memberScope,
      bool strict)
    {
      if (key._valueType != JSValueType.Symbol)
      {
        if (key._valueType == JSValueType.String && string.CompareOrdinal("length", key._oValue.ToString()) == 0)
          return;
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
                double num = 0.0;
                int index2 = 0;
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
        if (flag)
        {
          JSValue.notExists._valueType = JSValueType.NotExistsInObject;
          if (index1 < 0 || index1 > this._data.Count)
            return;
          this._data[index1] = value.Value;
          return;
        }
      }
      this.SetProperty(key, value, strict);
    }

    public NiL.JS.BaseLibrary.Array toJSON()
    {
      GlobalContext currentGlobalContext = Context.CurrentGlobalContext;
      NiL.JS.BaseLibrary.Array json = new NiL.JS.BaseLibrary.Array();
      for (int index = 0; index < this._data.Count; ++index)
        json[index] = currentGlobalContext.ProxyValue(this._data[index]);
      return json;
    }

    protected internal override IEnumerator<KeyValuePair<string, JSValue>> GetEnumerator(
      bool hideNonEnumerable,
      EnumerationMode enumerationMode)
    {
      NativeList owner = this;
      for (int i = 0; i < owner._data.Count; ++i)
        yield return new KeyValuePair<string, JSValue>(Tools.Int32ToString(i), enumerationMode > EnumerationMode.KeysOnly ? (JSValue) new NativeList.Element(owner, i) : (JSValue) null);
      // ISSUE: reference to a compiler-generated method
      IEnumerator<KeyValuePair<string, JSValue>> e = owner.\u003C\u003En__0(hideNonEnumerable, enumerationMode);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator<KeyValuePair<string, JSValue>>) null;
    }

    public IIterator iterator() => this._data.GetEnumerator().AsIterator();

    private sealed class Element : JSValue
    {
      private readonly NativeList _owner;
      private int _index;

      public Element(NativeList owner, int index)
      {
        this._owner = owner;
        this._index = index;
        this._attributes |= JSValueAttributesInternal.Reassign;
        object del = owner._data[index];
        this._valueType = JSValueType.Undefined;
        if (del == null)
          return;
        if (del is JSValue)
        {
          base.Assign(del as JSValue);
        }
        else
        {
          switch (del.GetType().GetTypeCode())
          {
            case TypeCode.Boolean:
              this._iValue = (bool) del ? 1 : 0;
              this._valueType = JSValueType.Boolean;
              break;
            case TypeCode.Char:
              this._oValue = (object) ((char) del).ToString();
              this._valueType = JSValueType.String;
              break;
            case TypeCode.SByte:
              this._iValue = (int) (sbyte) del;
              this._valueType = JSValueType.Integer;
              break;
            case TypeCode.Byte:
              this._iValue = (int) (byte) del;
              this._valueType = JSValueType.Integer;
              break;
            case TypeCode.Int16:
              this._iValue = (int) (short) del;
              this._valueType = JSValueType.Integer;
              break;
            case TypeCode.UInt16:
              this._iValue = (int) (ushort) del;
              this._valueType = JSValueType.Integer;
              break;
            case TypeCode.Int32:
              this._iValue = (int) del;
              this._valueType = JSValueType.Integer;
              break;
            case TypeCode.UInt32:
              uint num1 = (uint) del;
              if (num1 > (uint) int.MaxValue)
              {
                this._dValue = (double) num1;
                this._valueType = JSValueType.Double;
                break;
              }
              this._iValue = (int) num1;
              this._valueType = JSValueType.Integer;
              break;
            case TypeCode.Int64:
              this._dValue = (double) (long) del;
              this._valueType = JSValueType.Double;
              break;
            case TypeCode.UInt64:
              long num2 = (long) del;
              if (num2 > (long) int.MaxValue)
              {
                this._dValue = (double) num2;
                this._valueType = JSValueType.Double;
                break;
              }
              this._iValue = (int) num2;
              this._valueType = JSValueType.Integer;
              break;
            case TypeCode.Single:
              this._dValue = (double) (float) del;
              this._valueType = JSValueType.Double;
              break;
            case TypeCode.Double:
              this._dValue = (double) del;
              this._valueType = JSValueType.Double;
              break;
            case TypeCode.Decimal:
              this._dValue = (double) (Decimal) del;
              this._valueType = JSValueType.Double;
              break;
            case TypeCode.DateTime:
              base.Assign((JSValue) new ObjectWrapper((object) new Date((DateTime) del)));
              break;
            case TypeCode.String:
              this._oValue = del;
              this._valueType = JSValueType.String;
              break;
            default:
              if ((object) (del as Delegate) != null)
              {
                this._oValue = (object) new MethodProxy((Context) Context.CurrentGlobalContext, (MethodBase) ((Delegate) del).GetMethodInfo(), ((Delegate) del).Target);
                this._valueType = JSValueType.Function;
                break;
              }
              if (del is IList data)
              {
                this._oValue = (object) new NativeList(data);
                this._valueType = JSValueType.Object;
                break;
              }
              this._oValue = (object) JSValue.Marshal(del);
              this._valueType = JSValueType.Object;
              break;
          }
        }
      }

      public override void Assign(JSValue value) => this._owner._data[this._index] = value.Value;
    }
  }
}
