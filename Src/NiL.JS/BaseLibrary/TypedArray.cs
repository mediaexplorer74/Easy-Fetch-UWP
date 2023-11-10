// Decompiled with JetBrains decompiler
// Type: NiL.JS.BaseLibrary.TypedArray
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Core.Interop;
using System;
using System.Collections.Generic;

namespace NiL.JS.BaseLibrary
{
  [Prototype(typeof (Array))]
  public abstract class TypedArray : JSObject
  {
    protected abstract JSValue this[int index] { get; set; }

    [Field]
    [DoNotEnumerate]
    public ArrayBuffer buffer { [Hidden] get; private set; }

    [Field]
    [DoNotEnumerate]
    public int byteLength { [Hidden] get; private set; }

    [Field]
    [DoNotEnumerate]
    public int byteOffset { [Hidden] get; private set; }

    [Field]
    [DoNotEnumerate]
    public Number length { [Hidden] get; private set; }

    [Field]
    [DoNotEnumerate]
    public abstract int BYTES_PER_ELEMENT { [Hidden] get; }

    [Hidden]
    public abstract Type ElementType { [Hidden] get; }

    protected TypedArray()
    {
      this.buffer = new ArrayBuffer();
      this._valueType = JSValueType.Object;
      this._oValue = (object) this;
      this.length = (Number) 0;
    }

    [DoNotEnumerate]
    protected TypedArray(int length)
    {
      this.length = new Number(length);
      this.buffer = new ArrayBuffer(length * this.BYTES_PER_ELEMENT);
      this.byteLength = length * this.BYTES_PER_ELEMENT;
      this.byteOffset = 0;
      this._valueType = JSValueType.Object;
      this._oValue = (object) this;
    }

    [DoNotEnumerate]
    protected TypedArray(ArrayBuffer buffer, int byteOffset, int length)
    {
      if (byteOffset % this.BYTES_PER_ELEMENT != 0)
        ExceptionHelper.Throw((Error) new RangeError("Offset is not alligned"));
      if (buffer.byteLength % this.BYTES_PER_ELEMENT != 0)
        ExceptionHelper.Throw((Error) new RangeError("buffer.byteLength is not alligned"));
      if (buffer.byteLength < byteOffset)
        ExceptionHelper.Throw((Error) new RangeError("Invalid offset"));
      this.byteLength = System.Math.Min(buffer.byteLength - byteOffset, length * this.BYTES_PER_ELEMENT);
      this.buffer = buffer;
      this.length = new Number(this.byteLength / this.BYTES_PER_ELEMENT);
      this.byteOffset = byteOffset;
      this._valueType = JSValueType.Object;
      this._oValue = (object) this;
    }

    [DoNotEnumerate]
    protected TypedArray(JSValue iterablyObject)
    {
      Array array = Tools.arraylikeToArray(iterablyObject, true, false, false, -1L);
      if (array._data.Length > (uint) int.MaxValue)
        throw new OutOfMemoryException();
      int length = (int) array._data.Length;
      this.buffer = new ArrayBuffer(length * this.BYTES_PER_ELEMENT);
      this.length = new Number(length);
      this.byteLength = length * this.BYTES_PER_ELEMENT;
      this._valueType = JSValueType.Object;
      this._oValue = (object) this;
      foreach (KeyValuePair<int, JSValue> keyValuePair in array._data.ReversOrder)
        this[keyValuePair.Key] = keyValuePair.Value;
    }

    [AllowNullArguments]
    [ArgumentsCount(2)]
    public void set(Arguments args)
    {
      if (args == null)
        return;
      long int64_1 = Tools.JSObjectToInt64(args[1], 0L, false);
      JSValue targetObject = args[0] ?? JSValue.undefined;
      if (targetObject._valueType < JSValueType.String)
        return;
      long int64_2 = Tools.JSObjectToInt64(targetObject["length"], 0L, false);
      if ((long) this.length._iValue - int64_1 < int64_2)
        ExceptionHelper.Throw((Error) new RangeError("Invalid source length or offset argument"));
      JSValue key = (JSValue) 0;
      Arguments arguments = new Arguments();
      for (long index = 0; index < int64_2; ++index)
      {
        if (index > (long) int.MaxValue)
        {
          key._valueType = JSValueType.Double;
          key._dValue = (double) index;
        }
        else
          key._iValue = (int) index;
        JSValue jsValue = targetObject.GetProperty(key, false, PropertyScope.Common);
        if (jsValue._valueType == JSValueType.Property)
        {
          jsValue = ((jsValue._oValue as PropertyPair).getter ?? Function.Empty).Call(targetObject, arguments);
          arguments.Reset();
        }
        this[(int) (index + int64_1)] = jsValue;
      }
    }

    [ArgumentsCount(2)]
    public abstract TypedArray subarray(Arguments args);

    protected T subarrayImpl<T>(JSValue begin, JSValue end) where T : TypedArray, new()
    {
      int int32 = Tools.JSObjectToInt32(begin, 0, false);
      int val1 = end.Exists ? Tools.JSObjectToInt32(end, 0, false) : Tools.JSObjectToInt32((JSValue) this.length);
      if (int32 == 0 && val1 >= this.length._iValue)
        return (T) this;
      T obj = new T();
      obj.buffer = this.buffer;
      obj.byteLength = System.Math.Max(0, System.Math.Min(val1, this.length._iValue) - int32) * this.BYTES_PER_ELEMENT;
      obj.byteOffset = this.byteOffset + int32 * this.BYTES_PER_ELEMENT;
      obj.length = new Number(obj.byteLength / this.BYTES_PER_ELEMENT);
      return obj;
    }

    protected internal override sealed JSValue GetProperty(
      JSValue key,
      bool forWrite,
      PropertyScope memberScope)
    {
      if (memberScope < PropertyScope.Super && key._valueType != JSValueType.Symbol)
      {
        if (key._valueType == JSValueType.String && "length".Equals(key._oValue))
          return (JSValue) this.length;
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
            char ch = jsValue._oValue.ToString()[0];
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
        if (flag)
        {
          if (index1 < 0)
            ExceptionHelper.Throw((Error) new RangeError("Invalid array index"));
          return index1 >= this.length._iValue ? JSValue.undefined : this[index1];
        }
      }
      return base.GetProperty(key, forWrite, memberScope);
    }

    protected internal override void SetProperty(
      JSValue name,
      JSValue value,
      PropertyScope memberScope,
      bool strict)
    {
      if (name._valueType == JSValueType.String && "length".Equals(name._oValue))
        return;
      bool flag = false;
      int index1 = 0;
      JSValue jsValue = name;
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
          char ch = jsValue._oValue.ToString()[0];
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
      if (flag)
      {
        if (index1 < 0)
          ExceptionHelper.Throw((Error) new RangeError("Invalid array index"));
        if (index1 >= this.length._iValue)
          return;
        this[index1] = value;
      }
      else
        this.SetProperty(name, value, strict);
    }

    protected internal abstract System.Array ToNativeArray();

    protected internal override IEnumerator<KeyValuePair<string, JSValue>> GetEnumerator(
      bool hideNonEnum,
      EnumerationMode enumeratorMode)
    {
      IEnumerator<KeyValuePair<string, JSValue>> baseEnum = base.GetEnumerator(hideNonEnum, enumeratorMode);
      while (baseEnum.MoveNext())
        yield return baseEnum.Current;
      int i = 0;
      for (int len = Tools.JSObjectToInt32((JSValue) this.length); i < len; ++i)
        yield return new KeyValuePair<string, JSValue>(i.ToString(), this[i]);
    }

    protected sealed class Element : JSValue
    {
      private readonly TypedArray owner;
      private int index;

      public Element(TypedArray owner, int index)
      {
        this.owner = owner;
        this.index = index;
        this._attributes |= JSValueAttributesInternal.Reassign;
      }

      public override void Assign(JSValue value) => this.owner[this.index] = value;
    }
  }
}
