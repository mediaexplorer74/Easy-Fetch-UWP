// Decompiled with JetBrains decompiler
// Type: NiL.JS.BaseLibrary.Number
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Core.Interop;
using System;
using System.Globalization;
using System.Text;

namespace NiL.JS.BaseLibrary
{
  public sealed class Number : JSObject
  {
    [ReadOnly]
    [DoNotDelete]
    [DoNotEnumerate]
    [NotConfigurable]
    public static readonly JSValue NaN = (JSValue) double.NaN;
    [ReadOnly]
    [DoNotDelete]
    [DoNotEnumerate]
    [NotConfigurable]
    public static readonly JSValue POSITIVE_INFINITY = (JSValue) double.PositiveInfinity;
    [ReadOnly]
    [DoNotDelete]
    [DoNotEnumerate]
    [NotConfigurable]
    public static readonly JSValue NEGATIVE_INFINITY = (JSValue) double.NegativeInfinity;
    [ReadOnly]
    [DoNotDelete]
    [DoNotEnumerate]
    [NotConfigurable]
    public static readonly JSValue MAX_VALUE = (JSValue) double.MaxValue;
    [ReadOnly]
    [DoNotDelete]
    [DoNotEnumerate]
    [NotConfigurable]
    public static readonly JSValue MIN_VALUE = (JSValue) double.Epsilon;

    [DoNotEnumerate]
    static Number()
    {
      Number.POSITIVE_INFINITY._attributes |= JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.NonConfigurable | JSValueAttributesInternal.SystemObject;
      Number.NEGATIVE_INFINITY._attributes |= JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.NonConfigurable | JSValueAttributesInternal.SystemObject;
      Number.MAX_VALUE._attributes |= JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.NonConfigurable | JSValueAttributesInternal.SystemObject;
      Number.MIN_VALUE._attributes |= JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.NonConfigurable | JSValueAttributesInternal.SystemObject;
      Number.NaN._attributes |= JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.NonConfigurable | JSValueAttributesInternal.SystemObject;
    }

    [DoNotEnumerate]
    public Number()
    {
      this._valueType = JSValueType.Integer;
      this._iValue = 0;
      this._attributes |= JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.SystemObject;
    }

    [DoNotEnumerate]
    [StrictConversion]
    public Number(int value)
    {
      this._valueType = JSValueType.Integer;
      this._iValue = value;
      this._attributes |= JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.SystemObject;
    }

    [Hidden]
    public Number(long value)
    {
      if ((long) (int) value == value)
      {
        this._valueType = JSValueType.Integer;
        this._iValue = (int) value;
      }
      else
      {
        this._valueType = JSValueType.Double;
        this._dValue = (double) value;
      }
      this._attributes |= JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.SystemObject;
    }

    [DoNotEnumerate]
    [StrictConversion]
    public Number(double value)
    {
      this._valueType = JSValueType.Double;
      this._dValue = value;
      this._attributes |= JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.SystemObject;
    }

    [DoNotEnumerate]
    [StrictConversion]
    public Number(string value)
    {
      value = (value ?? "0").Trim(Tools.TrimChars);
      this._valueType = JSValueType.Integer;
      this._dValue = value.Length != 0 ? double.NaN : 0.0;
      this._valueType = JSValueType.Double;
      double num = 0.0;
      int index = 0;
      if (value.Length != 0 && Tools.ParseNumber(value, ref index, out num, 0, (ParseNumberOptions) (14 | (Context.CurrentContext._strict ? 1 : 0))) && index == value.Length)
        this._dValue = num;
      this._attributes |= JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.SystemObject;
    }

    [DoNotEnumerate]
    public Number(Arguments obj)
    {
      this._valueType = JSValueType.Double;
      this._dValue = Tools.JSObjectToDouble(obj[0]);
      this._attributes |= JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.SystemObject;
    }

    [DoNotEnumerate]
    [InstanceMember]
    public static JSValue toPrecision(JSValue self, Arguments digits) => (JSValue) Tools.JSObjectToDouble(self).ToString("G" + Tools.JSObjectToInt32(digits[0]).ToString(), (IFormatProvider) CultureInfo.InvariantCulture);

    [DoNotEnumerate]
    [InstanceMember]
    public static JSValue toExponential(JSValue self, Arguments digits)
    {
      double num1;
      switch (self._valueType)
      {
        case JSValueType.Integer:
          num1 = (double) self._iValue;
          break;
        case JSValueType.Double:
          num1 = self._dValue;
          break;
        case JSValueType.Object:
          if ((object) typeof (Number) != (object) self.GetType())
            ExceptionHelper.Throw((Error) new TypeError("Try to call Number.toExponential on not number object."));
          num1 = self._iValue == 0 ? self._dValue : (double) self._iValue;
          break;
        default:
          throw new InvalidOperationException();
      }
      int num2 = 0;
      switch (((JSValue) digits ?? JSValue.undefined)._valueType)
      {
        case JSValueType.Integer:
          num2 = digits._iValue;
          break;
        case JSValueType.Double:
          num2 = (int) digits._dValue;
          break;
        case JSValueType.String:
          double num3 = 0.0;
          int index = 0;
          if (Tools.ParseNumber(digits._oValue.ToString(), ref index, out num3, ParseNumberOptions.Default))
          {
            num2 = (int) num3;
            break;
          }
          break;
        case JSValueType.Object:
          JSValue valueValueString = digits[0].ToPrimitiveValue_Value_String();
          if (valueValueString._valueType != JSValueType.String)
          {
            if (valueValueString._valueType != JSValueType.Integer)
            {
              if (valueValueString._valueType != JSValueType.Double)
                break;
              goto case JSValueType.Double;
            }
            else
              goto case JSValueType.Integer;
          }
          else
            goto case JSValueType.String;
        default:
          return (JSValue) num1.ToString("e", (IFormatProvider) CultureInfo.InvariantCulture);
      }
      return (JSValue) num1.ToString("e" + num2.ToString(), (IFormatProvider) CultureInfo.InvariantCulture);
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue toFixed(JSValue self, Arguments digits)
    {
      double num;
      switch (self._valueType)
      {
        case JSValueType.Integer:
          num = (double) self._iValue;
          break;
        case JSValueType.Double:
          num = self._dValue;
          break;
        case JSValueType.Object:
          if ((object) typeof (Number) != (object) self.GetType())
            ExceptionHelper.Throw((Error) new TypeError("Try to call Number.toFixed on not number object."));
          num = self._iValue == 0 ? self._dValue : (double) self._iValue;
          break;
        default:
          throw new InvalidOperationException();
      }
      int int32 = Tools.JSObjectToInt32(digits[0], true);
      if (int32 < 0 || int32 > 20)
        ExceptionHelper.Throw((Error) new RangeError("toFixed() digits argument must be between 0 and 20"));
      if (System.Math.Abs(self._dValue) >= 1E+21)
        return (JSValue) self._dValue.ToString("0.####e+0", (IFormatProvider) CultureInfo.InvariantCulture);
      if (int32 > 0)
        ++int32;
      return (JSValue) System.Math.Round(num, int32).ToString("0.00000000000000000000".Substring(0, int32 + 1), (IFormatProvider) CultureInfo.InvariantCulture);
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(0)]
    public static JSValue toLocaleString(JSValue self) => (JSValue) (self._valueType == JSValueType.Integer ? self._iValue.ToString((IFormatProvider) CultureInfo.CurrentCulture) : self._dValue.ToString((IFormatProvider) CultureInfo.CurrentCulture));

    [InstanceMember]
    [DoNotEnumerate]
    [CLSCompliant(false)]
    public static JSValue toString(JSValue self, Arguments radix)
    {
      JSValueType valueType = self._valueType;
      if (self._valueType > JSValueType.Double && self is Number)
        self._valueType = self._dValue == 0.0 ? JSValueType.Integer : JSValueType.Double;
      try
      {
        if (self._valueType != JSValueType.Integer && self._valueType != JSValueType.Double)
          ExceptionHelper.Throw((Error) new TypeError("Try to call Number.toString on not Number object"));
        int num1 = 10;
        if (radix != null && radix.GetProperty("length")._iValue > 0)
        {
          JSValue jsValue = radix[0];
          if (jsValue._valueType == JSValueType.Object && jsValue._oValue == null)
            ExceptionHelper.Throw(new Error("Radix can't be null."));
          switch (jsValue._valueType)
          {
            case JSValueType.NotExists:
            case JSValueType.NotExistsInObject:
            case JSValueType.Undefined:
              num1 = 10;
              break;
            case JSValueType.Boolean:
            case JSValueType.Integer:
              num1 = jsValue._iValue;
              break;
            case JSValueType.Double:
              num1 = (int) jsValue._dValue;
              break;
            default:
              num1 = Tools.JSObjectToInt32(jsValue);
              break;
          }
        }
        if (num1 < 2 || num1 > 36)
          ExceptionHelper.Throw((Error) new TypeError("Radix must be between 2 and 36."));
        if (num1 == 10)
          return (JSValue) self.ToString();
        long num2 = (long) self._iValue;
        StringBuilder stringBuilder = new StringBuilder();
        if (self._valueType == JSValueType.Double)
        {
          if (double.IsNaN(self._dValue))
            return (JSValue) "NaN";
          if (double.IsPositiveInfinity(self._dValue))
            return (JSValue) "Infinity";
          if (double.IsNegativeInfinity(self._dValue))
            return (JSValue) "-Infinity";
          num2 = (long) self._dValue;
          if ((double) num2 != self._dValue)
          {
            double num3 = self._dValue;
            bool flag = num3 < 0.0;
            if (flag)
              num3 = -num3;
            stringBuilder.Append(Tools.NumChars[(int) (num3 % (double) num1)]);
            long num4 = num2 / (long) num1;
            for (; num3 >= 1.0; num3 /= (double) num1)
              stringBuilder.Append(Tools.NumChars[(int) (num3 % (double) num1)]);
            if (flag)
              stringBuilder.Append('-');
            int index1 = stringBuilder.Length - 1;
            for (int index2 = 0; index1 > index2; --index1)
            {
              stringBuilder[index1] ^= stringBuilder[index2];
              stringBuilder[index2] ^= stringBuilder[index1];
              stringBuilder[index1] ^= stringBuilder[index2];
              stringBuilder[index1] += (char) ((int) stringBuilder[index1] / 65 * 32);
              stringBuilder[index2] += (char) ((int) stringBuilder[index2] / 65 * 32);
              ++index2;
            }
            return (JSValue) stringBuilder.ToString();
          }
        }
        bool flag1 = num2 < 0L;
        if (flag1)
          num2 = -num2;
        if (num2 < 0L)
          ExceptionHelper.Throw(new Error("Internal error"));
        stringBuilder.Append(Tools.NumChars[num2 % (long) num1]);
        for (long index = num2 / (long) num1; index != 0L; index /= (long) num1)
          stringBuilder.Append(Tools.NumChars[index % (long) num1]);
        if (flag1)
          stringBuilder.Append('-');
        int index3 = stringBuilder.Length - 1;
        for (int index4 = 0; index3 > index4; --index3)
        {
          stringBuilder[index3] ^= stringBuilder[index4];
          stringBuilder[index4] ^= stringBuilder[index3];
          stringBuilder[index3] ^= stringBuilder[index4];
          stringBuilder[index3] += (char) ((int) stringBuilder[index3] / 65 * 32);
          stringBuilder[index4] += (char) ((int) stringBuilder[index4] / 65 * 32);
          ++index4;
        }
        return (JSValue) stringBuilder.ToString();
      }
      finally
      {
        self._valueType = valueType;
      }
    }

    [DoNotEnumerate]
    [InstanceMember]
    public static JSValue valueOf(JSValue self)
    {
      if (self is Number)
        return (JSValue) (self._iValue == 0 ? self._dValue : (double) self._iValue);
      if (self._valueType != JSValueType.Integer && self._valueType != JSValueType.Double)
        ExceptionHelper.Throw((Error) new TypeError("Try to call Number.valueOf on not number object."));
      return self;
    }

    [Hidden]
    public override string ToString()
    {
      if (this._valueType == JSValueType.Integer)
        return Tools.Int32ToString(this._iValue);
      if (this._valueType == JSValueType.Double)
        return Tools.DoubleToString(this._dValue);
      return this._iValue != 0 ? Tools.Int32ToString(this._iValue) : Tools.DoubleToString(this._dValue);
    }

    [Hidden]
    public override int GetHashCode() => this._valueType != JSValueType.Integer ? this._dValue.GetHashCode() : this._iValue.GetHashCode();

    [Hidden]
    public static implicit operator Number(int value) => new Number(value);

    [Hidden]
    public static implicit operator Number(double value) => new Number(value);

    [Hidden]
    public static implicit operator double(Number value)
    {
      if (value == null)
        return 0.0;
      return value._valueType != JSValueType.Integer ? value._dValue : (double) value._iValue;
    }

    [Hidden]
    public static explicit operator int(Number value)
    {
      if (value == null)
        return 0;
      return value._valueType != JSValueType.Integer ? (int) value._dValue : value._iValue;
    }

    [DoNotEnumerate]
    public static JSValue isNaN(JSValue x) => x._valueType == JSValueType.Double ? (JSValue) double.IsNaN(x._dValue) : (JSValue) Boolean.False;
  }
}
