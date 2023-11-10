// Decompiled with JetBrains decompiler
// Type: NiL.JS.BaseLibrary.Math
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Core.Interop;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace NiL.JS.BaseLibrary
{
  public static class Math
  {
    [Hidden]
    private static int _randomSeed = Environment.TickCount;
    [Hidden]
    internal static readonly ThreadLocal<Random> randomInstance = new ThreadLocal<Random>((Func<Random>) (() => new Random(Interlocked.Increment(ref Math._randomSeed))));
    [DoNotDelete]
    [DoNotEnumerate]
    [NotConfigurable]
    public static readonly Number E = new Number(System.Math.E);
    [DoNotDelete]
    [DoNotEnumerate]
    [NotConfigurable]
    public static readonly Number PI = new Number(System.Math.PI);
    [DoNotDelete]
    [DoNotEnumerate]
    [NotConfigurable]
    public static readonly Number LN2 = new Number(System.Math.Log(2.0));
    [DoNotDelete]
    [DoNotEnumerate]
    [NotConfigurable]
    public static readonly Number LN10 = new Number(System.Math.Log(10.0));
    [DoNotDelete]
    [DoNotEnumerate]
    [NotConfigurable]
    public static readonly Number LOG2E = new Number(1.0 / System.Math.Log(2.0));
    [DoNotDelete]
    [DoNotEnumerate]
    [NotConfigurable]
    public static readonly Number LOG10E = new Number(1.0 / System.Math.Log(10.0));
    [DoNotDelete]
    [DoNotEnumerate]
    [NotConfigurable]
    public static readonly Number SQRT1_2 = new Number(System.Math.Sqrt(0.5));
    [DoNotDelete]
    [DoNotEnumerate]
    [NotConfigurable]
    public static readonly Number SQRT2 = new Number(System.Math.Sqrt(2.0));

    [DoNotDelete]
    [DoNotEnumerate]
    public static JSValue abs(JSValue value)
    {
      switch (value._valueType)
      {
        case JSValueType.Integer:
          if (value._iValue >= 0)
            return value;
          value = value.CloneImpl(false);
          if (value._iValue == int.MinValue)
          {
            value._valueType = JSValueType.Double;
            value._dValue = -(double) value._iValue;
          }
          else
            value._iValue = -value._iValue;
          return value;
        case JSValueType.Double:
          if (value._dValue > 0.0 || value._dValue == 0.0 && !Tools.IsNegativeZero(value._dValue))
            return value;
          value = value.CloneImpl(false);
          value._dValue = -value._dValue;
          return value;
        default:
          return (JSValue) System.Math.Abs(Tools.JSObjectToDouble(value));
      }
    }

    [DoNotEnumerate]
    [DoNotDelete]
    public static JSValue acos(JSValue value) => (JSValue) System.Math.Acos(Tools.JSObjectToDouble(value));

    [DoNotEnumerate]
    [DoNotDelete]
    public static JSValue acosh(JSValue value)
    {
      double num = Tools.JSObjectToDouble(value);
      return (JSValue) (num >= 1.0 ? System.Math.Log(num + System.Math.Sqrt(num * num - 1.0)) : double.NaN);
    }

    [DoNotEnumerate]
    [DoNotDelete]
    public static JSValue asin(JSValue value) => (JSValue) System.Math.Asin(Tools.JSObjectToDouble(value));

    [DoNotEnumerate]
    [DoNotDelete]
    public static JSValue asinh(JSValue value)
    {
      double num = Tools.JSObjectToDouble(value);
      return (JSValue) System.Math.Log(num + System.Math.Sqrt(num * num + 1.0));
    }

    [DoNotEnumerate]
    [DoNotDelete]
    public static JSValue atan(JSValue value) => (JSValue) System.Math.Atan(Tools.JSObjectToDouble(value));

    [DoNotEnumerate]
    [DoNotDelete]
    public static JSValue atanh(JSValue value)
    {
      double num = Tools.JSObjectToDouble(value);
      return (JSValue) (System.Math.Abs(num) < 1.0 ? 0.5 * System.Math.Log((1.0 + num) / (1.0 - num)) : double.NaN);
    }

    [DoNotEnumerate]
    [DoNotDelete]
    [ArgumentsCount(2)]
    public static JSValue atan2(JSValue x, JSValue y)
    {
      if (!x.Defined || !y.Defined)
        return (JSValue) double.NaN;
      double num1 = Tools.JSObjectToDouble(x);
      double num2 = Tools.JSObjectToDouble(y);
      return double.IsInfinity(num1) && double.IsInfinity(num2) ? (JSValue) System.Math.Atan2((double) System.Math.Sign(num1), (double) System.Math.Sign(num2)) : (JSValue) System.Math.Atan2(num1, num2);
    }

    [DoNotEnumerate]
    [DoNotDelete]
    public static JSValue cbrt(JSValue value)
    {
      double d = Tools.JSObjectToDouble(value);
      if (double.IsNaN(d))
        return Number.NaN;
      double num = System.Math.Pow(System.Math.Abs(d), 1.0 / 3.0);
      if (d < 0.0)
        num = -num;
      return (JSValue) num;
    }

    [DoNotEnumerate]
    [DoNotDelete]
    public static JSValue ceil(JSValue value) => (JSValue) System.Math.Ceiling(Tools.JSObjectToDouble(value));

    [DoNotEnumerate]
    [DoNotDelete]
    public static JSValue clz32(JSValue value)
    {
      uint num1 = (uint) Tools.JSObjectToInt32(value, 0, 0, false);
      if (num1 < 0U)
        return (JSValue) 0;
      if (num1 == 0U)
        return (JSValue) 32;
      int num2 = 0;
      int num3 = 16;
      while (num1 > 1U)
      {
        uint num4 = num1 >> num3;
        if (num4 != 0U)
        {
          num1 = num4;
          num2 += num3;
        }
        num3 >>= 1;
      }
      return (JSValue) (31 - num2);
    }

    [DoNotEnumerate]
    [DoNotDelete]
    public static JSValue cos(JSValue value) => (JSValue) System.Math.Cos(Tools.JSObjectToDouble(value));

    [DoNotEnumerate]
    [DoNotDelete]
    public static JSValue cosh(JSValue value) => (JSValue) System.Math.Cosh(Tools.JSObjectToDouble(value));

    [DoNotEnumerate]
    [DoNotDelete]
    public static JSValue exp(JSValue value)
    {
      double num = System.Math.Exp(Tools.JSObjectToDouble(value));
      if ((value._attributes & JSValueAttributesInternal.Cloned) == JSValueAttributesInternal.None)
        return (JSValue) num;
      value._valueType = JSValueType.Double;
      value._dValue = num;
      return value;
    }

    [DoNotEnumerate]
    [DoNotDelete]
    public static JSValue expm1(JSValue value)
    {
      double d = Tools.JSObjectToDouble(value);
      double num1 = 0.0;
      if (d >= -1.0 && d <= 1.0)
      {
        double num2 = 1.0;
        double num3 = d;
        double num4 = d;
        int num5 = 2;
        for (; num1 != num4; num4 += num3 / num2)
        {
          num1 = num4;
          num3 *= d;
          num2 *= (double) num5++;
        }
      }
      else
        num1 = System.Math.Exp(d) - 1.0;
      if ((value._attributes & JSValueAttributesInternal.Cloned) == JSValueAttributesInternal.None)
        return (JSValue) num1;
      value._valueType = JSValueType.Double;
      value._dValue = num1;
      return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong shr(ulong x, int y) => y > 63 ? 0UL : x >> y;

    [DoNotDelete]
    [DoNotEnumerate]
    public static JSValue floor(JSValue value)
    {
      if (value._valueType == JSValueType.Integer)
        return value;
      double d = Tools.JSObjectToDouble(value);
      if (d == 0.0)
      {
        if ((value._attributes & JSValueAttributesInternal.Cloned) == JSValueAttributesInternal.None)
          return (JSValue) d;
        value._valueType = JSValueType.Integer;
        value._iValue = 0;
        return value;
      }
      if (double.IsNaN(d))
        return Number.NaN;
      long int64Bits = BitConverter.DoubleToInt64Bits(d);
      ulong x = (ulong) (int64Bits & 4503599627370495L | 4503599627370496L);
      long num1 = int64Bits >> 63 | 1L;
      int y = 52 - (int) ((int64Bits & long.MaxValue) >> 52) + 1023;
      if (y > 0)
      {
        if (num1 < 0L)
          return y > 64 || ((long) x & (1L << y) - 1L) != 0L ? (JSValue) (-(long) Math.shr(x, y) - 1L) : (JSValue) -(long) Math.shr(x, y);
        long num2 = (long) Math.shr(x, y) * num1;
        if ((value._attributes & JSValueAttributesInternal.Cloned) == JSValueAttributesInternal.None)
          return (JSValue) num2;
        if ((num2 & (long) uint.MaxValue) == num2)
        {
          value._valueType = JSValueType.Integer;
          value._iValue = (int) num2;
        }
        else
        {
          value._valueType = JSValueType.Double;
          value._dValue = (double) num2;
        }
        return value;
      }
      if ((value._attributes & JSValueAttributesInternal.Cloned) == JSValueAttributesInternal.None)
        return (JSValue) d;
      value._valueType = JSValueType.Double;
      value._dValue = d;
      return value;
    }

    [DoNotDelete]
    [DoNotEnumerate]
    public static JSValue fround(JSValue value) => (JSValue) Tools.JSObjectToDouble(value);

    [DoNotEnumerate]
    [DoNotDelete]
    [ArgumentsCount(2)]
    public static JSValue hypot(Arguments args)
    {
      JSValue jsValue = (JSValue) null;
      double d1 = 0.0;
      for (int index = 0; index < args.Length; ++index)
      {
        if (jsValue == null && (args[index]._attributes & JSValueAttributesInternal.Cloned) != JSValueAttributesInternal.None)
          jsValue = args[index];
        double d2 = Tools.JSObjectToDouble(args[index]);
        if (double.IsInfinity(d2))
        {
          d1 = double.PositiveInfinity;
          break;
        }
        d1 += d2 * d2;
      }
      double num = System.Math.Sqrt(d1);
      if (jsValue == null)
        return (JSValue) num;
      jsValue._valueType = JSValueType.Double;
      jsValue._dValue = num;
      return jsValue;
    }

    [DoNotEnumerate]
    [DoNotDelete]
    [ArgumentsCount(2)]
    public static JSValue imul(JSValue x, JSValue y) => (JSValue) (Tools.JSObjectToInt32(x, 0, 0, false) * Tools.JSObjectToInt32(y, 0, 0, false));

    [DoNotDelete]
    [DoNotEnumerate]
    public static JSValue log(JSValue value)
    {
      double num = System.Math.Log(Tools.JSObjectToDouble(value));
      if ((value._attributes & JSValueAttributesInternal.Cloned) == JSValueAttributesInternal.None)
        return (JSValue) num;
      value._valueType = JSValueType.Double;
      value._dValue = num;
      return value;
    }

    [DoNotDelete]
    [DoNotEnumerate]
    public static JSValue log1p(JSValue value)
    {
      double num1 = Tools.JSObjectToDouble(value);
      double num2 = 0.0;
      if (num1 >= -0.25 && num1 <= 0.25)
      {
        double num3 = num1;
        double num4 = num1;
        int num5 = 1;
        double num6;
        int num7;
        for (; num2 != num4; num4 = num4 - (num6 = num3 * num1) / (double) (num7 = num5 + 1) + (num3 = num6 * num1) / (double) (num5 = num7 + 1))
          num2 = num4;
        return (JSValue) num2;
      }
      double num8 = System.Math.Log(num1 + 1.0);
      if ((value._attributes & JSValueAttributesInternal.Cloned) == JSValueAttributesInternal.None)
        return (JSValue) num8;
      value._valueType = JSValueType.Double;
      value._dValue = num8;
      return value;
    }

    [DoNotDelete]
    [DoNotEnumerate]
    public static JSValue log10(JSValue value)
    {
      double num = System.Math.Log10(Tools.JSObjectToDouble(value));
      if ((value._attributes & JSValueAttributesInternal.Cloned) == JSValueAttributesInternal.None)
        return (JSValue) num;
      value._valueType = JSValueType.Double;
      value._dValue = num;
      return value;
    }

    [DoNotDelete]
    [DoNotEnumerate]
    public static JSValue log2(JSValue value)
    {
      double num = System.Math.Log(Tools.JSObjectToDouble(value)) * 1.4426950408889634;
      if ((value._attributes & JSValueAttributesInternal.Cloned) == JSValueAttributesInternal.None)
        return (JSValue) num;
      value._valueType = JSValueType.Double;
      value._dValue = num;
      return value;
    }

    [DoNotDelete]
    [DoNotEnumerate]
    [ArgumentsCount(2)]
    public static JSValue max(Arguments args)
    {
      JSValue jsValue = (JSValue) null;
      double val1 = double.NegativeInfinity;
      for (int index = 0; index < args.Length; ++index)
      {
        if (jsValue == null && (args[index]._attributes & JSValueAttributesInternal.Cloned) != JSValueAttributesInternal.None)
          jsValue = args[index];
        double num = Tools.JSObjectToDouble(args[index]);
        if (double.IsNaN(num))
          return Number.NaN;
        val1 = System.Math.Max(val1, num);
      }
      if (jsValue == null)
        return (JSValue) val1;
      jsValue._valueType = JSValueType.Double;
      jsValue._dValue = val1;
      return jsValue;
    }

    [DoNotEnumerate]
    [DoNotDelete]
    [ArgumentsCount(2)]
    public static JSValue min(Arguments args)
    {
      JSValue jsValue = (JSValue) null;
      double val1 = double.PositiveInfinity;
      for (int index = 0; index < args.Length; ++index)
      {
        if (jsValue == null && (args[index]._attributes & JSValueAttributesInternal.Cloned) != JSValueAttributesInternal.None)
          jsValue = args[index];
        double num = Tools.JSObjectToDouble(args[index]);
        if (double.IsNaN(num))
          return Number.NaN;
        val1 = System.Math.Min(val1, num);
      }
      if (jsValue == null)
        return (JSValue) val1;
      jsValue._valueType = JSValueType.Double;
      jsValue._dValue = val1;
      return jsValue;
    }

    [DoNotEnumerate]
    [DoNotDelete]
    [ArgumentsCount(2)]
    public static JSValue pow(JSValue a, JSValue b)
    {
      if (!a.Defined || !b.Defined)
        return Number.NaN;
      double num1 = Tools.JSObjectToDouble(a);
      double num2 = Tools.JSObjectToDouble(b);
      if ((num1 == 1.0 || num1 == -1.0) && double.IsInfinity(num2))
        return Number.NaN;
      return double.IsNaN(num1) && num2 == 0.0 ? (JSValue) 1 : (JSValue) System.Math.Pow(num1, num2);
    }

    [DoNotEnumerate]
    [DoNotDelete]
    public static JSValue random() => (JSValue) Math.randomInstance.Value.NextDouble();

    [DoNotDelete]
    [DoNotEnumerate]
    public static JSValue round(JSValue value)
    {
      if (value._valueType == JSValueType.Integer)
        return value;
      double d = Tools.JSObjectToDouble(value);
      if (d == 0.0)
      {
        if ((value._attributes & JSValueAttributesInternal.Cloned) == JSValueAttributesInternal.None)
          return (JSValue) d;
        value._valueType = JSValueType.Integer;
        value._iValue = 0;
        return value;
      }
      if (double.IsNaN(d))
        return Number.NaN;
      long int64Bits = BitConverter.DoubleToInt64Bits(d);
      ulong x = (ulong) (int64Bits & 4503599627370495L | 4503599627370496L);
      long num1 = int64Bits >> 63 | 1L;
      int y = 52 - (int) ((int64Bits & long.MaxValue) >> 52) + 1023;
      if (y > 0)
      {
        if (num1 < 0L && ((long) Math.shr(x, y - 1) & 1L) == 1L)
          return ((long) x & (1L << y - 1) - 1L) != 0L ? (JSValue) (-(long) Math.shr(x, y) - 1L) : (JSValue) -(long) Math.shr(x, y);
        long num2 = ((long) Math.shr(x, y) + ((long) Math.shr(x, y - 1) & 1L) * num1) * num1;
        if ((value._attributes & JSValueAttributesInternal.Cloned) == JSValueAttributesInternal.None)
          return (JSValue) num2;
        if ((num2 & (long) uint.MaxValue) == num2)
        {
          value._valueType = JSValueType.Integer;
          value._iValue = (int) num2;
        }
        else
        {
          value._valueType = JSValueType.Double;
          value._dValue = (double) num2;
        }
        return value;
      }
      if ((value._attributes & JSValueAttributesInternal.Cloned) == JSValueAttributesInternal.None)
        return (JSValue) d;
      value._valueType = JSValueType.Double;
      value._dValue = d;
      return value;
    }

    [DoNotEnumerate]
    [DoNotDelete]
    public static JSValue sign(JSValue value)
    {
      double d = Tools.JSObjectToDouble(value);
      if (!double.IsNaN(d))
        d = (double) System.Math.Sign(d);
      if ((value._attributes & JSValueAttributesInternal.Cloned) == JSValueAttributesInternal.None)
        return (JSValue) d;
      value._valueType = JSValueType.Double;
      value._dValue = d;
      return value;
    }

    [DoNotEnumerate]
    [DoNotDelete]
    public static JSValue sin(JSValue value)
    {
      double num = System.Math.Sin(Tools.JSObjectToDouble(value));
      if ((value._attributes & JSValueAttributesInternal.Cloned) == JSValueAttributesInternal.None)
        return (JSValue) num;
      value._valueType = JSValueType.Double;
      value._dValue = num;
      return value;
    }

    [DoNotEnumerate]
    [DoNotDelete]
    public static JSValue sinh(JSValue value)
    {
      double num = System.Math.Sinh(Tools.JSObjectToDouble(value));
      if ((value._attributes & JSValueAttributesInternal.Cloned) == JSValueAttributesInternal.None)
        return (JSValue) num;
      value._valueType = JSValueType.Double;
      value._dValue = num;
      return value;
    }

    [DoNotEnumerate]
    [DoNotDelete]
    public static JSValue sqrt(JSValue value)
    {
      double num = System.Math.Sqrt(Tools.JSObjectToDouble(value));
      if ((value._attributes & JSValueAttributesInternal.Cloned) == JSValueAttributesInternal.None)
        return (JSValue) num;
      value._valueType = JSValueType.Double;
      value._dValue = num;
      return value;
    }

    [DoNotEnumerate]
    [DoNotDelete]
    public static JSValue tan(JSValue value)
    {
      double num = System.Math.Tan(Tools.JSObjectToDouble(value));
      if ((value._attributes & JSValueAttributesInternal.Cloned) == JSValueAttributesInternal.None)
        return (JSValue) num;
      value._valueType = JSValueType.Double;
      value._dValue = num;
      return value;
    }

    [DoNotEnumerate]
    [DoNotDelete]
    public static JSValue tanh(JSValue value)
    {
      double num = System.Math.Tanh(Tools.JSObjectToDouble(value));
      if ((value._attributes & JSValueAttributesInternal.Cloned) == JSValueAttributesInternal.None)
        return (JSValue) num;
      value._valueType = JSValueType.Double;
      value._dValue = num;
      return value;
    }

    [DoNotEnumerate]
    [DoNotDelete]
    public static JSValue trunc(JSValue value)
    {
      double num = System.Math.Truncate(Tools.JSObjectToDouble(value));
      if ((value._attributes & JSValueAttributesInternal.Cloned) == JSValueAttributesInternal.None)
        return (JSValue) num;
      value._valueType = JSValueType.Double;
      value._dValue = num;
      return value;
    }

    [DoNotEnumerate]
    [DoNotDelete]
    [ArgumentsCount(2)]
    public static JSValue IEEERemainder(JSValue a, JSValue b) => (JSValue) System.Math.IEEERemainder(Tools.JSObjectToDouble(a), Tools.JSObjectToDouble(b));
  }
}
