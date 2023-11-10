// Decompiled with JetBrains decompiler
// Type: NiL.JS.BaseLibrary.Array
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Core.Interop;
using NiL.JS.Expressions;
using NiL.JS.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NiL.JS.BaseLibrary
{
  public sealed class Array : JSObject, IIterable
  {
    private static readonly SparseArray<JSValue> emptyData = new SparseArray<JSValue>();
    [Hidden]
    internal SparseArray<JSValue> _data;
    private Array.LengthField _lengthObj;

    [DoNotEnumerate]
    public Array()
    {
      this._oValue = (object) this;
      this._valueType = JSValueType.Object;
      this._data = new SparseArray<JSValue>();
      this._attributes |= JSValueAttributesInternal.SystemObject;
    }

    [DoNotEnumerate]
    public Array(int length)
      : this((double) length)
    {
    }

    internal Array(long length)
    {
      if (length < 0L || length > (long) uint.MaxValue)
        ExceptionHelper.Throw((Error) new RangeError("Invalid array length."));
      this._oValue = (object) this;
      this._valueType = JSValueType.Object;
      this._data = new SparseArray<JSValue>((int) System.Math.Min(100000L, length));
      this._attributes |= JSValueAttributesInternal.SystemObject;
    }

    [DoNotEnumerate]
    public Array(double length)
    {
      if ((double) (long) length != length || length < 0.0 || length > (double) uint.MaxValue)
        ExceptionHelper.Throw((Error) new RangeError("Invalid array length."));
      this._oValue = (object) this;
      this._valueType = JSValueType.Object;
      this._data = new SparseArray<JSValue>();
      this._attributes |= JSValueAttributesInternal.SystemObject;
      if (length <= 0.0)
        return;
      this._data[(int) (uint) length - 1] = (JSValue) null;
    }

    internal Array(JSValue[] data)
    {
      this._oValue = (object) this;
      this._valueType = JSValueType.Object;
      this._data = new SparseArray<JSValue>(data);
      this._attributes |= JSValueAttributesInternal.SystemObject;
    }

    [DoNotEnumerate]
    public Array(Arguments args)
    {
      if (args == null)
        throw new ArgumentNullException(nameof (args));
      this._oValue = (object) this;
      this._valueType = JSValueType.Object;
      this._data = new SparseArray<JSValue>();
      this._attributes |= JSValueAttributesInternal.SystemObject;
      for (int index = 0; index < args._iValue; ++index)
        this._data[index] = args[index].CloneImpl(false);
    }

    [Hidden]
    public Array(ICollection source)
      : this((IEnumerable) source)
    {
    }

    [Hidden]
    public Array(IEnumerable source)
      : this(source == null ? (IEnumerator) null : source.GetEnumerator())
    {
    }

    [Hidden]
    internal Array(IEnumerator source)
    {
      this._oValue = (object) this;
      this._valueType = JSValueType.Object;
      if (source == null)
        throw new ArgumentNullException("enumerator");
      this._data = new SparseArray<JSValue>();
      int num = 0;
      while (source.MoveNext())
      {
        object current = source.Current;
        SparseArray<JSValue> data = this._data;
        int index = num++;
        if (!(current is JSValue jsValue1))
          jsValue1 = Context.CurrentGlobalContext.ProxyValue(current);
        JSValue jsValue2 = jsValue1.CloneImpl(false);
        data[index] = jsValue2;
      }
      this._attributes |= JSValueAttributesInternal.SystemObject;
    }

    [Hidden]
    public void Add(JSValue obj) => this._data.Add(obj);

    [Hidden]
    public JSValue length
    {
      [Hidden] get
      {
        if (this._lengthObj == null)
          this._lengthObj = new Array.LengthField(this);
        if (this._data.Length <= (uint) int.MaxValue)
        {
          this._lengthObj._iValue = (int) this._data.Length;
          this._lengthObj._valueType = JSValueType.Integer;
        }
        else
        {
          this._lengthObj._dValue = (double) this._data.Length;
          this._lengthObj._valueType = JSValueType.Double;
        }
        return (JSValue) this._lengthObj;
      }
    }

    [Hidden]
    internal bool SetLenght(long nlen)
    {
      if ((long) this._data.Length == nlen)
        return true;
      if (nlen < 0L)
        ExceptionHelper.Throw((Error) new RangeError("Invalid array length"));
      if ((long) this._data.Length > nlen)
      {
        bool flag = true;
        foreach (KeyValuePair<int, JSValue> keyValuePair in this._data.ReversOrder)
        {
          if ((long) (uint) keyValuePair.Key >= nlen)
          {
            if (keyValuePair.Value != null && keyValuePair.Value.Exists && (keyValuePair.Value._attributes & JSValueAttributesInternal.DoNotDelete) != JSValueAttributesInternal.None)
            {
              nlen = (long) keyValuePair.Key;
              flag = false;
            }
          }
          else
            break;
        }
        if (!flag)
        {
          this.SetLenght(nlen + 1L);
          return false;
        }
      }
      while ((long) this._data.Length > nlen)
      {
        this._data.RemoveAt((int) this._data.Length - 1);
        this._data.Trim();
      }
      if ((long) this._data.Length != nlen)
        this._data[(int) nlen - 1] = this._data[(int) nlen - 1];
      return true;
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue concat(JSValue self, Arguments args)
    {
      Array array1;
      if (!(self?.GetProperty("length", PropertyScope.Own) ?? JSValue.undefined).Defined)
      {
        if (self._valueType < JSValueType.Object)
          self = (JSValue) self.ToObject();
        array1 = new Array() { self };
      }
      else
        array1 = Tools.arraylikeToArray(self, true, true, false, -1L);
      if (args != null)
      {
        for (int index1 = 0; index1 < args._iValue; ++index1)
        {
          JSValue jsValue1 = args[index1];
          if (jsValue1._oValue is Array oValue)
          {
            Array array2 = Tools.arraylikeToArray((JSValue) oValue, true, false, false, -1L);
            for (int index2 = 0; (long) index2 < (long) array2._data.Length; ++index2)
            {
              JSValue jsValue2 = array2._data[index2];
              array1._data.Add(jsValue2);
            }
          }
          else
            array1._data.Add(jsValue1.CloneImpl(false));
        }
      }
      return (JSValue) array1;
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(2)]
    public static JSValue copyWithin(JSValue self, Arguments args)
    {
      if (self == null || self.IsNull || self.IsUndefined())
        ExceptionHelper.ThrowTypeError("this is null or undefined");
      long lengthOfArraylike = Tools.getLengthOfArraylike(self, false);
      long num1 = Tools.JSObjectToInt64(args?[0] ?? JSValue.undefined, 0L, true);
      if (num1 < 0L)
        num1 += lengthOfArraylike;
      if (num1 < 0L)
        num1 = 0L;
      if (num1 > lengthOfArraylike)
        num1 = lengthOfArraylike;
      long num2 = Tools.JSObjectToInt64(args?[1] ?? JSValue.undefined, 0L, true);
      if (num2 < 0L)
        num2 += lengthOfArraylike;
      if (num2 < 0L)
        num2 = 0L;
      if (num2 > lengthOfArraylike)
        num2 = lengthOfArraylike;
      long num3 = Tools.JSObjectToInt64(args?[2] ?? JSValue.undefined, lengthOfArraylike, true);
      if (num3 < 0L)
        num3 += lengthOfArraylike;
      if (num3 < 0L)
        num3 = 0L;
      if (num3 > lengthOfArraylike)
        num3 = lengthOfArraylike;
      if (num2 == num1 || self._valueType < JSValueType.Object)
        return (JSValue) self.ToObject();
      int num4 = System.Math.Sign(num2 - num1);
      long num5 = System.Math.Min(num3 - num2, lengthOfArraylike - num1);
      long num6 = (num5 - 1L) * (long) (-(num4 - 1) / 2);
      if (self.Value is Array array)
      {
        long index1 = num2 + num6;
        long index2 = num1 + num6;
        for (; num5 != 0L; --num5)
        {
          array._data[(int) index2] = array._data[(int) index1];
          index1 += (long) num4;
          index2 += (long) num4;
        }
      }
      else
      {
        JSValue jsValue1 = new JSValue();
        long num7 = num2 + num6;
        long num8 = num1 + num6;
        for (; num5 != 0L; --num5)
        {
          if ((long) (int) num7 == num7)
          {
            jsValue1._iValue = (int) num7;
            jsValue1._valueType = JSValueType.Integer;
          }
          else
          {
            jsValue1._dValue = (double) (int) num7;
            jsValue1._valueType = JSValueType.Double;
          }
          JSValue jsValue2 = Tools.InvokeGetter(self.GetProperty(jsValue1, false, PropertyScope.Own), self);
          if ((long) (int) num8 == num8)
          {
            jsValue1._iValue = (int) num8;
            jsValue1._valueType = JSValueType.Integer;
          }
          else
          {
            jsValue1._dValue = (double) num8;
            jsValue1._valueType = JSValueType.Double;
          }
          if (jsValue2.Exists)
            self.SetProperty(jsValue1, jsValue2, true);
          else
            self.DeleteProperty(jsValue1);
          num7 += (long) num4;
          num8 += (long) num4;
        }
      }
      return self;
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(2)]
    public static JSValue fill(JSValue self, Arguments args)
    {
      if (self == null || self.IsNull || self.IsUndefined())
        ExceptionHelper.ThrowTypeError("this is null or undefined");
      long lengthOfArraylike = Tools.getLengthOfArraylike(self, false);
      JSValue jsValue = args?[0] ?? JSValue.undefined;
      long num1 = Tools.JSObjectToInt64(args[1], 0L, true);
      if (num1 < 0L)
        num1 += lengthOfArraylike;
      if (num1 < 0L)
        num1 = 0L;
      if (num1 > lengthOfArraylike)
        num1 = lengthOfArraylike;
      long num2 = Tools.JSObjectToInt64(args[2], lengthOfArraylike, true);
      if (num2 < 0L)
        num2 += lengthOfArraylike;
      if (num2 < 0L)
        num2 = 0L;
      if (num2 > lengthOfArraylike)
        num2 = lengthOfArraylike;
      if (self.Value is Array array)
      {
        for (long index = num1; index < num2; ++index)
          array._data[(int) index] = jsValue.CloneImpl(false);
      }
      else
      {
        JSValue name = new JSValue();
        for (long index = num1; index < num2; ++index)
        {
          if ((long) (int) index == index)
          {
            name._iValue = (int) index;
            name._valueType = JSValueType.Integer;
          }
          else
          {
            name._dValue = (double) index;
            name._valueType = JSValueType.Double;
          }
          self.SetProperty(name, jsValue, true);
        }
      }
      return self;
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue find(JSValue self, Arguments args)
    {
      if (self == null)
        self = JSValue.undefined;
      if (self._valueType < JSValueType.Object)
        self = (JSValue) self.ToObject();
      JSValue result = JSValue.undefined;
      Array.iterateImpl(self, args, JSValue.undefined, JSValue.undefined, false, (Func<JSValue, long, JSValue, ICallable, bool>) ((value, index, thisBind, jsCallback) =>
      {
        value = value.CloneImpl(false);
        if (!(bool) jsCallback.Call(thisBind, new Arguments()
        {
          value,
          (JSValue) index,
          self
        }))
          return true;
        result = value.CloneImpl(false);
        return false;
      }));
      return result;
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue findIndex(JSValue self, Arguments args)
    {
      if (self == null)
        self = JSValue.undefined;
      if (self._valueType < JSValueType.Object)
        self = (JSValue) self.ToObject();
      long result = -1;
      Array.iterateImpl(self, args, JSValue.undefined, JSValue.undefined, false, (Func<JSValue, long, JSValue, ICallable, bool>) ((value, index, thisBind, jsCallback) =>
      {
        value = value.CloneImpl(false);
        if (!(bool) jsCallback.Call(thisBind, new Arguments()
        {
          value,
          (JSValue) index,
          self
        }))
          return true;
        result = index;
        return false;
      }));
      return (JSValue) result;
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue every(JSValue self, Arguments args)
    {
      if (self == null)
        self = JSValue.undefined;
      if (self._valueType < JSValueType.Object)
        self = (JSValue) self.ToObject();
      bool result = true;
      Array.iterateImpl(self, args, JSValue.undefined, JSValue.undefined, false, (Func<JSValue, long, JSValue, ICallable, bool>) ((value, index, thisBind, jsCallback) =>
      {
        value = value.CloneImpl(false);
        int num1 = result ? 1 : 0;
        int num2 = (bool) jsCallback.Call(thisBind, new Arguments()
        {
          value,
          (JSValue) index,
          self
        }) ? 1 : 0;
        return result = (num1 & num2) != 0;
      }));
      return (JSValue) result;
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue some(JSValue self, Arguments args)
    {
      if (self == null)
        self = JSValue.undefined;
      if (self._valueType < JSValueType.Object)
        self = (JSValue) self.ToObject();
      bool result = true;
      Array.iterateImpl(self, args, JSValue.undefined, JSValue.undefined, false, (Func<JSValue, long, JSValue, ICallable, bool>) ((value, index, thisBind, jsCallback) =>
      {
        value = value.CloneImpl(false);
        int num1 = result ? 1 : 0;
        int num2 = !(bool) jsCallback.Call(thisBind, new Arguments()
        {
          value,
          (JSValue) index,
          self
        }) ? 1 : 0;
        return result = (num1 & num2) != 0;
      }));
      return (JSValue) !result;
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue filter(JSValue self, Arguments args)
    {
      if (self == null)
        self = JSValue.undefined;
      if (self._valueType < JSValueType.Object)
        self = (JSValue) self.ToObject();
      Array result = new Array();
      Array.iterateImpl(self, args, JSValue.undefined, JSValue.undefined, false, (Func<JSValue, long, JSValue, ICallable, bool>) ((value, index, thisBind, jsCallback) =>
      {
        value = value.CloneImpl(false);
        if ((bool) jsCallback.Call(thisBind, new Arguments()
        {
          value,
          (JSValue) index,
          self
        }))
          result.Add(value.CloneImpl(false));
        return true;
      }));
      return (JSValue) result;
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue map(JSValue self, Arguments args)
    {
      if (self == null)
        self = JSValue.undefined;
      if (self._valueType < JSValueType.Object)
        self = (JSValue) self.ToObject();
      Array result = new Array();
      long nlen = Array.iterateImpl(self, args, JSValue.undefined, JSValue.undefined, false, (Func<JSValue, long, JSValue, ICallable, bool>) ((value, index, thisBind, jsCallback) =>
      {
        value = value.CloneImpl(false);
        result[(int) index] = jsCallback.Call(thisBind, new Arguments()
        {
          value,
          (JSValue) index,
          self
        }).CloneImpl(false);
        return true;
      }));
      result.SetLenght(nlen);
      return (JSValue) result;
    }

    [DoNotEnumerate]
    [ArgumentsCount(1)]
    public static JSValue from(Arguments args)
    {
      JSValue arrayLike = args?[0] ?? JSValue.undefined;
      if (arrayLike == null)
        arrayLike = JSValue.undefined;
      if (arrayLike._valueType < JSValueType.Object)
        arrayLike = (JSValue) arrayLike.ToObject();
      bool simpleFunction = false;
      if (args.Length == 1)
        simpleFunction = true;
      Array result = new Array();
      Func<JSValue, long, JSValue, ICallable, bool> callback = (Func<JSValue, long, JSValue, ICallable, bool>) ((value, index, thisBind, jsCallback) =>
      {
        value = value.CloneImpl(false);
        if (simpleFunction)
          result[(int) index] = value;
        else
          result[(int) index] = jsCallback.Call(thisBind, new Arguments()
          {
            value,
            (JSValue) index,
            arrayLike
          }).CloneImpl(false);
        return true;
      });
      if (arrayLike.IsIterable())
      {
        int nlen = 0;
        foreach (JSValue jsValue in IterationProtocolExtensions.AsIterable(arrayLike).AsEnumerable())
        {
          int num = callback(jsValue, (long) nlen++, args[2], simpleFunction ? (ICallable) Function.Empty : args[1].As<ICallable>()) ? 1 : 0;
        }
        result.SetLenght((long) nlen);
      }
      else
      {
        Arguments args1 = new Arguments();
        for (int index = 1; index < args.Length; ++index)
          args1.Add(args[index]);
        if (simpleFunction)
          args1.Add((JSValue) Function.Empty);
        long nlen = Array.iterateImpl(arrayLike, args1, JSValue.undefined, JSValue.undefined, false, callback);
        result.SetLenght(nlen);
      }
      return (JSValue) result;
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue forEach(JSValue self, Arguments args)
    {
      if (self == null)
        self = JSValue.undefined;
      if (self._valueType < JSValueType.Object)
        self = (JSValue) self.ToObject();
      Array.iterateImpl(self, args, JSValue.undefined, JSValue.undefined, false, (Func<JSValue, long, JSValue, ICallable, bool>) ((value, index, thisBind, jsCallback) =>
      {
        value = value.CloneImpl(false);
        jsCallback.Call(thisBind, new Arguments()
        {
          value,
          (JSValue) index,
          self
        });
        return true;
      }));
      return (JSValue) null;
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue indexOf(JSValue self, Arguments args)
    {
      if (self == null)
        self = JSValue.undefined;
      long result = -1;
      Array.iterateImpl(self, (Arguments) null, args?[1] ?? JSValue.undefined, JSValue.undefined, false, (Func<JSValue, long, JSValue, ICallable, bool>) ((value, index, thisBind, jsCallback) =>
      {
        if (!StrictEqual.Check(args[0], value))
          return true;
        result = index;
        return false;
      }));
      return (JSValue) result;
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue includes(JSValue self, Arguments args)
    {
      long result = -1;
      Array.iterateImpl(self, (Arguments) null, args?[1] ?? JSValue.undefined, JSValue.undefined, false, (Func<JSValue, long, JSValue, ICallable, bool>) ((value, index, thisBind, jsCallback) =>
      {
        if ((args[0].IsNaN() ? (value.IsNaN() ? 1 : 0) : (StrictEqual.Check(args[0], value) ? 1 : 0)) == 0)
          return true;
        result = index;
        return false;
      }));
      return (JSValue) (result != -1L);
    }

    private static long iterateImpl(
      JSValue self,
      Arguments args,
      JSValue startIndexSrc,
      JSValue endIndexSrc,
      bool processMissing,
      Func<JSValue, long, JSValue, ICallable, bool> callback)
    {
      Array oValue = self._oValue as Array;
      bool flag1 = oValue != null;
      if (!self.Defined || self._valueType >= JSValueType.Object && self._oValue == null)
        ExceptionHelper.Throw((Error) new TypeError("Trying to call method for null or undefined"));
      long num1 = flag1 ? (long) oValue._data.Length : Tools.getLengthOfArraylike(self, false);
      long num2 = 0;
      ICallable callable = (ICallable) null;
      JSValue jsValue1 = (JSValue) null;
      if (args != null)
      {
        callable = args[0] == null ? (ICallable) null : args[0]._oValue as ICallable;
        if (callable == null)
          ExceptionHelper.Throw((Error) new TypeError("Callback is not a function."));
        jsValue1 = args.Length > 1 ? args[1] : (JSValue) null;
      }
      else if (startIndexSrc.Exists)
      {
        num2 = Tools.JSObjectToInt64(startIndexSrc, 0L, true);
        if (num2 > num1)
          num2 = num1;
        if (num2 < 0L)
          num2 += num1;
        if (num2 < 0L)
          num2 = 0L;
        long num3 = Tools.JSObjectToInt64(endIndexSrc, long.MaxValue, true);
        if (num3 > num1)
          num3 = num1;
        if (num3 < 0L)
          num3 += num1;
        if (num3 < 0L)
          num3 = 0L;
        if (num1 > num3)
          num1 = num3;
      }
      if (num1 > 0L)
      {
        if (!flag1)
        {
          long num4 = num2 - 1L;
          for (JSValue jsValue2 = self; jsValue2 != null && !jsValue2.IsNull && jsValue2.Defined; jsValue2 = (JSValue) jsValue2.__proto__)
          {
            IEnumerator<KeyValuePair<string, JSValue>> enumerator = jsValue2.GetEnumerator(false, EnumerationMode.RequireValues);
            while (enumerator.MoveNext())
            {
              KeyValuePair<string, JSValue> current = enumerator.Current;
              long result;
              if (long.TryParse(current.Key, out result))
              {
                if (result >= num1)
                {
                  num4 = result;
                  break;
                }
                if (result - num4 > 1L)
                {
                  for (long index = num4 + 1L; index < result; ++index)
                  {
                    JSValue key = new JSValue();
                    if (index <= (long) int.MaxValue)
                    {
                      key._iValue = (int) index;
                      key._valueType = JSValueType.Integer;
                    }
                    else
                    {
                      key._dValue = (double) index;
                      key._valueType = JSValueType.Double;
                    }
                    JSValue property = jsValue2.GetProperty(key, false, PropertyScope.Common);
                    if ((processMissing || property.Exists) && !callback(Tools.InvokeGetter(property, self), index, jsValue1, callable))
                      return num1;
                  }
                }
                else if (result <= num4)
                  continue;
                if (!callback(Tools.InvokeGetter(current.Value, self), result, jsValue1, callable))
                  return num1;
                num4 = result;
              }
              else
                break;
            }
            if (num1 - num4 <= 1L)
              break;
          }
        }
        else
        {
          JSValue key = new JSValue();
          long num5 = num2 - 1L;
          IEnumerator<KeyValuePair<int, JSValue>> enumerator = oValue._data.DirectOrder.GetEnumerator();
          bool flag2 = true;
          while (flag2)
          {
            KeyValuePair<int, JSValue> current1;
            for (flag2 = enumerator.MoveNext(); flag2; flag2 = enumerator.MoveNext())
            {
              current1 = enumerator.Current;
              if (current1.Value == null)
                continue;
              current1 = enumerator.Current;
              if (current1.Value.Exists)
                break;
            }
            KeyValuePair<int, JSValue> current2 = enumerator.Current;
            long num6 = (long) (uint) current2.Key;
            if (!flag2)
            {
              current1 = enumerator.Current;
              if (current1.Value != null)
              {
                current1 = enumerator.Current;
                if (current1.Value.Exists)
                  goto label_55;
              }
              num6 = num1;
            }
label_55:
            JSValue property1 = current2.Value;
            if (num6 - num5 > 1L || !flag2 && num6 < num1)
            {
              if (!flag2)
                num6 = num1;
              for (long index = num5 + 1L; index < num6; ++index)
              {
                if (index <= (long) int.MaxValue)
                {
                  key._iValue = (int) index;
                  key._valueType = JSValueType.Integer;
                }
                else
                {
                  key._dValue = (double) index;
                  key._valueType = JSValueType.Double;
                }
                JSValue property2 = self.GetProperty(key, false, PropertyScope.Common);
                if ((processMissing || property2.Exists) && !callback(Tools.InvokeGetter(property2, self), index, jsValue1, callable))
                  return num1;
              }
              property1 = current2.Value;
            }
            if (num6 > num5)
            {
              num5 = num6;
              if (num6 < num1 && flag2)
              {
                if (property1 != null && property1.Exists)
                {
                  JSValue jsValue3 = Tools.InvokeGetter(property1, self);
                  if (!callback(jsValue3, num6, jsValue1, callable))
                    return num1;
                }
              }
              else
                break;
            }
          }
        }
      }
      return num1;
    }

    private static long reverseIterateImpl(
      JSValue self,
      Arguments args,
      JSValue startIndexSrc,
      Func<JSValue, long, JSValue, Function, bool> callback)
    {
      Array oValue = self._oValue as Array;
      int num1 = oValue != null ? 1 : 0;
      if (!self.Defined || self._valueType >= JSValueType.Object && self._oValue == null)
        ExceptionHelper.Throw((Error) new TypeError("Trying to call method for for null or undefined"));
      long num2 = num1 != 0 ? (long) oValue._data.Length : Tools.getLengthOfArraylike(self, false);
      long num3 = num2 - 1L;
      Function function = (Function) null;
      JSValue jsValue1 = (JSValue) null;
      if (args != null)
      {
        function = args[0] == null ? (Function) null : args[0]._oValue as Function;
        if (function == null)
          ExceptionHelper.Throw((Error) new TypeError("Callback is not a function."));
        jsValue1 = args.Length > 1 ? args[1] : (JSValue) null;
      }
      else if (startIndexSrc.Exists)
      {
        num3 = Tools.JSObjectToInt64(startIndexSrc, 0L, true);
        if (num3 > num2)
          num3 = num2;
        if (num3 < 0L)
          num3 += num2;
        if (num3 < 0L)
          num3 = -1L;
      }
      if (num1 == 0)
      {
        for (long index = num3; index >= 0L; --index)
        {
          JSValue key = new JSValue();
          if (index <= (long) int.MaxValue)
          {
            key._iValue = (int) index;
            key._valueType = JSValueType.Integer;
          }
          else
          {
            key._dValue = (double) index;
            key._valueType = JSValueType.Double;
          }
          JSValue property = self.GetProperty(key, false, PropertyScope.Common);
          if (property.Exists && !callback(Tools.InvokeGetter(property, self), index, jsValue1, function))
            return num2;
        }
      }
      else
      {
        long num4 = num3 + 1L;
        IEnumerator<KeyValuePair<int, JSValue>> enumerator = oValue._data.ReversOrder.GetEnumerator();
        bool flag = true;
        while (flag)
        {
          KeyValuePair<int, JSValue> current1;
          for (flag = enumerator.MoveNext(); flag; flag = enumerator.MoveNext())
          {
            current1 = enumerator.Current;
            if ((long) (uint) current1.Key > num3)
              continue;
            current1 = enumerator.Current;
            if (current1.Value != null)
            {
              current1 = enumerator.Current;
              if (current1.Value.Exists)
                break;
            }
          }
          KeyValuePair<int, JSValue> current2 = enumerator.Current;
          long num5 = (long) (uint) current2.Key;
          if (!flag)
          {
            current1 = enumerator.Current;
            if (current1.Value != null)
            {
              current1 = enumerator.Current;
              if (current1.Value.Exists)
                goto label_33;
            }
            num5 = 0L;
          }
label_33:
          JSValue property1 = current2.Value;
          if (num4 - num5 > 1L || !flag && num4 > 0L)
          {
            if (!flag)
              num5 = -1L;
            for (long index = num4 - 1L; index > num5; --index)
            {
              JSValue key = new JSValue();
              if (index <= (long) int.MaxValue)
              {
                key._iValue = (int) index;
                key._valueType = JSValueType.Integer;
              }
              else
              {
                key._dValue = (double) index;
                key._valueType = JSValueType.Double;
              }
              JSValue property2 = self.GetProperty(key, false, PropertyScope.Common);
              if (property2.Exists && !callback(Tools.InvokeGetter(property2, self), index, jsValue1, function))
                return num2;
            }
            property1 = current2.Value;
          }
          num4 = num5;
          if (num5 < num2 && flag)
          {
            if (property1 != null && property1.Exists)
            {
              JSValue jsValue2 = Tools.InvokeGetter(property1, self);
              if (!callback(jsValue2, num5, jsValue1, function))
                return num2;
            }
          }
          else
            break;
        }
      }
      return num2;
    }

    [DoNotEnumerate]
    public static JSValue isArray(Arguments args)
    {
      if (args != null)
        return (JSValue) (args[0].Value is Array || args[0].Value == Context.CurrentGlobalContext.GetPrototype(typeof (Array)));
      ExceptionHelper.ThrowArgumentNull(nameof (args));
      return (JSValue) null;
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue join(JSValue self, Arguments args)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("Array.prototype.join called for null or undefined"));
      return (JSValue) Array.joinImpl(self, args == null || args._iValue == 0 || !args[0].Defined ? "," : args[0].ToString(), false);
    }

    private static string joinImpl(JSValue self, string separator, bool locale)
    {
      if (!(self.Value is Array array1))
        array1 = Tools.arraylikeToArray(self, true, false, false, -1L);
      Array array2 = array1;
      SparseArray<JSValue> data = array2._data;
      if (data == null || data.Length == 0U)
        return "";
      if ((long) (data.Length - 1U) * (long) separator.Length > (long) int.MaxValue)
        ExceptionHelper.Throw((Error) new RangeError("The array is too big"));
      array2._data = Array.emptyData;
      StringBuilder stringBuilder = new StringBuilder((int) ((long) (data.Length - 1U) * (long) separator.Length));
      JSValue key = (JSValue) 0;
      for (long index1 = 0; index1 < (long) data.Length; ++index1)
      {
        if (index1 > 0L)
          stringBuilder.Append(separator);
        int index2 = (int) index1;
        JSValue property = data[index2];
        if (property == null || !property.Exists)
        {
          if (index1 <= (long) int.MaxValue)
          {
            key._iValue = index2;
            key._valueType = JSValueType.Integer;
          }
          else
          {
            key._dValue = (double) index1;
            key._valueType = JSValueType.Double;
          }
          property = self.GetProperty(key, false, PropertyScope.Common);
        }
        if (property != null && property.Defined)
        {
          if (property._valueType == JSValueType.String)
            stringBuilder.Append(property.ToString());
          else if (property._valueType < JSValueType.String || property._oValue != null)
            stringBuilder.Append(locale ? (object) property.ToPrimitiveValue_LocaleString_Value() : (object) property.ToPrimitiveValue_String_Value());
        }
      }
      array2._data = data;
      return stringBuilder.ToString();
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue lastIndexOf(JSValue self, Arguments args)
    {
      long result = -1;
      Array.reverseIterateImpl(self, (Arguments) null, args?[1] ?? JSValue.undefined, (Func<JSValue, long, JSValue, Function, bool>) ((value, index, thisBind, jsCallback) =>
      {
        if (!StrictEqual.Check(args[0], value))
          return true;
        result = index;
        return false;
      }));
      return (JSValue) result;
    }

    [DoNotEnumerate]
    public static JSValue of(Arguments args) => args == null || args.Length == 0 ? (JSValue) new Array() : (JSValue) new Array(args);

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(0)]
    public static JSValue pop(JSValue self)
    {
      JSValue.notExists._valueType = JSValueType.NotExistsInObject;
      if (self is Array array)
      {
        if (array._data.Length == 0U)
          return JSValue.notExists;
        int index = (int) array._data.Length - 1;
        JSValue jsValue = array._data[index] ?? self[index.ToString()];
        if (jsValue._valueType == JSValueType.Property)
          jsValue = ((jsValue._oValue as NiL.JS.Core.PropertyPair).getter ?? Function.Empty).Call(self, (Arguments) null);
        array._data.RemoveAt(index);
        array._data[index - 1] = array._data[index - 1];
        return jsValue;
      }
      long lengthOfArraylike = Tools.getLengthOfArraylike(self, true);
      if (lengthOfArraylike <= 0L || lengthOfArraylike > (long) uint.MaxValue)
        return JSValue.notExists;
      long num = lengthOfArraylike - 1L;
      JSValue property = self.GetProperty(num.ToString(), true, PropertyScope.Common);
      JSValue jsValue1 = property._valueType != JSValueType.Property ? property.CloneImpl(false) : ((property._oValue as NiL.JS.Core.PropertyPair).getter ?? Function.Empty).Call(self, (Arguments) null);
      if ((property._attributes & JSValueAttributesInternal.DoNotDelete) == JSValueAttributesInternal.None)
      {
        property._oValue = (object) null;
        property._valueType = JSValueType.NotExistsInObject;
      }
      self["length"] = (JSValue) num;
      return jsValue1;
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue push(JSValue self, Arguments args)
    {
      JSValue.notExists._valueType = JSValueType.NotExistsInObject;
      if (self is Array array)
      {
        if (args != null)
        {
          for (int index = 0; index < args.Length; ++index)
          {
            if (array._data.Length == uint.MaxValue)
            {
              if (array._fields == null)
                array._fields = JSObject.getFieldsContainer();
              array._fields[uint.MaxValue.ToString()] = args[0].CloneImpl(false);
              ExceptionHelper.Throw((Error) new RangeError("Invalid length of array"));
            }
            array._data.Add(args[index].CloneImpl(false));
          }
        }
        return array.length;
      }
      long lengthOfArraylike = Tools.getLengthOfArraylike(self, false);
      if (args != null)
      {
        long num = lengthOfArraylike;
        lengthOfArraylike += (long) args.Length;
        self["length"] = (JSValue) lengthOfArraylike;
        int index = 0;
        while (num < lengthOfArraylike)
        {
          self[num.ToString()] = args[index].CloneImpl(false);
          ++num;
          ++index;
        }
      }
      return (JSValue) lengthOfArraylike;
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(0)]
    public static JSValue reverse(JSValue self)
    {
      Arguments arguments = (Arguments) null;
      if (self is Array array)
      {
        uint index = array._data.Length >> 1;
        while (index-- > 0U)
        {
          JSValue jsValue1 = array._data[(int) array._data.Length - 1 - (int) index];
          JSValue jsValue2 = array._data[(int) index];
          if (jsValue1 == null || !jsValue1.Exists)
            jsValue1 = array.__proto__[(array._data.Length - 1U - index).ToString()];
          JSValue jsValue3 = jsValue1._valueType != JSValueType.Property ? jsValue1 : ((jsValue1._oValue as NiL.JS.Core.PropertyPair).getter ?? Function.Empty).Call(self, (Arguments) null).CloneImpl(false);
          if (jsValue2 == null || !jsValue2.Exists)
            jsValue2 = array.__proto__[index.ToString()];
          JSValue jsValue4 = jsValue2._valueType != JSValueType.Property ? jsValue2 : ((jsValue2._oValue as NiL.JS.Core.PropertyPair).getter ?? Function.Empty).Call(self, (Arguments) null).CloneImpl(false);
          if (jsValue1._valueType == JSValueType.Property)
          {
            if (arguments == null)
              arguments = new Arguments();
            arguments._iValue = 1;
            arguments[0] = jsValue2;
            ((jsValue1._oValue as NiL.JS.Core.PropertyPair).setter ?? Function.Empty).Call(self, arguments);
          }
          else
            array._data[(int) array._data.Length - 1 - (int) index] = !jsValue4.Exists ? (JSValue) null : jsValue4;
          if (jsValue2._valueType == JSValueType.Property)
          {
            if (arguments == null)
              arguments = new Arguments();
            arguments._iValue = 1;
            arguments[0] = jsValue1;
            ((jsValue2._oValue as NiL.JS.Core.PropertyPair).setter ?? Function.Empty).Call(self, arguments);
          }
          else
            array._data[(int) index] = !jsValue3.Exists ? (JSValue) null : jsValue3;
        }
        return self;
      }
      long lengthOfArraylike = Tools.getLengthOfArraylike(self, false);
      for (int index = 0; (long) index < lengthOfArraylike >> 1; ++index)
      {
        JSValue jsValue5 = (JSValue) index.ToString();
        JSValue jsValue6 = (JSValue) (lengthOfArraylike - 1L - (long) index).ToString();
        JSValue property1 = self.GetProperty(jsValue5, false, PropertyScope.Common);
        JSValue property2 = self.GetProperty(jsValue6, false, PropertyScope.Common);
        JSValue jsValue7 = property1;
        JSValue jsValue8 = property2;
        JSValue jsValue9 = jsValue7._valueType != JSValueType.Property ? jsValue7.CloneImpl(false) : ((property1._oValue as NiL.JS.Core.PropertyPair).getter ?? Function.Empty).Call(self, (Arguments) null).CloneImpl(false);
        JSValue jsValue10 = jsValue8._valueType != JSValueType.Property ? jsValue8.CloneImpl(false) : ((property2._oValue as NiL.JS.Core.PropertyPair).getter ?? Function.Empty).Call(self, (Arguments) null).CloneImpl(false);
        if (property1._valueType == JSValueType.Property)
        {
          if (arguments == null)
            arguments = new Arguments();
          arguments._iValue = 1;
          arguments[0] = jsValue10;
          ((property1._oValue as NiL.JS.Core.PropertyPair).setter ?? Function.Empty).Call(self, arguments);
        }
        else if (jsValue10.Exists)
        {
          self.SetProperty(jsValue5, jsValue10, false);
        }
        else
        {
          JSValue property3 = self.GetProperty(jsValue5, true, PropertyScope.Own);
          if ((property3._attributes & JSValueAttributesInternal.DoNotDelete) == JSValueAttributesInternal.None)
          {
            property3._oValue = (object) null;
            property3._valueType = JSValueType.NotExists;
          }
        }
        if (property2._valueType == JSValueType.Property)
        {
          if (arguments == null)
            arguments = new Arguments();
          arguments._iValue = 1;
          arguments[0] = jsValue9;
          ((property2._oValue as NiL.JS.Core.PropertyPair).setter ?? Function.Empty).Call(self, arguments);
        }
        else if (jsValue9.Exists)
        {
          self.SetProperty(jsValue6, jsValue9, false);
        }
        else
        {
          JSValue property4 = self.GetProperty(jsValue6, true, PropertyScope.Own);
          if ((property4._attributes & JSValueAttributesInternal.DoNotDelete) == JSValueAttributesInternal.None)
          {
            property4._oValue = (object) null;
            property4._valueType = JSValueType.NotExists;
          }
        }
      }
      return self;
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue reduce(JSValue self, Arguments args)
    {
      if (self._valueType < JSValueType.Object)
        self = (JSValue) self.ToObject();
      JSValue result = JSValue.undefined;
      bool skip = true;
      if (args._iValue > 1)
      {
        skip = false;
        result = args[1];
        args[1] = (JSValue) null;
        args._iValue = 1;
      }
      if ((skip ? 0L : 1L) + Array.iterateImpl(self, args, JSValue.undefined, JSValue.undefined, false, (Func<JSValue, long, JSValue, ICallable, bool>) ((value, index, thisBind, jsCallback) =>
      {
        value = value.CloneImpl(false);
        if (!skip)
        {
          if (!result.Exists)
            result = JSValue.undefined;
          result = jsCallback.Call(thisBind, new Arguments()
          {
            result,
            value,
            (JSValue) index,
            self
          }).CloneImpl(false);
        }
        else
        {
          result = value;
          skip = false;
        }
        return true;
      })) == 0L | skip)
        ExceptionHelper.ThrowTypeError("Length of array cannot be 0");
      return result;
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue reduceRight(JSValue self, Arguments args)
    {
      if (self._valueType < JSValueType.Object)
        self = (JSValue) self.ToObject();
      JSValue result = JSValue.undefined;
      bool skip = true;
      if (args._iValue > 1)
      {
        skip = false;
        result = args[1];
        args[1] = (JSValue) null;
        args._iValue = 1;
      }
      if ((skip ? 0L : 1L) + Array.reverseIterateImpl(self, args, JSValue.undefined, (Func<JSValue, long, JSValue, Function, bool>) ((value, index, thisBind, jsCallback) =>
      {
        value = value.CloneImpl(false);
        if (!skip)
        {
          if (!result.Exists)
            result = JSValue.undefined;
          result = jsCallback.Call(thisBind, new Arguments()
          {
            result,
            value,
            (JSValue) index,
            self
          }).CloneImpl(false);
        }
        else
        {
          result = value;
          skip = false;
        }
        return true;
      })) == 0L | skip)
        ExceptionHelper.ThrowTypeError("Length of array cannot be 0");
      return result;
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(0)]
    public static JSValue shift(JSValue self)
    {
      if (self._oValue is Array oValue)
      {
        JSValue property1 = oValue._data[0];
        if (property1 == null || !property1.Exists)
          property1 = oValue.__proto__["0"];
        if (property1._valueType == JSValueType.Property)
          property1 = Tools.InvokeGetter(property1, self);
        JSValue jsValue = property1;
        uint length = oValue._data.Length;
        long num = 0;
        IEnumerator<KeyValuePair<int, JSValue>> enumerator = oValue._data.DirectOrder.GetEnumerator();
        do
        {
          KeyValuePair<int, JSValue> keyValuePair;
          do
          {
            if (enumerator.MoveNext())
              keyValuePair = enumerator.Current;
            else if (length != 0U)
              keyValuePair = new KeyValuePair<int, JSValue>((int) length, (JSValue) null);
            else
              goto label_27;
          }
          while (keyValuePair.Key == 0 || (uint) keyValuePair.Key < length - 1U && (keyValuePair.Value == null || !keyValuePair.Value.Exists));
          for (; num < (long) length && num <= (long) (uint) keyValuePair.Key; ++num)
          {
            JSValue property2;
            int index;
            if (num == (long) (uint) keyValuePair.Key && keyValuePair.Value != null && keyValuePair.Value.Exists)
            {
              property2 = keyValuePair.Value;
              index = keyValuePair.Key;
            }
            else
            {
              index = (int) num;
              property2 = oValue.__proto__[num.ToString()];
            }
            if (property2 != null && property2._valueType == JSValueType.Property)
              property2 = Tools.InvokeGetter(property2, self);
            if (jsValue != null && jsValue._valueType == JSValueType.Property)
              ((jsValue._oValue as NiL.JS.Core.PropertyPair).setter ?? Function.Empty).Call(self, new Arguments()
              {
                property2
              });
            else if (property2 != null)
            {
              if (property2.Exists)
                oValue._data[index - 1] = property2;
              if (property2._valueType != JSValueType.Property)
                oValue._data[index] = (JSValue) null;
            }
            jsValue = property2;
          }
        }
        while (num < (long) length && num >= 0L);
label_27:
        if (length == 1U)
          oValue._data.Clear();
        else if (length > 0U)
          oValue._data.RemoveAt((int) length - 1);
        return property1;
      }
      JSValue property3 = self["length"];
      if (property3._valueType == JSValueType.Property)
        property3 = Tools.InvokeGetter(property3, self);
      long num1 = (long) (uint) Tools.JSObjectToDouble(property3);
      if (num1 > (long) uint.MaxValue)
        ExceptionHelper.Throw((Error) new RangeError("Invalid array length"));
      JSValue jsValue1;
      if (num1 == 0L)
      {
        self["length"] = jsValue1 = (JSValue) num1;
        return JSValue.notExists;
      }
      JSValue key1 = new JSValue()
      {
        _valueType = JSValueType.String,
        _oValue = (object) "0"
      };
      JSValue property4 = self.GetProperty(key1, true, PropertyScope.Common);
      JSValue property5 = property4;
      JSValue jsValue2 = property5._valueType != JSValueType.Property ? property5.CloneImpl(false) : Tools.InvokeGetter(property5, self).CloneImpl(false);
      if ((property4._attributes & (JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly)) == JSValueAttributesInternal.None)
      {
        property4._oValue = (object) null;
        property4._valueType = JSValueType.NotExists;
      }
      if (num1 == 1L)
      {
        self["length"] = jsValue1 = (JSValue) (num1 - 1L);
        return jsValue2;
      }
      Array array = Tools.arraylikeToArray(self, false, true, false, -1L);
      self["length"] = jsValue1 = (JSValue) (num1 - 1L);
      List<string> stringList = new List<string>();
      foreach (KeyValuePair<string, JSValue> keyValuePair in self)
      {
        int index = 0;
        double num2 = 0.0;
        long num3;
        if (Tools.ParseNumber(keyValuePair.Key, ref index, out num2) && index == keyValuePair.Key.Length && (double) (num3 = (long) num2) == num2 && num3 < num1)
        {
          JSValue jsValue3 = keyValuePair.Value;
          if (jsValue3.Exists && jsValue3._valueType != JSValueType.Property)
            stringList.Add(keyValuePair.Key);
        }
      }
      JSValue key2 = new JSValue()
      {
        _valueType = JSValueType.String
      };
      for (int index = 0; index < stringList.Count; ++index)
      {
        key2._oValue = (object) stringList[index];
        JSValue property6 = self.GetProperty(key2, true, PropertyScope.Common);
        if ((property6._attributes & (JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly)) == JSValueAttributesInternal.None)
        {
          property6._oValue = (object) null;
          property6._valueType = JSValueType.NotExists;
        }
      }
      key2._valueType = JSValueType.Integer;
      foreach (KeyValuePair<int, JSValue> keyValuePair in array._data.DirectOrder)
      {
        if ((uint) keyValuePair.Key > (uint) int.MaxValue)
        {
          key2._valueType = JSValueType.Double;
          key2._dValue = (double) (uint) (keyValuePair.Key - 1);
        }
        else
          key2._iValue = keyValuePair.Key - 1;
        if (keyValuePair.Value != null && keyValuePair.Value.Exists)
        {
          JSValue property7 = self.GetProperty(key2, true, PropertyScope.Common);
          if (property7._valueType == JSValueType.Property)
            ((property7._oValue as NiL.JS.Core.PropertyPair).setter ?? Function.Empty).Call(self, new Arguments()
            {
              keyValuePair.Value
            });
          else
            property7.Assign(keyValuePair.Value);
        }
      }
      return jsValue2;
    }

    [DoNotEnumerate]
    [ArgumentsCount(2)]
    [InstanceMember]
    public static JSValue slice(JSValue self, Arguments args)
    {
      if (args == null)
        throw new ArgumentNullException(nameof (args));
      if (!self.Defined || self._valueType >= JSValueType.Object && self._oValue == null)
        ExceptionHelper.Throw((Error) new TypeError("Can not call Array.prototype.slice for null or undefined"));
      Array result = new Array();
      long index = 0;
      Array.iterateImpl(self, (Arguments) null, args[0], args[1], true, (Func<JSValue, long, JSValue, ICallable, bool>) ((value, itemIndex, thisBind, jsCallback) =>
      {
        if (value.Exists)
        {
          value = value.CloneImpl(false);
          if (index < 4294967294L)
            result._data[(int) index] = value;
          else
            result[index.ToString()] = value;
        }
        ++index;
        return true;
      }));
      return (JSValue) result;
    }

    [DoNotEnumerate]
    [ArgumentsCount(2)]
    [InstanceMember]
    public static JSValue splice(JSValue self, Arguments args) => Array.spliceImpl(self, args, true);

    private static JSValue spliceImpl(JSValue self, Arguments args, bool needResult)
    {
      if (args == null)
        throw new ArgumentNullException(nameof (args));
      if (args.Length == 0)
        return !needResult ? (JSValue) null : (JSValue) new Array();
      if (self is Array array1)
      {
        uint length = array1._data.Length;
        long val1 = (long) System.Math.Min(Tools.JSObjectToDouble(args[0]), (double) length);
        long num1 = args.Length <= 1 ? (long) length : (args[1]._valueType > JSValueType.Undefined ? (long) System.Math.Min(Tools.JSObjectToDouble(args[1]), (double) length) : 0L);
        if (val1 < 0L)
          val1 = (long) length + val1;
        if (val1 < 0L)
          val1 = 0L;
        if (num1 < 0L)
          num1 = 0L;
        if (num1 == 0L && args._iValue <= 2)
          return !needResult ? (JSValue) null : (JSValue) new Array();
        long num2 = (long) (uint) System.Math.Min(val1, (long) length);
        long num3 = (long) (uint) System.Math.Min(num1 + num2, (long) length);
        Array array = needResult ? new Array((int) (num3 - num2)) : (Array) null;
        long num4 = (long) System.Math.Max(0, args._iValue - 2) - (num3 - num2);
        foreach (KeyValuePair<int, JSValue> keyValuePair in num4 > 0L ? array1._data.ReversOrder : array1._data.DirectOrder)
        {
          if ((long) keyValuePair.Key >= num2)
          {
            if ((long) keyValuePair.Key >= num3)
            {
              if (num4 == 0L)
                break;
            }
            int key = keyValuePair.Key;
            JSValue property = keyValuePair.Value;
            if (property == null || !property.Exists)
            {
              JSValue jsValue = array1.__proto__[((uint) key).ToString()];
              if (jsValue.Exists)
                property = jsValue.CloneImpl(false);
              else
                continue;
            }
            if (property._valueType == JSValueType.Property)
              property = Tools.InvokeGetter(property, self).CloneImpl(false);
            if ((long) key < num3)
            {
              if (needResult)
                array._data[(int) ((long) key - num2)] = property;
            }
            else
            {
              JSValue jsValue = array1._data[(int) ((long) key + num4)];
              if (jsValue != null && jsValue._valueType == JSValueType.Property)
                ((jsValue._oValue as NiL.JS.Core.PropertyPair).setter ?? Function.Empty).Call(self, new Arguments()
                {
                  property
                });
              else
                array1._data[(int) ((long) key + num4)] = property;
              array1._data[key] = (JSValue) null;
            }
          }
        }
        if (num4 < 0L)
        {
          do
          {
            array1._data.RemoveAt((int) array1._data.Length - 1);
          }
          while (++num4 < 0L);
        }
        for (int index = 2; index < args._iValue; ++index)
        {
          if (args[index].Exists)
          {
            JSValue jsValue = array1._data[(int) (num2 + (long) index - 2L)];
            if (jsValue != null && jsValue._valueType == JSValueType.Property)
              ((jsValue._oValue as NiL.JS.Core.PropertyPair).setter ?? Function.Empty).Call(self, new Arguments()
              {
                args[index]
              });
            else
              array1._data[(int) (num2 + (long) index - 2L)] = args[index].CloneImpl(false);
          }
        }
        return (JSValue) array;
      }
      long lengthOfArraylike = Tools.getLengthOfArraylike(self, false);
      long val1_1 = (long) System.Math.Min(Tools.JSObjectToDouble(args[0]), (double) lengthOfArraylike);
      long num5 = args.Length <= 1 ? lengthOfArraylike : (args[1]._valueType > JSValueType.Undefined ? (long) System.Math.Min(Tools.JSObjectToDouble(args[1]), (double) lengthOfArraylike) : 0L);
      if (val1_1 < 0L)
        val1_1 = lengthOfArraylike + val1_1;
      if (val1_1 < 0L)
        val1_1 = 0L;
      if (num5 < 0L)
        num5 = 0L;
      if (num5 == 0L && args._iValue <= 2)
      {
        JSValue property = self.GetProperty("length", true, PropertyScope.Common);
        if (property._valueType == JSValueType.Property)
          ((property._oValue as NiL.JS.Core.PropertyPair).setter ?? Function.Empty).Call(self, new Arguments()
          {
            (JSValue) lengthOfArraylike
          });
        else
          property.Assign((JSValue) lengthOfArraylike);
        return (JSValue) new Array();
      }
      long num6 = (long) (uint) System.Math.Min(val1_1, lengthOfArraylike);
      long num7 = (long) (uint) System.Math.Min(num5 + num6, lengthOfArraylike);
      long num8 = (long) System.Math.Max(0, args._iValue - 2) - (num7 - num6);
      Array array2 = needResult ? new Array() : (Array) null;
      long num9 = -1;
      foreach (KeyValuePair<uint, JSValue> keyValuePair in Tools.EnumerateArraylike(lengthOfArraylike, self))
      {
        if (num9 == -1L)
          num9 = (long) keyValuePair.Key;
        if ((long) keyValuePair.Key - num9 > 1L && (long) keyValuePair.Key < num7)
        {
          for (long index = num9 + 1L; index < (long) keyValuePair.Key; ++index)
          {
            JSValue property = self.__proto__[index.ToString()];
            JSValue jsValue = property._valueType != JSValueType.Property ? property.CloneImpl(false) : Tools.InvokeGetter(property, self).CloneImpl(false);
            if (needResult)
              array2._data[(int) index] = jsValue.CloneImpl(false);
          }
        }
        if ((long) keyValuePair.Key < num7)
        {
          if (num6 <= (long) keyValuePair.Key)
          {
            JSValue property = keyValuePair.Value;
            JSValue jsValue = property.ValueType != JSValueType.Property ? property.CloneImpl(false) : Tools.InvokeGetter(property, self).CloneImpl(false);
            if (needResult)
              array2._data[(int) ((long) keyValuePair.Key - num6)] = jsValue;
          }
          num9 = (long) keyValuePair.Key;
        }
        else
          break;
      }
      if (num9 == -1L & needResult)
      {
        for (int index = 0; (long) index < num7 - num6; ++index)
          array2.Add(self.__proto__[((long) index + num6).ToString()].CloneImpl(false));
      }
      JSValue key1 = new JSValue();
      if (num8 > 0L)
      {
        long num10 = lengthOfArraylike;
        while (num10-- > num7)
        {
          if (num10 <= (long) int.MaxValue)
          {
            key1._valueType = JSValueType.Integer;
            key1._iValue = (int) (num10 + num8);
          }
          else
          {
            key1._valueType = JSValueType.Double;
            key1._dValue = (double) (num10 + num8);
          }
          JSValue property1 = self.GetProperty(key1, true, PropertyScope.Common);
          if (num10 + num8 <= (long) int.MaxValue)
          {
            key1._valueType = JSValueType.Integer;
            key1._iValue = (int) num10;
          }
          else
          {
            key1._valueType = JSValueType.Double;
            key1._dValue = (double) num10;
          }
          JSValue property2 = self.GetProperty(key1, true, PropertyScope.Common);
          if (property2._valueType == JSValueType.Property)
            property2 = Tools.InvokeGetter(property2, self);
          if (property1._valueType == JSValueType.Property)
            ((property1._oValue as NiL.JS.Core.PropertyPair).setter ?? Function.Empty).Call(self, new Arguments()
            {
              property2
            });
          else
            property1.Assign(property2);
        }
      }
      else if (num8 < 0L)
      {
        for (long index = num6; index < num7; ++index)
        {
          if (index + num8 <= (long) int.MaxValue)
          {
            key1._valueType = JSValueType.Integer;
            key1._iValue = (int) index;
          }
          else
          {
            key1._valueType = JSValueType.Double;
            key1._dValue = (double) index;
          }
          JSValue property = self.GetProperty(key1, true, PropertyScope.Common);
          if (index >= lengthOfArraylike + num8 && (property._attributes & JSValueAttributesInternal.DoNotDelete) == JSValueAttributesInternal.None)
          {
            property._valueType = JSValueType.NotExists;
            property._oValue = (object) null;
          }
        }
        for (long index = num7; index < lengthOfArraylike; ++index)
        {
          if (index <= (long) int.MaxValue)
          {
            key1._valueType = JSValueType.Integer;
            key1._iValue = (int) (index + num8);
          }
          else
          {
            key1._valueType = JSValueType.Double;
            key1._dValue = (double) (index + num8);
          }
          JSValue property3 = self.GetProperty(key1, true, PropertyScope.Common);
          if (index + num8 <= (long) int.MaxValue)
          {
            key1._valueType = JSValueType.Integer;
            key1._iValue = (int) index;
          }
          else
          {
            key1._valueType = JSValueType.Double;
            key1._dValue = (double) index;
          }
          JSValue property4 = self.GetProperty(key1, true, PropertyScope.Common);
          JSValue property5 = property4;
          if (property5._valueType == JSValueType.Property)
            property5 = Tools.InvokeGetter(property5, self);
          if (property3._valueType == JSValueType.Property)
            ((property3._oValue as NiL.JS.Core.PropertyPair).setter ?? Function.Empty).Call(self, new Arguments()
            {
              property5
            });
          else
            property3.Assign(property5);
          if (index >= lengthOfArraylike + num8 && (property4._attributes & JSValueAttributesInternal.DoNotDelete) == JSValueAttributesInternal.None)
          {
            property4._valueType = JSValueType.NotExists;
            property4._oValue = (object) null;
          }
        }
      }
      for (int index = 2; index < args._iValue; ++index)
      {
        if ((long) (index - 2) + num6 <= (long) int.MaxValue)
        {
          key1._valueType = JSValueType.Integer;
          key1._iValue = (int) ((long) (index - 2) + num6);
        }
        else
        {
          key1._valueType = JSValueType.Double;
          key1._dValue = (double) ((long) (index - 2) + num6);
        }
        JSValue property = self.GetProperty(key1, true, PropertyScope.Common);
        if (property._valueType == JSValueType.Property)
          ((property._oValue as NiL.JS.Core.PropertyPair).setter ?? Function.Empty).Call(self, new Arguments()
          {
            args[index]
          });
        else
          property.Assign(args[index]);
      }
      long num11 = lengthOfArraylike + num8;
      JSValue property6 = self.GetProperty("length", true, PropertyScope.Common);
      if (property6._valueType == JSValueType.Property)
        ((property6._oValue as NiL.JS.Core.PropertyPair).setter ?? Function.Empty).Call(self, new Arguments()
        {
          (JSValue) num11
        });
      else
        property6.Assign((JSValue) num11);
      return (JSValue) array2;
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue sort(JSValue self, Arguments args)
    {
      Function comparer = args != null ? args[0]._oValue as Function : throw new ArgumentNullException(nameof (args));
      if (self is Array array)
      {
        if (comparer != null)
        {
          JSValue second = new JSValue();
          JSValue first = new JSValue();
          args._iValue = 2;
          args[0] = first;
          args[1] = second;
          BinaryTree<JSValue, List<JSValue>> binaryTree = new BinaryTree<JSValue, List<JSValue>>((IComparer<JSValue>) new Array.JSComparer(args, first, second, comparer));
          uint length = array._data.Length;
          foreach (KeyValuePair<int, JSValue> keyValuePair in array._data.DirectOrder)
          {
            if (keyValuePair.Value != null && keyValuePair.Value.Defined)
            {
              JSValue key = keyValuePair.Value;
              if (key._valueType == JSValueType.Property)
                key = ((key._oValue as NiL.JS.Core.PropertyPair).getter ?? Function.Empty).Call(self, (Arguments) null).CloneImpl(false);
              List<JSValue> jsValueList = (List<JSValue>) null;
              if (!binaryTree.TryGetValue(key, out jsValueList))
                binaryTree[key] = jsValueList = new List<JSValue>();
              jsValueList.Add(keyValuePair.Value);
            }
          }
          array._data.Clear();
          foreach (BinaryTree<JSValue, List<JSValue>>.Node node in binaryTree.Nodes)
          {
            for (int index = 0; index < node.value.Count; ++index)
              array._data.Add(node.value[index]);
          }
          array._data[(int) length - 1] = array._data[(int) length - 1];
        }
        else
        {
          BinaryTree<string, List<JSValue>> binaryTree = new BinaryTree<string, List<JSValue>>((IComparer<string>) StringComparer.Ordinal);
          uint length = array._data.Length;
          foreach (KeyValuePair<int, JSValue> keyValuePair in array._data.DirectOrder)
          {
            if (keyValuePair.Value != null && keyValuePair.Value.Exists)
            {
              JSValue jsValue = keyValuePair.Value;
              if (jsValue._valueType == JSValueType.Property)
                jsValue = ((jsValue._oValue as NiL.JS.Core.PropertyPair).getter ?? Function.Empty).Call(self, (Arguments) null).CloneImpl(false);
              List<JSValue> jsValueList = (List<JSValue>) null;
              string key = jsValue.ToString();
              if (!binaryTree.TryGetValue(key, out jsValueList))
                binaryTree[key] = jsValueList = new List<JSValue>();
              jsValueList.Add(keyValuePair.Value);
            }
          }
          array._data.Clear();
          foreach (BinaryTree<string, List<JSValue>>.Node node in binaryTree.Nodes)
          {
            for (int index = 0; index < node.value.Count; ++index)
              array._data.Add(node.value[index]);
          }
          array._data[(int) length - 1] = array._data[(int) length - 1];
        }
      }
      else
      {
        long lengthOfArraylike = Tools.getLengthOfArraylike(self, false);
        if (comparer != null)
        {
          JSValue second = new JSValue();
          JSValue first = new JSValue();
          args._iValue = 2;
          args[0] = first;
          args[1] = second;
          BinaryTree<JSValue, List<JSValue>> binaryTree = new BinaryTree<JSValue, List<JSValue>>((IComparer<JSValue>) new Array.JSComparer(args, first, second, comparer));
          List<string> stringList = new List<string>();
          foreach (KeyValuePair<uint, JSValue> keyValuePair in Tools.EnumerateArraylike(lengthOfArraylike, self))
          {
            stringList.Add(keyValuePair.Key.ToString());
            JSValue jsValue1 = keyValuePair.Value;
            if (jsValue1.Defined)
            {
              JSValue jsValue2 = jsValue1.CloneImpl(false);
              JSValue key = jsValue2._valueType != JSValueType.Property ? jsValue2 : ((jsValue2._oValue as NiL.JS.Core.PropertyPair).getter ?? Function.Empty).Call(self, (Arguments) null);
              List<JSValue> jsValueList = (List<JSValue>) null;
              if (!binaryTree.TryGetValue(key, out jsValueList))
                binaryTree[key] = jsValueList = new List<JSValue>();
              jsValueList.Add(jsValue2);
            }
          }
          JSValue key1 = new JSValue()
          {
            _valueType = JSValueType.String
          };
          int count1 = stringList.Count;
          while (count1-- > 0)
          {
            key1._oValue = (object) stringList[count1];
            JSValue property = self.GetProperty(key1, true, PropertyScope.Common);
            if ((property._attributes & JSValueAttributesInternal.DoNotDelete) == JSValueAttributesInternal.None)
            {
              property._oValue = (object) null;
              property._valueType = JSValueType.NotExists;
            }
          }
          uint num = 0;
          foreach (BinaryTree<JSValue, List<JSValue>>.Node node in binaryTree.Nodes)
          {
            int count2 = node.value.Count;
            while (count2-- > 0)
              self[num++.ToString()] = node.value[count2];
          }
        }
        else
        {
          BinaryTree<string, List<JSValue>> binaryTree = new BinaryTree<string, List<JSValue>>((IComparer<string>) StringComparer.Ordinal);
          List<string> stringList = new List<string>();
          foreach (KeyValuePair<string, JSValue> keyValuePair in self)
          {
            int index = 0;
            double num = 0.0;
            if (Tools.ParseNumber(keyValuePair.Key, ref index, out num) && index == keyValuePair.Key.Length && num < (double) lengthOfArraylike)
            {
              stringList.Add(keyValuePair.Key);
              JSValue jsValue3 = keyValuePair.Value;
              if (jsValue3.Defined)
              {
                JSValue jsValue4 = jsValue3.CloneImpl(false);
                List<JSValue> jsValueList = (List<JSValue>) null;
                string key = jsValue4.ToString();
                if (!binaryTree.TryGetValue(key, out jsValueList))
                  binaryTree[key] = jsValueList = new List<JSValue>();
                jsValueList.Add(jsValue4);
              }
            }
          }
          int count3 = stringList.Count;
          while (count3-- > 0)
            self[stringList[count3]]._valueType = JSValueType.NotExists;
          uint num1 = 0;
          foreach (BinaryTree<string, List<JSValue>>.Node node in binaryTree.Nodes)
          {
            int count4 = node.value.Count;
            while (count4-- > 0)
              self[num1++.ToString()] = node.value[count4];
          }
        }
      }
      return self;
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue unshift(JSValue self, Arguments args)
    {
      int iValue = args._iValue;
      while (iValue-- > 0)
        args[iValue + 2] = args[iValue];
      args._iValue += 2;
      args[0] = (JSValue) 0;
      args[1] = args[0];
      Array.spliceImpl(self, args, false);
      return (JSValue) Tools.getLengthOfArraylike(self, false);
    }

    [Hidden]
    public override string ToString() => Array.joinImpl((JSValue) this, ",", false);

    [DoNotEnumerate]
    [CLSCompliant(false)]
    [ArgumentsCount(0)]
    public new JSValue toString(Arguments args) => (JSValue) this.ToString();

    [DoNotEnumerate]
    public new JSValue toLocaleString() => (JSValue) Array.joinImpl((JSValue) this, ",", true);

    [Hidden]
    protected internal override IEnumerator<KeyValuePair<string, JSValue>> GetEnumerator(
      bool hideNonEnum,
      EnumerationMode enumeratorMode)
    {
      Array array = this;
      foreach (KeyValuePair<int, JSValue> keyValuePair in array._data.DirectOrder)
      {
        if (keyValuePair.Value != null && keyValuePair.Value.Exists && (!hideNonEnum || (keyValuePair.Value._attributes & JSValueAttributesInternal.DoNotEnumerate) == JSValueAttributesInternal.None))
        {
          JSValue jsValue = keyValuePair.Value;
          if (enumeratorMode == EnumerationMode.RequireValuesForWrite && (jsValue._attributes & (JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.SystemObject)) == JSValueAttributesInternal.SystemObject)
            array._data[keyValuePair.Key] = jsValue = jsValue.CloneImpl(true);
          yield return new KeyValuePair<string, JSValue>(((uint) keyValuePair.Key).ToString(), jsValue);
        }
      }
      if (!hideNonEnum)
        yield return new KeyValuePair<string, JSValue>("length", array.length);
      if (array._fields != null)
      {
        foreach (KeyValuePair<string, JSValue> field in (IEnumerable<KeyValuePair<string, JSValue>>) array._fields)
        {
          if (field.Value.Exists && (!hideNonEnum || (field.Value._attributes & JSValueAttributesInternal.DoNotEnumerate) == JSValueAttributesInternal.None))
          {
            JSValue jsValue = field.Value;
            if (enumeratorMode == EnumerationMode.RequireValuesForWrite && (jsValue._attributes & (JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.SystemObject)) == JSValueAttributesInternal.SystemObject)
              array._fields[field.Key] = jsValue = jsValue.CloneImpl(true);
            yield return new KeyValuePair<string, JSValue>(field.Key, jsValue);
          }
        }
      }
    }

    [Hidden]
    public JSValue this[int index]
    {
      [Hidden] get
      {
        JSValue.notExists._valueType = JSValueType.NotExistsInObject;
        JSValue jsValue = this._data[index] ?? JSValue.notExists;
        return jsValue._valueType < JSValueType.Undefined ? this.__proto__.GetProperty((JSValue) index, false, PropertyScope.Common) : jsValue;
      }
      [Hidden] set
      {
        if ((long) index >= (long) this._data.Length && this._lengthObj != null && (this._lengthObj._attributes & JSValueAttributesInternal.ReadOnly) != JSValueAttributesInternal.None)
          return;
        JSValue jsValue = this._data[index];
        if (jsValue == null)
        {
          jsValue = new JSValue()
          {
            _valueType = JSValueType.NotExistsInObject
          };
          this._data[index] = jsValue;
        }
        else if ((jsValue._attributes & JSValueAttributesInternal.SystemObject) != JSValueAttributesInternal.None)
          this._data[index] = jsValue = jsValue.CloneImpl(false);
        if (jsValue._valueType == JSValueType.Property)
        {
          Function setter = (jsValue._oValue as NiL.JS.Core.PropertyPair).setter;
          if (setter == null)
            return;
          setter.Call((JSValue) this, new Arguments()
          {
            value
          });
        }
        else
          jsValue.Assign(value);
      }
    }

    [Hidden]
    protected internal override JSValue GetProperty(
      JSValue key,
      bool forWrite,
      PropertyScope memberScope)
    {
      if (key._valueType != JSValueType.Symbol && memberScope < PropertyScope.Super)
      {
        bool flag1 = false;
        int index1 = 0;
        bool flag2;
        do
        {
          flag2 = false;
          switch (key._valueType)
          {
            case JSValueType.Integer:
              flag1 = (key._iValue & int.MinValue) == 0;
              index1 = key._iValue;
              break;
            case JSValueType.Double:
              flag1 = key._dValue >= 0.0 && key._dValue < (double) uint.MaxValue && (double) (long) key._dValue == key._dValue;
              if (flag1)
              {
                index1 = (int) (uint) key._dValue;
                break;
              }
              break;
            case JSValueType.String:
              if (string.CompareOrdinal("length", key._oValue.ToString()) == 0)
                return this.length;
              string code = key._oValue.ToString();
              if (code.Length > 0 && '0' <= code[0] && '9' >= code[0])
              {
                int index2 = 0;
                double num;
                if (Tools.ParseNumber(code, ref index2, out num) && index2 == code.Length && num >= 0.0 && num < (double) uint.MaxValue && (double) (long) num == num)
                {
                  flag1 = true;
                  index1 = (int) (uint) num;
                  break;
                }
                break;
              }
              break;
            default:
              if (key._valueType >= JSValueType.Object)
              {
                key = key.ToPrimitiveValue_String_Value();
                object obj = key.Value;
                if (obj != null && string.CompareOrdinal("length", obj.ToString()) == 0)
                  return this.length;
                if (key.ValueType < JSValueType.Object)
                {
                  flag2 = true;
                  break;
                }
                break;
              }
              break;
          }
        }
        while (flag2);
        if (flag1)
        {
          forWrite &= (this._attributes & JSValueAttributesInternal.Immutable) == JSValueAttributesInternal.None;
          if (forWrite)
          {
            if (this._lengthObj != null && (this._lengthObj._attributes & JSValueAttributesInternal.ReadOnly) != JSValueAttributesInternal.None && (long) index1 >= (long) this._data.Length)
            {
              if (memberScope == PropertyScope.Own)
                ExceptionHelper.Throw((Error) new TypeError("Can not add item into fixed size array"));
              return JSValue.notExists;
            }
            JSValue property = this._data[index1];
            if (property == null)
            {
              property = new JSValue()
              {
                _valueType = JSValueType.NotExistsInObject
              };
              this._data[index1] = property;
            }
            else if ((property._attributes & JSValueAttributesInternal.SystemObject) != JSValueAttributesInternal.None)
              this._data[index1] = property = property.CloneImpl(false);
            return property;
          }
          JSValue.notExists._valueType = JSValueType.NotExistsInObject;
          JSValue jsValue = this._data[index1] ?? JSValue.notExists;
          return jsValue._valueType < JSValueType.Undefined && memberScope != PropertyScope.Own ? this.__proto__.GetProperty(key, false, memberScope) : jsValue;
        }
      }
      return base.GetProperty(key, forWrite, memberScope);
    }

    [Hidden]
    public override JSValue valueOf() => base.valueOf();

    public IIterator iterator() => this._data.GetEnumerator().AsIterator();

    [DoNotEnumerate]
    [InstanceMember]
    public static IIterator entries(JSValue self)
    {
      Array array = self.As<Array>();
      return (array == null ? Array.getGenericEntriesEnumerator(self) : (IEnumerable) array.getEntriesEnumerator()).AsIterable().iterator();
    }

    private static IEnumerable getGenericEntriesEnumerator(JSValue self)
    {
      long length = Tools.getLengthOfArraylike(self, false);
      for (uint i = 0; (long) i < length; ++i)
      {
        JSValue jsValue = i >= (uint) int.MaxValue ? self.GetProperty(i.ToString()) : self.GetProperty(Tools.Int32ToString((int) i));
        yield return (object) new Array()
        {
          (JSValue) (long) i,
          jsValue
        };
      }
    }

    private IEnumerable<Array> getEntriesEnumerator()
    {
      int prev = -1;
      foreach (KeyValuePair<int, JSValue> keyValuePair in this._data.DirectOrder)
      {
        KeyValuePair<int, JSValue> item = keyValuePair;
        if (item.Key - prev > 1)
        {
          while (prev < item.Key - 1)
          {
            ++prev;
            yield return new Array()
            {
              (JSValue) prev,
              this[prev]
            };
          }
        }
        if (item.Value == null || !item.Value.Exists)
          yield return new Array()
          {
            (JSValue) item.Key,
            this[item.Key]
          };
        else
          yield return new Array()
          {
            (JSValue) item.Key,
            item.Value
          };
        prev = item.Key;
        item = new KeyValuePair<int, JSValue>();
      }
    }

    private sealed class JSComparer : IComparer<JSValue>
    {
      private Arguments args;
      private JSValue first;
      private JSValue second;
      private Function comparer;

      public JSComparer(Arguments args, JSValue first, JSValue second, Function comparer)
      {
        this.args = args;
        this.first = first;
        this.second = second;
        this.comparer = comparer;
      }

      public int Compare(JSValue x, JSValue y)
      {
        this.first.Assign(x);
        this.second.Assign(y);
        this.args[0] = this.first;
        this.args[1] = this.second;
        return Tools.JSObjectToInt32(this.comparer.Call(JSValue.undefined, this.args));
      }
    }

    private sealed class LengthField : JSValue
    {
      private Array array;

      public LengthField(Array owner)
      {
        this._attributes |= JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.NonConfigurable | JSValueAttributesInternal.Reassign;
        this.array = owner;
        if ((long) (int) this.array._data.Length == (long) this.array._data.Length)
        {
          this._iValue = (int) this.array._data.Length;
          this._valueType = JSValueType.Integer;
        }
        else
        {
          this._dValue = (double) this.array._data.Length;
          this._valueType = JSValueType.Double;
        }
      }

      public override void Assign(JSValue value)
      {
        double d = Tools.JSObjectToDouble(value);
        uint nlen = (uint) d;
        if (double.IsNaN(d) || double.IsInfinity(d) || (double) nlen != d)
          ExceptionHelper.Throw((Error) new RangeError("Invalid array length"));
        if ((this._attributes & JSValueAttributesInternal.ReadOnly) != JSValueAttributesInternal.None)
          return;
        this.array.SetLenght((long) nlen);
        if ((long) (int) this.array._data.Length == (long) this.array._data.Length)
        {
          this._iValue = (int) this.array._data.Length;
          this._valueType = JSValueType.Integer;
        }
        else
        {
          this._dValue = (double) this.array._data.Length;
          this._valueType = JSValueType.Double;
        }
      }
    }
  }
}
