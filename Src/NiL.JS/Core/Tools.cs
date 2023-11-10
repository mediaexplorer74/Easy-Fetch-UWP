// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Tools
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core.Functions;
using NiL.JS.Core.Interop;
using NiL.JS.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NiL.JS.Core
{
  public static class Tools
  {
    private static readonly Type[] intTypeWithinArray = new Type[1]
    {
      typeof (int)
    };
    internal static readonly char[] TrimChars = new char[26]
    {
      ' ',
      '\t',
      '\n',
      '\r',
      '\v',
      '\f',
      ' ',
      ' ',
      '\u180E',
      ' ',
      ' ',
      ' ',
      ' ',
      ' ',
      ' ',
      ' ',
      ' ',
      ' ',
      ' ',
      ' ',
      '\u2028',
      '\u2029',
      ' ',
      ' ',
      '　',
      '\uFEFF'
    };
    internal static readonly char[] NumChars = new char[36]
    {
      '0',
      '1',
      '2',
      '3',
      '4',
      '5',
      '6',
      '7',
      '8',
      '9',
      'A',
      'B',
      'C',
      'D',
      'E',
      'F',
      'G',
      'H',
      'I',
      'J',
      'K',
      'L',
      'M',
      'N',
      'O',
      'P',
      'Q',
      'R',
      'S',
      'T',
      'U',
      'V',
      'W',
      'X',
      'Y',
      'Z'
    };
    private static readonly Tools.DoubleStringCacheItem[] cachedDoubleString = new Tools.DoubleStringCacheItem[8];
    private static int cachedDoubleStringsIndex = 0;
    private static readonly string[] divFormats = new string[15]
    {
      ".#",
      ".##",
      ".###",
      ".####",
      ".#####",
      ".######",
      ".#######",
      ".########",
      ".#########",
      ".##########",
      ".###########",
      ".############",
      ".#############",
      ".##############",
      ".###############"
    };
    private static readonly Decimal[] powersOf10 = new Decimal[37]
    {
      0.000000000000000001M,
      0.00000000000000001M,
      0.0000000000000001M,
      0.000000000000001M,
      0.00000000000001M,
      0.0000000000001M,
      0.000000000001M,
      0.00000000001M,
      0.0000000001M,
      0.000000001M,
      0.00000001M,
      0.0000001M,
      0.000001M,
      0.00001M,
      0.0001M,
      0.001M,
      0.01M,
      0.1M,
      1M,
      10M,
      100M,
      1000M,
      10000M,
      100000M,
      1000000M,
      10000000M,
      100000000M,
      1000000000M,
      10000000000M,
      100000000000M,
      1000000000000M,
      10000000000000M,
      100000000000000M,
      1000000000000000M,
      10000000000000000M,
      100000000000000000M,
      1000000000000000000M
    };
    private const int cacheSize = 16;
    private static readonly Tools.IntStringCacheItem[] intStringCache = new Tools.IntStringCacheItem[16];
    private static int intStrCacheIndex = -1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double JSObjectToDouble(JSValue arg)
    {
      for (; arg != null; arg = arg.ToPrimitiveValue_Value_String())
      {
        switch (arg._valueType)
        {
          case JSValueType.NotExists:
          case JSValueType.NotExistsInObject:
          case JSValueType.Undefined:
            return double.NaN;
          case JSValueType.Boolean:
          case JSValueType.Integer:
            return (double) arg._iValue;
          case JSValueType.Double:
            return arg._dValue;
          case JSValueType.String:
            double num = double.NaN;
            int index = 0;
            string code = arg._oValue.ToString();
            if (code.Length > 0 && (Tools.IsWhiteSpace(code[0]) || Tools.IsWhiteSpace(code[code.Length - 1])))
              code = code.Trim(Tools.TrimChars);
            return Tools.ParseNumber(code, ref index, out num, 0, ParseNumberOptions.AllowFloat | ParseNumberOptions.AllowAutoRadix) && index < code.Length ? double.NaN : num;
          case JSValueType.Object:
          case JSValueType.Function:
          case JSValueType.Date:
            if (arg._oValue == null)
              return 0.0;
            continue;
          default:
            throw new NotImplementedException();
        }
      }
      return double.NaN;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int JSObjectToInt32(JSValue arg) => arg._valueType == JSValueType.Integer ? arg._iValue : Tools.JSObjectToInt32(arg, 0, 0, false);

    internal static void SkipSpaces(string code, ref int i)
    {
      while (i < code.Length && Tools.IsWhiteSpace(code[i]))
        ++i;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int JSObjectToInt32(JSValue arg, int nullOrUndefinedOrNan) => Tools.JSObjectToInt32(arg, nullOrUndefinedOrNan, 0, false);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int JSObjectToInt32(JSValue arg, bool alternateInfinity) => Tools.JSObjectToInt32(arg, 0, 0, alternateInfinity);

    public static int JSObjectToInt32(JSValue arg, int nullOrUndefined, bool alternateInfinity) => Tools.JSObjectToInt32(arg, nullOrUndefined, 0, alternateInfinity);

    public static int JSObjectToInt32(
      JSValue arg,
      int nullOrUndefined,
      int nan,
      bool alternateInfinity)
    {
      return Tools.JSObjectToInt32(arg, nullOrUndefined, nullOrUndefined, nan, alternateInfinity);
    }

    public static int JSObjectToInt32(
      JSValue value,
      int @null,
      int undefined,
      int nan,
      bool alternateInfinity)
    {
      if (value == null)
        return @null;
      switch (value._valueType)
      {
        case JSValueType.NotExists:
        case JSValueType.NotExistsInObject:
        case JSValueType.Undefined:
          return undefined;
        case JSValueType.Boolean:
        case JSValueType.Integer:
          return value._iValue;
        case JSValueType.Double:
          if (double.IsNaN(value._dValue))
            return nan;
          if (!double.IsInfinity(value._dValue))
            return (int) (long) value._dValue;
          if (!alternateInfinity)
            return 0;
          return !double.IsPositiveInfinity(value._dValue) ? int.MinValue : int.MaxValue;
        case JSValueType.String:
          double d = 0.0;
          int index = 0;
          string code = value._oValue.ToString().Trim();
          if (!Tools.ParseNumber(code, ref index, out d, 0, ParseNumberOptions.AllowFloat | ParseNumberOptions.AllowAutoRadix) || index < code.Length)
            return 0;
          if (double.IsNaN(d))
            return nan;
          if (!double.IsInfinity(d))
            return (int) d;
          if (!alternateInfinity)
            return 0;
          return !double.IsPositiveInfinity(d) ? int.MinValue : int.MaxValue;
        case JSValueType.Object:
        case JSValueType.Function:
        case JSValueType.Date:
          if (value._oValue == null)
            return @null;
          value = value.ToPrimitiveValue_Value_String();
          return Tools.JSObjectToInt32(value, 0, 0, 0, true);
        default:
          throw new NotImplementedException();
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long JSObjectToInt64(JSValue arg) => Tools.JSObjectToInt64(arg, 0L, false);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long JSObjectToInt64(JSValue arg, long nullOrUndefinedOrNan) => Tools.JSObjectToInt64(arg, nullOrUndefinedOrNan, false);

    public static long JSObjectToInt64(JSValue arg, long nullOrUndefined, bool alternateInfinity)
    {
      if (arg == null)
        return nullOrUndefined;
      JSValue jsValue = arg;
      switch (jsValue._valueType)
      {
        case JSValueType.NotExists:
        case JSValueType.NotExistsInObject:
        case JSValueType.Undefined:
          return nullOrUndefined;
        case JSValueType.Boolean:
        case JSValueType.Integer:
          return (long) jsValue._iValue;
        case JSValueType.Double:
          if (double.IsNaN(jsValue._dValue))
            return 0;
          if (!double.IsInfinity(jsValue._dValue))
            return (long) jsValue._dValue;
          if (!alternateInfinity)
            return 0;
          return !double.IsPositiveInfinity(jsValue._dValue) ? long.MinValue : long.MaxValue;
        case JSValueType.String:
          double d = 0.0;
          int index = 0;
          string code = jsValue._oValue.ToString().Trim();
          if (!Tools.ParseNumber(code, ref index, out d, 0, ParseNumberOptions.AllowFloat | ParseNumberOptions.AllowAutoRadix) || index < code.Length || double.IsNaN(d))
            return 0;
          if (!double.IsInfinity(d))
            return (long) d;
          if (!alternateInfinity)
            return 0;
          return !double.IsPositiveInfinity(d) ? long.MinValue : long.MaxValue;
        case JSValueType.Object:
        case JSValueType.Function:
        case JSValueType.Date:
          return jsValue._oValue == null ? nullOrUndefined : Tools.JSObjectToInt64(jsValue.ToPrimitiveValue_Value_String(), nullOrUndefined, alternateInfinity);
        default:
          throw new NotImplementedException();
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static JSValue JSObjectToNumber(JSValue arg) => Tools.JSObjectToNumber(arg, new JSValue());

    internal static JSValue JSObjectToNumber(JSValue arg, JSValue result)
    {
      if (arg == null)
      {
        result._valueType = JSValueType.Integer;
        result._iValue = 0;
        return result;
      }
      switch (arg._valueType)
      {
        case JSValueType.NotExists:
        case JSValueType.NotExistsInObject:
        case JSValueType.Undefined:
          result._valueType = JSValueType.Double;
          result._dValue = double.NaN;
          return result;
        case JSValueType.Boolean:
          result._valueType = JSValueType.Integer;
          result._iValue = arg._iValue;
          return result;
        case JSValueType.Integer:
        case JSValueType.Double:
          return arg;
        case JSValueType.String:
          double num = 0.0;
          int index = 0;
          string code = arg._oValue.ToString().Trim(Tools.TrimChars);
          if (!Tools.ParseNumber(code, ref index, out num, ParseNumberOptions.AllowFloat | ParseNumberOptions.AllowAutoRadix) || index < code.Length)
            num = double.NaN;
          result._valueType = JSValueType.Double;
          result._dValue = num;
          return result;
        case JSValueType.Object:
        case JSValueType.Function:
        case JSValueType.Date:
          if (arg._oValue == null)
          {
            result._valueType = JSValueType.Integer;
            result._iValue = 0;
            return result;
          }
          arg = arg.ToPrimitiveValue_Value_String();
          return Tools.JSObjectToNumber(arg);
        default:
          throw new NotImplementedException();
      }
    }

    internal static object convertJStoObj(JSValue jsobj, Type targetType, bool hightLoyalty)
    {
      if (jsobj == null)
        return (object) null;
      if (TypeExtensions.IsAssignableFrom(targetType, jsobj.GetType()))
        return (object) jsobj;
      if (targetType.GetTypeInfo().IsGenericType && (object) targetType.GetGenericTypeDefinition() == (object) typeof (Nullable<>))
        targetType = TypeExtensions.GetGenericArguments(targetType)[0];
      switch (jsobj._valueType)
      {
        case JSValueType.Boolean:
          if (hightLoyalty && (object) targetType == (object) typeof (string))
            return jsobj._iValue == 0 ? (object) "false" : (object) "true";
          if ((object) targetType == (object) typeof (bool))
            return (object) (jsobj._iValue != 0);
          return (object) targetType == (object) typeof (NiL.JS.BaseLibrary.Boolean) ? (object) new NiL.JS.BaseLibrary.Boolean(jsobj._iValue != 0) : (object) null;
        case JSValueType.Integer:
          if (hightLoyalty)
          {
            if ((object) targetType == (object) typeof (string))
              return (object) Tools.Int32ToString(jsobj._iValue);
            if ((object) targetType == (object) typeof (byte))
              return (object) (byte) jsobj._iValue;
            if ((object) targetType == (object) typeof (sbyte))
              return (object) (sbyte) jsobj._iValue;
            if ((object) targetType == (object) typeof (ushort))
              return (object) (ushort) jsobj._iValue;
            if ((object) targetType == (object) typeof (short))
              return (object) (short) jsobj._iValue;
          }
          if ((object) targetType == (object) typeof (int))
            return (object) jsobj._iValue;
          if ((object) targetType == (object) typeof (uint))
            return (object) (uint) jsobj._iValue;
          if ((object) targetType == (object) typeof (long))
            return (object) (long) jsobj._iValue;
          if ((object) targetType == (object) typeof (ulong))
            return (object) (ulong) jsobj._iValue;
          if ((object) targetType == (object) typeof (double))
            return (object) (double) jsobj._iValue;
          if ((object) targetType == (object) typeof (float))
            return (object) (float) jsobj._iValue;
          if ((object) targetType == (object) typeof (Decimal))
            return (object) (Decimal) jsobj._iValue;
          if ((object) targetType == (object) typeof (Number))
            return (object) new Number(jsobj._iValue);
          return targetType.GetTypeInfo().IsEnum ? Enum.ToObject(targetType, (object) jsobj._iValue) : (object) null;
        case JSValueType.Double:
          if (hightLoyalty)
          {
            if ((object) targetType == (object) typeof (byte))
              return (object) (byte) jsobj._dValue;
            if ((object) targetType == (object) typeof (sbyte))
              return (object) (sbyte) jsobj._dValue;
            if ((object) targetType == (object) typeof (ushort))
              return (object) (ushort) jsobj._dValue;
            if ((object) targetType == (object) typeof (short))
              return (object) (short) jsobj._dValue;
            if ((object) targetType == (object) typeof (int))
              return (object) (int) jsobj._dValue;
            if ((object) targetType == (object) typeof (uint))
              return (object) (uint) jsobj._dValue;
            if ((object) targetType == (object) typeof (long))
              return (object) (long) jsobj._dValue;
            if ((object) targetType == (object) typeof (ulong))
              return (object) (ulong) jsobj._dValue;
            if ((object) targetType == (object) typeof (Decimal))
              return (object) (Decimal) jsobj._dValue;
            if ((object) targetType == (object) typeof (string))
              return (object) Tools.DoubleToString(jsobj._dValue);
            if (targetType.GetTypeInfo().IsEnum)
              return Enum.ToObject(targetType, (object) (long) jsobj._dValue);
          }
          if ((object) targetType == (object) typeof (double))
            return (object) jsobj._dValue;
          if ((object) targetType == (object) typeof (float))
            return (object) (float) jsobj._dValue;
          return (object) targetType == (object) typeof (Number) ? (object) new Number(jsobj._dValue) : (object) null;
        case JSValueType.String:
          if (hightLoyalty)
          {
            if ((object) targetType == (object) typeof (byte))
              return (object) (byte) Tools.JSObjectToInt32(jsobj);
            if ((object) targetType == (object) typeof (sbyte))
              return (object) (sbyte) Tools.JSObjectToInt32(jsobj);
            if ((object) targetType == (object) typeof (short))
              return (object) (short) Tools.JSObjectToInt32(jsobj);
            if ((object) targetType == (object) typeof (ushort))
              return (object) (ushort) Tools.JSObjectToInt32(jsobj);
            if ((object) targetType == (object) typeof (int))
              return (object) Tools.JSObjectToInt32(jsobj);
            if ((object) targetType == (object) typeof (uint))
              return (object) (uint) Tools.JSObjectToInt64(jsobj);
            if ((object) targetType == (object) typeof (long))
              return (object) Tools.JSObjectToInt64(jsobj);
            if ((object) targetType == (object) typeof (ulong))
              return (object) (ulong) Tools.JSObjectToInt64(jsobj);
            if ((object) targetType == (object) typeof (double))
            {
              if (jsobj.Value.ToString() == "NaN")
                return (object) double.NaN;
              double d = Tools.JSObjectToDouble(jsobj);
              return !double.IsNaN(d) ? (object) d : (object) null;
            }
            if ((object) targetType == (object) typeof (float))
            {
              double d = Tools.JSObjectToDouble(jsobj);
              return !double.IsNaN(d) ? (object) (float) d : (object) null;
            }
            if ((object) targetType == (object) typeof (Decimal))
            {
              double d = Tools.JSObjectToDouble(jsobj);
              return !double.IsNaN(d) ? (object) (Decimal) d : (object) null;
            }
            if (targetType.GetTypeInfo().IsEnum)
            {
              try
              {
                return Enum.Parse(targetType, jsobj.Value.ToString());
              }
              catch
              {
                return (object) null;
              }
            }
          }
          if ((object) targetType == (object) typeof (string))
            return (object) jsobj.Value.ToString();
          return (object) targetType == (object) typeof (NiL.JS.BaseLibrary.String) ? (object) new NiL.JS.BaseLibrary.String(jsobj.Value.ToString()) : (object) null;
        case JSValueType.Symbol:
          if (hightLoyalty && (object) targetType == (object) typeof (string))
            return (object) jsobj.Value.ToString();
          return (object) targetType == (object) typeof (Symbol) ? jsobj.Value : (object) null;
        case JSValueType.Function:
          if (hightLoyalty && (object) targetType == (object) typeof (string))
            return (object) jsobj.Value.ToString();
          if (!targetType.GetTypeInfo().IsAbstract && targetType.GetTypeInfo().IsSubclassOf(typeof (Delegate)))
            return (object) (jsobj.Value as Function).MakeDelegate(targetType);
          break;
      }
      object obj = jsobj.Value;
      if (obj == null)
        return (object) null;
      if (TypeExtensions.IsAssignableFrom(targetType, obj.GetType()) || targetType.GetTypeInfo().IsEnum && Enum.IsDefined(targetType, obj))
        return obj;
      switch (obj)
      {
        case Proxy proxy when TypeExtensions.IsAssignableFrom(targetType, proxy._hostedType):
          jsobj = (JSValue) proxy.PrototypeInstance;
          return jsobj is ObjectWrapper ? jsobj.Value : (object) jsobj;
        case ConstructorProxy _ when TypeExtensions.IsAssignableFrom(typeof (Type), targetType):
          return (object) (obj as ConstructorProxy)._staticProxy._hostedType;
        case NiL.JS.BaseLibrary.Array _:
        case TypedArray _:
        case ArrayBuffer _ when TypeExtensions.IsAssignableFrom(typeof (IEnumerable), targetType):
          Type type1;
          Type type2;
          if (targetType.IsArray && (object) (type1 = targetType.GetElementType()) != null || (object) (type2 = NiL.JS.Backward.Backward.GetInterface(targetType, typeof (IEnumerable<>).Name)) != null && TypeExtensions.IsAssignableFrom(targetType, (type1 = TypeExtensions.GetGenericArguments(type2)[0]).MakeArrayType()))
          {
            if (type1.GetTypeInfo().IsPrimitive)
            {
              if ((object) type1 == (object) typeof (byte) && obj is ArrayBuffer)
                return (object) (obj as ArrayBuffer).GetData();
              if (obj is TypedArray typedArray && (object) typedArray.ElementType == (object) type1)
                return (object) typedArray.ToNativeArray();
            }
            return Tools.convertArray(obj as NiL.JS.BaseLibrary.Array, type1, hightLoyalty);
          }
          if (TypeExtensions.IsAssignableFrom(targetType, typeof (object[])))
            return Tools.convertArray(obj as NiL.JS.BaseLibrary.Array, typeof (object), hightLoyalty);
          break;
      }
      return (object) null;
    }

    private static object convertArray(NiL.JS.BaseLibrary.Array array, Type elementType, bool hightLoyalty)
    {
      if (array == null)
        return (object) null;
      IList instance = (IList) Activator.CreateInstance(elementType.MakeArrayType(), (object) (int) array._data.Length);
      int count = instance.Count;
      while (count-- > 0)
      {
        JSValue jsValue = array._data[count] ?? JSValue.undefined;
        object obj = Tools.convertJStoObj(jsValue, elementType, hightLoyalty);
        if (!hightLoyalty && obj == null && (elementType.GetTypeInfo().IsValueType || !jsValue.IsNull && !jsValue.IsUndefined()))
          return (object) null;
        instance[count] = obj;
      }
      return (object) instance;
    }

    public static string DoubleToString(double d)
    {
      if (d == 0.0)
        return "0";
      if (double.IsPositiveInfinity(d))
        return "Infinity";
      if (double.IsNegativeInfinity(d))
        return "-Infinity";
      if (double.IsNaN(d))
        return "NaN";
      string str1 = (string) null;
      lock (Tools.cachedDoubleString)
      {
        int index = 8;
        while (index-- > 0)
        {
          if (Tools.cachedDoubleString[index].key == d)
            return Tools.cachedDoubleString[index].value;
        }
        double num1 = System.Math.Abs(d);
        if (num1 < 1E-06)
          str1 = d.ToString("0.####e-0", (IFormatProvider) CultureInfo.InvariantCulture);
        else if (num1 >= 1E+21)
        {
          str1 = d.ToString("0.####e+0", (IFormatProvider) CultureInfo.InvariantCulture);
        }
        else
        {
          int num2 = d < 0.0 || d == -0.0 && Tools.IsNegativeZero(d) ? 1 : 0;
          if (num1 >= 1E+18)
          {
            str1 = ((ulong) (num1 / 1000.0)).ToString((IFormatProvider) CultureInfo.InvariantCulture) + "000";
          }
          else
          {
            ulong num3 = num1 < 1.0 ? 0UL : (ulong) num1;
            str1 = num3 == 0UL ? "0" : num3.ToString((IFormatProvider) CultureInfo.InvariantCulture);
            double num4 = num1 % 1.0;
            if (num4 != 0.0 && str1.Length <= 15)
            {
              string str2 = num4.ToString(Tools.divFormats[15 - str1.Length], (IFormatProvider) CultureInfo.InvariantCulture);
              str1 = !(str2 == "1") ? str1 + str2 : (num3 + 1UL).ToString((IFormatProvider) CultureInfo.InvariantCulture);
            }
          }
          if (num2 == 1)
            str1 = "-" + str1;
        }
        Tools.cachedDoubleString[Tools.cachedDoubleStringsIndex].key = d;
        Tools.cachedDoubleString[Tools.cachedDoubleStringsIndex].value = str1;
        Tools.cachedDoubleStringsIndex = Tools.cachedDoubleStringsIndex + 1 & 7;
      }
      return str1;
    }

    internal static void CheckEndOfInput(string code, ref int i)
    {
      if (i < code.Length)
        return;
      ExceptionHelper.ThrowSyntaxError(Strings.UnexpectedEndOfSource, code, i);
    }

    public static string Int32ToString(int value)
    {
      if (value == 0)
        return "0";
      int index = 16;
      while (index-- > 0)
      {
        if (Tools.intStringCache[index].key == value)
          return Tools.intStringCache[index].value;
      }
      Tools.intStrCacheIndex = Tools.intStrCacheIndex + 1 & 15;
      Tools.IntStringCacheItem intStringCacheItem = new Tools.IntStringCacheItem()
      {
        key = value,
        value = value.ToString((IFormatProvider) CultureInfo.InvariantCulture)
      };
      Tools.intStringCache[Tools.intStrCacheIndex] = intStringCacheItem;
      return intStringCacheItem.value;
    }

    public static bool ParseNumber(string code, out double value, int radix)
    {
      int index = 0;
      return Tools.ParseNumber(code, ref index, out value, radix, ParseNumberOptions.Default);
    }

    public static bool ParseNumber(string code, out double value, ParseNumberOptions options)
    {
      int index = 0;
      return Tools.ParseNumber(code, ref index, out value, 0, options);
    }

    public static bool ParseNumber(
      string code,
      out double value,
      int radix,
      ParseNumberOptions options)
    {
      int index = 0;
      return Tools.ParseNumber(code, ref index, out value, radix, options);
    }

    public static bool ParseNumber(string code, ref int index, out double value) => Tools.ParseNumber(code, ref index, out value, 0, ParseNumberOptions.Default);

    public static bool ParseNumber(
      string code,
      ref int index,
      out double value,
      ParseNumberOptions options)
    {
      return Tools.ParseNumber(code, ref index, out value, 0, options);
    }

    public static bool IsDigit(char c) => c >= '0' && c <= '9';

    public static bool ParseNumber(
      string code,
      ref int index,
      out double value,
      int radix,
      ParseNumberOptions options)
    {
      switch (code)
      {
        case null:
          throw new ArgumentNullException(nameof (code));
        case "":
          value = 0.0;
          return true;
        default:
          if (radix != 0 && (radix < 2 || radix > 36))
          {
            value = double.NaN;
            return false;
          }
          bool flag1 = (options & ParseNumberOptions.RaiseIfOctal) != 0;
          bool flag2 = (options & ParseNumberOptions.ProcessOctalLiteralsOldSyntax) != 0;
          bool flag3 = (options & ParseNumberOptions.AllowAutoRadix) != 0;
          bool flag4 = (options & ParseNumberOptions.AllowFloat) != 0;
          int num1 = index;
          while (num1 < code.Length && Tools.IsWhiteSpace(code[num1]) && !Tools.IsLineTerminator(code[num1]))
            ++num1;
          if (num1 >= code.Length)
          {
            value = 0.0;
            return true;
          }
          if (code.Length - num1 >= "NaN".Length && code.IndexOf("NaN", num1, "NaN".Length, StringComparison.Ordinal) == num1)
          {
            index = num1 + "NaN".Length;
            value = double.NaN;
            return true;
          }
          int num2 = 1;
          if (code[num1] == '-' || code[num1] == '+')
            num2 = 44 - (int) code[num1++];
          if (code.Length - num1 >= "Infinity".Length && code.IndexOf("Infinity", num1, "Infinity".Length, StringComparison.Ordinal) == num1)
          {
            index = num1 + "Infinity".Length;
            value = (double) num2 * double.PositiveInfinity;
            return true;
          }
          bool flag5 = false;
          if (flag3 && code[num1] == '0' && num1 + 1 < code.Length)
          {
            if (Tools.IsDigit(code[num1 + 1]))
            {
              if (flag1)
                ExceptionHelper.ThrowSyntaxError("Octal literals not allowed in strict mode", code, num1);
              while (num1 + 1 < code.Length && code[num1 + 1] == '0')
                ++num1;
              if (flag2 && num1 + 1 < code.Length && Tools.IsDigit(code[num1 + 1]))
                radix = 8;
            }
            else if ((radix == 0 || radix == 16) && (code[num1 + 1] == 'x' || code[num1 + 1] == 'X'))
            {
              num1 += 2;
              radix = 16;
            }
            else if ((radix == 0 || radix == 8) && (code[num1 + 1] == 'o' || code[num1 + 1] == 'O'))
            {
              num1 += 2;
              radix = 8;
            }
            else if ((radix == 0 || radix == 8) && (code[num1 + 1] == 'b' || code[num1 + 1] == 'B'))
            {
              num1 += 2;
              radix = 2;
            }
          }
          if (flag4 && radix == 0)
          {
            ulong num3 = 0;
            int num4 = 0;
            int num5 = 0;
            while (num1 < code.Length && Tools.IsDigit(code[num1]))
            {
              if (num4 <= 18)
              {
                num3 = num3 * 10UL + (ulong) ((int) code[num1++] - 48);
              }
              else
              {
                ++num5;
                ++num1;
              }
              ++num4;
              flag5 = true;
            }
            if (!flag5 && (num1 >= code.Length || code[num1] != '.'))
            {
              value = double.NaN;
              return false;
            }
            if (num1 < code.Length && code[num1] == '.')
            {
              ++num1;
              while (num1 < code.Length && Tools.IsDigit(code[num1]))
              {
                if (num4 <= 18 || (long) (num3 * 10UL / 10UL) == (long) num3)
                {
                  num3 = num3 * 10UL + (ulong) ((int) code[num1++] - 48);
                  --num5;
                }
                else
                  ++num1;
                ++num4;
                flag5 = true;
              }
            }
            if (!flag5)
            {
              value = double.NaN;
              return false;
            }
            if (num1 < code.Length && (code[num1] == 'e' || code[num1] == 'E'))
            {
              ++num1;
              int num6 = 0;
              int num7 = code[num1] == '+' || code[num1] == '-' ? 44 - (int) code[num1++] : 1;
              if (!Tools.IsDigit(code[num1]))
              {
                --num1;
                if (code[num1] == 'e' || code[num1] == 'E')
                  --num1;
              }
              else
              {
                int num8 = 0;
                while (num1 < code.Length && Tools.IsDigit(code[num1]))
                {
                  if (num8 <= 6)
                    num6 = num6 * 10 + ((int) code[num1++] - 48);
                  else
                    ++num1;
                }
                num5 += num6 * num7;
              }
            }
            if (num5 != 0)
            {
              int y;
              if (num5 < 0)
              {
                if (num3 != 0UL)
                {
                  for (; num3 % 10UL == 0UL && num5 < 0; num3 /= 10UL)
                    ++num5;
                }
                if (num3 == 0UL)
                {
                  value = 0.0;
                  y = 0;
                }
                else if (num5 < -18)
                {
                  ulong num9 = num3 % 1000000UL;
                  ulong num10 = num3 / 1000000UL;
                  value = (double) ((Decimal) num10 * 0.000000000001M + (Decimal) num9 * 0.000000000000000001M);
                  y = num5 + 18;
                }
                else
                {
                  ulong num11 = num3;
                  ulong num12 = num3 / (ulong) Tools.powersOf10[-num5 + 18];
                  ulong num13 = num11 - num12 * (ulong) Tools.powersOf10[-num5 + 18];
                  ulong num14 = (ulong) Tools.powersOf10[-num5 + 18];
                  int num15 = 0;
                  if (num13 != 0UL)
                  {
                    while (num13 != 0UL && num12 >> 52 == 0UL)
                    {
                      ++num15;
                      num12 <<= 1;
                      num13 <<= 1;
                      if (num13 >= num14)
                      {
                        num12 |= 1UL;
                        num13 -= num14;
                      }
                    }
                    if (num13 >= num14 >> 1)
                      ++num12;
                    for (; num12 < 4503599627370496UL; num12 <<= 1)
                      ++num15;
                  }
                  else if (num12 != 0UL)
                  {
                    while (num12 >> 52 == 0UL)
                    {
                      num12 <<= 1;
                      ++num15;
                    }
                    while (num12 > 9007199254740991UL)
                    {
                      num12 >>= 1;
                      --num15;
                    }
                  }
                  ulong num16 = num12 & 4503599627370495UL | (ulong) (1023 - num15 + 52) << 52;
                  value = BitConverter.Int64BitsToDouble((long) num16);
                  y = 0;
                }
              }
              else if (num5 > 10)
              {
                value = (double) num3 * 10000000000.0;
                y = num5 - 10;
              }
              else
              {
                ulong num17 = num3 % 10000UL;
                int num18 = num5 + 4;
                ulong num19 = num3 / 10000UL;
                value = (double) ((Decimal) num19 * Tools.powersOf10[num18 + 18]) + (double) ((Decimal) num17 * Tools.powersOf10[num18 + 18 - 4]);
                y = 0;
              }
              switch (y)
              {
                case -324:
                  value *= 9.8813129168249309E-324;
                  value *= 0.1;
                  break;
                case 0:
                  break;
                default:
                  double num20 = System.Math.Pow(10.0, (double) y);
                  value *= num20;
                  break;
              }
            }
            else
              value = (double) num3;
            value *= (double) num2;
            index = num1;
            return true;
          }
          if (radix == 0)
            radix = 10;
          value = 0.0;
          bool flag6 = false;
          double num21 = 0.0;
          ulong num22 = 0;
          for (; num1 < code.Length; ++num1)
          {
            int index1 = Tools.hexCharToInt(code[num1]);
            if (index1 < radix && ((int) Tools.NumChars[index1] == (int) code[num1] || (int) Tools.NumChars[index1] + 32 == (int) code[num1]))
            {
              if (flag6)
              {
                num21 = num21 * (double) radix + (double) index1;
              }
              else
              {
                num22 = num22 * (ulong) radix + (ulong) index1;
                if (((long) num22 & -144115188075855872L) != 0L)
                {
                  flag6 = true;
                  num21 = (double) num22;
                }
              }
              flag5 = true;
            }
            else
              break;
          }
          if (!flag5)
          {
            value = double.NaN;
            return false;
          }
          value = flag6 ? num21 : (double) num22;
          value *= (double) num2;
          index = num1;
          return true;
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsNegativeZero(double d) => (BitConverter.DoubleToInt64Bits(d) & -9218868437227405313L) == long.MinValue;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Unescape(string code, bool strict) => Tools.Unescape(code, strict, true, false, true);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Unescape(
      string code,
      bool strict,
      bool processUnknown,
      bool processRegexComp)
    {
      return Tools.Unescape(code, strict, processUnknown, processRegexComp, true);
    }

    public static string Unescape(
      string code,
      bool strict,
      bool processUnknown,
      bool processRegexComp,
      bool fullUnicode)
    {
      switch (code)
      {
        case null:
          throw new ArgumentNullException(nameof (code));
        case "":
          return code;
        default:
          StringBuilder stringBuilder = (StringBuilder) null;
          for (int index1 = 0; index1 < code.Length; ++index1)
          {
            if (code[index1] == '\\' && index1 + 1 < code.Length)
            {
              if (stringBuilder == null)
              {
                stringBuilder = new StringBuilder(code.Length);
                for (int index2 = 0; index2 < index1; ++index2)
                  stringBuilder.Append(code[index2]);
              }
              ++index1;
              switch (code[index1])
              {
                case '\n':
                  continue;
                case '\r':
                  if (code.Length > index1 + 1 && code[index1 + 1] == '\n')
                  {
                    ++index1;
                    continue;
                  }
                  continue;
                case 'C':
                case 'c':
                  if (processRegexComp)
                  {
                    if (index1 + 1 < code.Length)
                    {
                      char ch = code[index1 + 1];
                      if (ch >= 'a' && ch <= 'z')
                        ch -= ' ';
                      if ((ushort) ((uint) ch - 64U) < (ushort) 32)
                      {
                        stringBuilder.Append("\\c");
                        stringBuilder.Append(ch);
                        ++index1;
                        continue;
                      }
                      goto case 'K';
                    }
                    else
                      goto case 'K';
                  }
                  else
                    break;
                case 'K':
                case 'P':
                case 'k':
                case 'p':
                  if (processRegexComp)
                  {
                    stringBuilder.Append("\\b\\B");
                    continue;
                  }
                  break;
                case 'b':
                  stringBuilder.Append(processRegexComp ? "\\b" : "\b");
                  continue;
                case 'f':
                  stringBuilder.Append(processRegexComp ? "\\f" : "\f");
                  continue;
                case 'n':
                  stringBuilder.Append(processRegexComp ? "\\n" : "\n");
                  continue;
                case 'r':
                  stringBuilder.Append(processRegexComp ? "\\r" : "\r");
                  continue;
                case 't':
                  stringBuilder.Append(processRegexComp ? "\\t" : "\t");
                  continue;
                case 'u':
                case 'x':
                  if (index1 + (code[index1] == 'u' ? 5 : 3) > code.Length)
                  {
                    if (processRegexComp)
                    {
                      stringBuilder.Append(code[index1]);
                      continue;
                    }
                    ExceptionHelper.ThrowSyntaxError("Invalid escape code (\"" + code + "\")");
                  }
                  if (fullUnicode && code[index1] == 'u' && code[index1 + 1] == '{')
                  {
                    int num1 = code.IndexOf('}', index1 + 2);
                    if (num1 == -1)
                      ExceptionHelper.Throw((Error) new SyntaxError("Invalid escape sequence"));
                    string s = code.Substring(index1 + 2, num1 - index1 - 2);
                    uint result = 0;
                    if (uint.TryParse(s, NumberStyles.HexNumber, (IFormatProvider) null, out result))
                    {
                      if (result <= (uint) ushort.MaxValue)
                        stringBuilder.Append((char) result);
                      else if (result <= 1114111U)
                      {
                        uint num2 = result - 65536U;
                        char ch1 = (char) ((num2 >> 10) + 55296U);
                        char ch2 = (char) (num2 % 1024U + 56320U);
                        stringBuilder.Append(ch1).Append(ch2);
                      }
                      else
                        ExceptionHelper.Throw((Error) new SyntaxError("Invalid escape sequence '\\u{" + s + "}'"));
                      index1 += s.Length + 2;
                      continue;
                    }
                    if (processRegexComp)
                    {
                      stringBuilder.Append(code[index1]);
                      continue;
                    }
                    ExceptionHelper.Throw((Error) new SyntaxError("Invalid escape sequence '\\u{" + s + "}'"));
                    continue;
                  }
                  string s1 = code.Substring(index1 + 1, code[index1] == 'u' ? 4 : 2);
                  ushort result1 = 0;
                  if (ushort.TryParse(s1, NumberStyles.HexNumber, (IFormatProvider) null, out result1))
                  {
                    char ch = (char) result1;
                    stringBuilder.Append(ch);
                    index1 += s1.Length;
                    continue;
                  }
                  if (processRegexComp)
                  {
                    stringBuilder.Append(code[index1]);
                    continue;
                  }
                  ExceptionHelper.Throw((Error) new SyntaxError("Invalid escape sequence '\\" + code[index1].ToString() + s1 + "'"));
                  continue;
                case 'v':
                  stringBuilder.Append(processRegexComp ? "\\v" : "\v");
                  continue;
              }
              if (!processRegexComp && code[index1] >= '0' && code[index1] <= '7')
              {
                if (strict && (code[index1] != '0' || code.Length > index1 + 1 && code[index1 + 1] >= '0' && code[index1 + 1] <= '7'))
                  ExceptionHelper.Throw((Error) new SyntaxError("Octal literals are not allowed in strict mode."));
                int num = (int) code[index1] - 48;
                if (index1 + 1 < code.Length && code[index1 + 1] >= '0' && code[index1 + 1] <= '7')
                  num = num * 8 + ((int) code[++index1] - 48);
                if (index1 + 1 < code.Length && code[index1 + 1] >= '0' && code[index1 + 1] <= '7')
                  num = num * 8 + ((int) code[++index1] - 48);
                stringBuilder.Append((char) num);
              }
              else if (processUnknown)
              {
                stringBuilder.Append(code[index1]);
              }
              else
              {
                stringBuilder.Append('\\');
                stringBuilder.Append(code[index1]);
              }
            }
            else
              stringBuilder?.Append(code[index1]);
          }
          return ((object) stringBuilder ?? (object) code).ToString();
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string UnescapeNextChar(
      string code,
      int index,
      out int processedChars,
      bool strict)
    {
      return Tools.UnescapeNextChar(code, index, out processedChars, strict, true, false, true);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string UnescapeNextChar(
      string code,
      int index,
      out int processedChars,
      bool strict,
      bool processUnknown,
      bool processRegexComp)
    {
      return Tools.UnescapeNextChar(code, index, out processedChars, strict, processUnknown, processRegexComp, true);
    }

    public static string UnescapeNextChar(
      string code,
      int index,
      out int processedChars,
      bool strict,
      bool processUnknown,
      bool processRegexComp,
      bool fullUnicode)
    {
      processedChars = 0;
      switch (code)
      {
        case null:
          throw new ArgumentNullException(nameof (code));
        case "":
          return code;
        default:
          if (index >= code.Length)
            return "";
          int index1 = index;
          if (code[index1] == '\\' && index1 + 1 < code.Length)
          {
            int index2 = index1 + 1;
            processedChars = 2;
            switch (code[index2])
            {
              case '\n':
                return "";
              case '\r':
                if (code.Length > index2 + 1 && code[index2] == '\n')
                  processedChars = 3;
                return "";
              case 'C':
              case 'c':
                if (processRegexComp)
                {
                  if (index2 + 1 < code.Length)
                  {
                    char ch = code[index2 + 1];
                    if (ch >= 'a' && ch <= 'z')
                      ch -= ' ';
                    if ((ushort) ((uint) ch - 64U) < (ushort) 32)
                    {
                      ++processedChars;
                      return "\\c" + ch.ToString();
                    }
                    goto case 'K';
                  }
                  else
                    goto case 'K';
                }
                else
                  break;
              case 'K':
              case 'P':
              case 'k':
              case 'p':
                if (processRegexComp)
                  return "\\b\\B";
                break;
              case 'b':
                return !processRegexComp ? "\b" : "\\b";
              case 'f':
                return !processRegexComp ? "\f" : "\\f";
              case 'n':
                return !processRegexComp ? "\n" : "\\n";
              case 'r':
                return !processRegexComp ? "\r" : "\\r";
              case 't':
                return !processRegexComp ? "\t" : "\\t";
              case 'u':
              case 'x':
                if (index2 + (code[index2] == 'u' ? 5 : 3) > code.Length)
                {
                  if (processRegexComp)
                    return code[index2].ToString();
                  ExceptionHelper.Throw((Error) new SyntaxError("Invalid escape code (\"" + code + "\")"));
                }
                if (code[index2] == 'u' && code[index2 + 1] == '{')
                {
                  if (!fullUnicode)
                    return code[index2].ToString();
                  int num = code.IndexOf('}', index2 + 2);
                  if (num == -1)
                    ExceptionHelper.Throw((Error) new SyntaxError("Invalid escape sequence"));
                  string s = code.Substring(index2 + 2, num - index2 - 2);
                  uint result = 0;
                  if (uint.TryParse(s, NumberStyles.HexNumber, (IFormatProvider) null, out result))
                  {
                    processedChars += s.Length + 2;
                    if (result <= (uint) ushort.MaxValue)
                      return ((char) result).ToString();
                    if (result <= 1114111U)
                    {
                      result -= 65536U;
                      char ch1 = (char) ((result >> 10) + 55296U);
                      char ch2 = (char) (result % 1024U + 56320U);
                      return ch1.ToString() + ch2.ToString();
                    }
                    ExceptionHelper.Throw((Error) new SyntaxError("Invalid escape sequence '\\u{" + s + "}'"));
                  }
                  else
                  {
                    if (processRegexComp)
                      return code[index2].ToString();
                    ExceptionHelper.Throw((Error) new SyntaxError("Invalid escape sequence '\\u{" + s + "}'"));
                  }
                }
                else
                {
                  string s = code.Substring(index2 + 1, code[index2] == 'u' ? 4 : 2);
                  ushort result = 0;
                  if (ushort.TryParse(s, NumberStyles.HexNumber, (IFormatProvider) null, out result))
                  {
                    processedChars += s.Length;
                    return ((char) result).ToString();
                  }
                  if (processRegexComp)
                    return code[index2].ToString();
                  ExceptionHelper.Throw((Error) new SyntaxError("Invalid escape sequence '\\" + code[index2].ToString() + s + "'"));
                }
                return code[index2].ToString();
              case 'v':
                return !processRegexComp ? "\v" : "\\v";
            }
            if (code[index2] >= '0' && code[index2] <= '7' && !processRegexComp)
            {
              if (strict)
                ExceptionHelper.Throw((Error) new SyntaxError("Octal literals are not allowed in strict mode."));
              int num1 = (int) code[index2] - 48;
              if (index2 + 1 < code.Length && code[index2 + 1] >= '0' && code[index2 + 1] <= '7')
              {
                num1 = num1 * 8 + ((int) code[++index2] - 48);
                ++processedChars;
              }
              if (index2 + 1 < code.Length && code[index2 + 1] >= '0' && code[index2 + 1] <= '7')
              {
                int num2;
                num1 = num1 * 8 + ((int) code[num2 = index2 + 1] - 48);
                ++processedChars;
              }
              return ((char) num1).ToString();
            }
            return !processUnknown ? "\\" + code[index2].ToString() : code[index2].ToString();
          }
          processedChars = 1;
          return code[index1].ToString();
      }
    }

    internal static int NextCodePoint(string str, ref int i) => str[i] >= '\uD800' && str[i] <= '\uDBFF' && i + 1 < str.Length && str[i + 1] >= '\uDC00' && str[i + 1] <= '\uDFFF' ? ((int) str[i] - 55296) * 1024 + ((int) str[++i] - 56320) + 65536 : (int) str[i];

    internal static int NextCodePoint(string str, ref int i, bool regexp)
    {
      if (str[i] >= '\uD800' && str[i] <= '\uDBFF' && i + 1 < str.Length && str[i + 1] >= '\uDC00' && str[i + 1] <= '\uDFFF')
        return ((int) str[i] - 55296) * 1024 + ((int) str[++i] - 56320) + 65536;
      if (!regexp || str[i] != '\\' || i + 1 >= str.Length)
        return (int) str[i];
      ++i;
      if (i + 1 < str.Length && str[i] == 'c' && str[i + 1] >= 'A' && str[i + 1] <= 'Z')
      {
        ++i;
        return (int) str[i] - 64;
      }
      if (str[i] >= '0' && str[i] <= '7')
      {
        int num = (int) str[i] - 48;
        if (i + 1 < str.Length && Tools.IsDigit(str[i + 1]))
          num = num * 8 + ((int) str[++i] - 48);
        if (i + 1 < str.Length && Tools.IsDigit(str[i + 1]))
          num = num * 8 + ((int) str[++i] - 48);
        return num;
      }
      if (str[i] == 't')
        return 9;
      if (str[i] == 'f')
        return 12;
      if (str[i] == 'v')
        return 11;
      if (str[i] == 'b')
        return 8;
      if (str[i] == 'n')
        return 10;
      return str[i] == 'r' ? 13 : Tools.NextCodePoint(str, ref i);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsSurrogatePair(string str, int i) => i >= 0 && i + 1 < str.Length && str[i] >= '\uD800' && str[i] <= '\uDBFF' && str[i + 1] >= '\uDC00' && str[i + 1] <= '\uDFFF';

    internal static string CodePointToString(int codePoint)
    {
      if (codePoint < 0 || codePoint > 1114111)
        ExceptionHelper.Throw((Error) new RangeError("Invalid code point " + codePoint.ToString()));
      if (codePoint <= (int) ushort.MaxValue)
        return ((char) codePoint).ToString();
      codePoint -= 65536;
      char ch1 = (char) ((codePoint >> 10) + 55296);
      char ch2 = (char) (codePoint % 1024 + 56320);
      return ch1.ToString() + ch2.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsLineTerminator(char c) => c == '\n' || c == '\r' || c == '\u2028' || c == '\u2029';

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool isHex(char p)
    {
      if (p < '0' || p > 'f')
        return false;
      int num = Tools.hexCharToInt(p);
      return num >= 0 && num < 16;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int hexCharToInt(char p) => ((int) p % 97 % 65 + 10) % 58;

    internal static long getLengthOfArraylike(JSValue src, bool reassignLen)
    {
      JSValue property = src.GetProperty("length", true, PropertyScope.Common);
      uint int64 = (uint) Tools.JSObjectToInt64(Tools.InvokeGetter(property, src).ToPrimitiveValue_Value_String(), 0L, false);
      if (reassignLen)
      {
        if (property._valueType == JSValueType.Property)
          ((property._oValue as PropertyPair).setter ?? Function.Empty).Call(src, new Arguments()
          {
            (JSValue) (long) int64
          });
        else
          property.Assign((JSValue) (long) int64);
      }
      return (long) int64;
    }

    internal static NiL.JS.BaseLibrary.Array arraylikeToArray(
      JSValue src,
      bool evalProps,
      bool clone,
      bool reassignLen,
      long _length)
    {
      NiL.JS.BaseLibrary.Array array1 = new NiL.JS.BaseLibrary.Array();
      bool flag1 = true;
      while (flag1)
      {
        bool flag2 = false;
        if (src is NiL.JS.BaseLibrary.Array array2)
        {
          if (_length == -1L)
            _length = (long) array2._data.Length;
          long num = -1;
          foreach (KeyValuePair<int, JSValue> keyValuePair in array2._data.DirectOrder)
          {
            if ((long) keyValuePair.Key < _length)
            {
              JSValue jsValue = keyValuePair.Value;
              if (jsValue != null && jsValue.Exists)
              {
                if (!flag2 && System.Math.Abs(num - (long) keyValuePair.Key) > 1L)
                  flag2 = true;
                if (evalProps && jsValue._valueType == JSValueType.Property)
                  jsValue = (jsValue._oValue as PropertyPair).getter == null ? JSValue.undefined : (jsValue._oValue as PropertyPair).getter.Call(src, (Arguments) null).CloneImpl(false);
                else if (clone)
                  jsValue = jsValue.CloneImpl(false);
                if (array1._data[keyValuePair.Key] == null)
                  array1._data[keyValuePair.Key] = jsValue;
              }
            }
            else
              break;
          }
          flag1 = flag2 | System.Math.Abs(num - _length) > 1L;
        }
        else
        {
          if (_length == -1L)
          {
            _length = Tools.getLengthOfArraylike(src, reassignLen);
            if (_length == 0L)
              return array1;
          }
          long num = -1;
          foreach (KeyValuePair<uint, JSValue> keyValuePair in Tools.EnumerateArraylike(_length, src))
          {
            JSValue jsValue = keyValuePair.Value;
            if (jsValue.Exists)
            {
              if (evalProps && jsValue._valueType == JSValueType.Property)
                jsValue = (jsValue._oValue as PropertyPair).getter == null ? JSValue.undefined : (jsValue._oValue as PropertyPair).getter.Call(src, (Arguments) null).CloneImpl(false);
              else if (clone)
                jsValue = jsValue.CloneImpl(false);
              if (!flag2 && System.Math.Abs(num - (long) keyValuePair.Key) > 1L)
                flag2 = true;
              if (array1._data[(int) keyValuePair.Key] == null)
                array1._data[(int) keyValuePair.Key] = jsValue;
            }
          }
          flag1 = flag2 | System.Math.Abs(num - _length) > 1L;
        }
        if (src.__proto__ != JSValue.@null)
        {
          if (!(src.__proto__._oValue is JSValue jsValue))
            jsValue = (JSValue) src.__proto__;
          src = jsValue;
          if (src == null || src._valueType >= JSValueType.String && src._oValue == null)
            break;
        }
        else
          break;
      }
      array1._data[(int) (_length - 1L)] = array1._data[(int) (_length - 1L)];
      return array1;
    }

    internal static IEnumerable<KeyValuePair<uint, JSValue>> EnumerateArraylike(
      long length,
      JSValue src)
    {
      if (src._valueType == JSValueType.Object && src.Value is NiL.JS.BaseLibrary.Array)
      {
        foreach (KeyValuePair<int, JSValue> keyValuePair in (src.Value as NiL.JS.BaseLibrary.Array)._data.DirectOrder)
          yield return new KeyValuePair<uint, JSValue>((uint) keyValuePair.Key, keyValuePair.Value);
      }
      IEnumerator<KeyValuePair<string, JSValue>> @enum = src.GetEnumerator(false, EnumerationMode.RequireValues);
      while (@enum.MoveNext())
      {
        KeyValuePair<string, JSValue> current = @enum.Current;
        string key1 = current.Key;
        int index = 0;
        double num1 = 0.0;
        uint num2;
        if (Tools.ParseNumber(key1, ref index, out num1) && index == key1.Length && num1 < (double) length && (double) (num2 = (uint) num1) == num1)
        {
          int key2 = (int) num2;
          current = @enum.Current;
          JSValue jsValue = current.Value;
          yield return new KeyValuePair<uint, JSValue>((uint) key2, jsValue);
        }
      }
    }

    internal static int CompareWithMask(Enum x, Enum y, Enum mask) => ((int) x & (int) mask) - ((int) y & (int) mask);

    internal static bool IsEqual(Enum x, Enum y, Enum mask) => ((int) x & (int) mask) == ((int) y & (int) mask);

    internal static JSValue InvokeGetter(JSValue property, JSValue target)
    {
      if (property._valueType != JSValueType.Property)
        return property;
      if (!(property._oValue is PropertyPair oValue) || oValue.getter == null)
        return JSValue.undefined;
      property = oValue.getter.Call(target, (Arguments) null);
      if (property._valueType < JSValueType.Undefined)
        property = JSValue.undefined;
      return property;
    }

    internal static JSValue EvalExpressionSafe(Context context, NiL.JS.Expressions.Expression source)
    {
      JSValue jsValue = source.Evaluate(context);
      if (jsValue == null)
        return JSValue.undefined;
      if (jsValue._valueType != JSValueType.SpreadOperatorResult)
      {
        jsValue = jsValue.CloneImpl(false, JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.NonConfigurable | JSValueAttributesInternal.SystemObject | JSValueAttributesInternal.ProxyPrototype | JSValueAttributesInternal.Temporary | JSValueAttributesInternal.Reassign);
        jsValue._attributes |= JSValueAttributesInternal.Cloned;
      }
      return jsValue;
    }

    internal static bool IsWhiteSpace(char p)
    {
      switch ((int) p >> 8)
      {
        case 0:
        case 22:
        case 24:
        case 32:
        case 48:
        case 254:
          for (int index = 0; index < Tools.TrimChars.Length; ++index)
          {
            if ((int) p == (int) Tools.TrimChars[index])
              return true;
          }
          return false;
        default:
          return false;
      }
    }

    internal static Arguments CreateArguments(NiL.JS.Expressions.Expression[] arguments, Context initiator)
    {
      Arguments arguments1 = new Arguments(initiator);
      IList<JSValue> jsValueList = (IList<JSValue>) null;
      int index1 = 0;
      int index2 = 0;
      int index3 = 0;
      while (index2 < arguments.Length)
      {
        if (jsValueList != null)
        {
          if (index3 < jsValueList.Count)
          {
            arguments1[index1++] = jsValueList[index3];
            ++index3;
          }
          if (index3 == jsValueList.Count)
          {
            jsValueList = (IList<JSValue>) null;
            ++index2;
          }
        }
        else
        {
          JSValue jsValue = Tools.EvalExpressionSafe(initiator, arguments[index2]);
          if (jsValue._valueType == JSValueType.SpreadOperatorResult)
          {
            index3 = 0;
            jsValueList = jsValue._oValue as IList<JSValue>;
          }
          else
          {
            ++index2;
            arguments1[index1] = jsValue;
            ++index1;
          }
        }
      }
      arguments1._iValue = index1;
      return arguments1;
    }

    internal static LambdaExpression BuildJsCallTree(
      string name,
      System.Linq.Expressions.Expression functionGetter,
      ParameterExpression thisParameter,
      MethodInfo method,
      Type delegateType)
    {
      ParameterInfo[] parameters1 = method.GetParameters();
      ParameterExpression[] parameters2 = new ParameterExpression[parameters1.Length + (thisParameter != null ? 1 : 0)];
      int index1 = 0;
      if (thisParameter != null)
        parameters2[index1++] = thisParameter;
      for (; index1 < parameters1.Length; ++index1)
        parameters2[index1] = System.Linq.Expressions.Expression.Parameter(parameters1[index1].ParameterType, parameters1[index1].Name);
      ParameterExpression parameterExpression = System.Linq.Expressions.Expression.Parameter(typeof (Arguments), "arguments");
      List<System.Linq.Expressions.Expression> expressionList = new List<System.Linq.Expressions.Expression>();
      if (parameters1.Length != 0)
      {
        expressionList.Add((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Assign((System.Linq.Expressions.Expression) parameterExpression, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.New(typeof (Arguments))));
        for (int index2 = 0; index2 < parameters2.Length; ++index2)
        {
          System.Linq.Expressions.Expression expression = (System.Linq.Expressions.Expression) parameters2[index2];
          if (expression.Type.GetTypeInfo().IsValueType)
            expression = (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Convert(expression, typeof (object));
          GlobalContext currentGlobalContext = Context.CurrentGlobalContext;
          expressionList.Add((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) parameterExpression, typeof (Arguments).GetRuntimeMethod("Add", new Type[1]
          {
            typeof (JSValue)
          }), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) currentGlobalContext), Tools.methodof<object, JSValue>(new Func<object, JSValue>(currentGlobalContext.ProxyValue)), expression)));
        }
      }
      MethodCallExpression methodCallExpression = System.Linq.Expressions.Expression.Call(functionGetter, typeof (Function).GetRuntimeMethod("Call", new Type[1]
      {
        typeof (Arguments)
      }), (System.Linq.Expressions.Expression) parameterExpression);
      expressionList.Add((System.Linq.Expressions.Expression) methodCallExpression);
      if ((object) method.ReturnParameter.ParameterType != (object) typeof (void) && (object) method.ReturnParameter.ParameterType != (object) typeof (object) && !TypeExtensions.IsAssignableFrom(typeof (JSValue), method.ReturnParameter.ParameterType))
      {
        MethodInfo method1 = typeof (JSValueExtensions).GetRuntimeMethods().First<MethodInfo>((Func<MethodInfo, bool>) (x => x.Name == "As")).MakeGenericMethod(method.ReturnParameter.ParameterType);
        expressionList[expressionList.Count - 1] = (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call(method1, (System.Linq.Expressions.Expression) methodCallExpression);
      }
      BlockExpression body = System.Linq.Expressions.Expression.Block((IEnumerable<ParameterExpression>) new ParameterExpression[1]
      {
        parameterExpression
      }, (IEnumerable<System.Linq.Expressions.Expression>) expressionList);
      return (object) delegateType != null ? System.Linq.Expressions.Expression.Lambda(delegateType, (System.Linq.Expressions.Expression) body, name, (IEnumerable<ParameterExpression>) parameters2) : System.Linq.Expressions.Expression.Lambda((System.Linq.Expressions.Expression) body, name, (IEnumerable<ParameterExpression>) parameters2);
    }

    internal static MethodInfo methodof<T0, T1, T2, T3>(Action<T0, T1, T2, T3> method) => method.GetMethodInfo();

    internal static MethodInfo methodof<T0, T1>(Func<T0, T1> method) => method.GetMethodInfo();

    public static string GetTypeName(JSValue v)
    {
      if (v == null)
        return "null";
      switch (v._valueType)
      {
        case JSValueType.NotExists:
        case JSValueType.NotExistsInObject:
        case JSValueType.Undefined:
          return "undefined";
        case JSValueType.Boolean:
          return "Boolean";
        case JSValueType.Integer:
        case JSValueType.Double:
          return "Number";
        case JSValueType.String:
          return "String";
        case JSValueType.Symbol:
          return "Symbol";
        case JSValueType.Object:
          if (!(v is ObjectWrapper objectWrapper))
            objectWrapper = v._oValue as ObjectWrapper;
          if (objectWrapper != null)
            return objectWrapper.Value.GetType().Name;
          if (v._oValue == null)
            return "null";
          if (v._oValue is GlobalObject)
            return "global";
          if (v._oValue is Proxy)
          {
            Type hostedType = (v._oValue as Proxy)._hostedType;
            return (object) hostedType == (object) typeof (JSObject) ? "Object" : hostedType.Name;
          }
          return (object) v.Value.GetType() == (object) typeof (JSObject) ? "Object" : v.Value.GetType().Name;
        case JSValueType.Function:
          return "Function";
        case JSValueType.Date:
          return "Date";
        case JSValueType.Property:
          if (!(v._oValue is PropertyPair oValue))
            return "Property";
          string str = "";
          if (oValue.getter != null)
            str += "Getter";
          if (oValue.setter != null)
            str = str + (str.Length > 0 ? "/" : "") + "Setter";
          if (str.Length == 0)
            str = "Invalid";
          return str + " Property";
        default:
          throw new NotImplementedException();
      }
    }

    public static string JSValueToObjectString(JSValue v) => Tools.JSValueToObjectString(v, 1, 0);

    public static string JSValueToObjectString(JSValue v, int maxRecursionDepth) => Tools.JSValueToObjectString(v, maxRecursionDepth, 0);

    internal static string JSValueToObjectString(
      JSValue v,
      int maxRecursionDepth,
      int recursionDepth = 0)
    {
      if (v == null)
        return "null";
      switch (v.ValueType)
      {
        case JSValueType.String:
          return "\"" + v.ToString() + "\"";
        case JSValueType.Object:
          if (v._oValue == null)
            return "null";
          if (v.Value is RegExp)
            return v.ToString();
          if (v.Value is NiL.JS.BaseLibrary.Array || v.Value == Context.CurrentGlobalContext.GetPrototype(typeof (NiL.JS.BaseLibrary.Array)) || v == Context.CurrentGlobalContext.GetPrototype(typeof (NiL.JS.BaseLibrary.Array)))
          {
            if (!(v.Value is NiL.JS.BaseLibrary.Array array))
            {
              StringBuilder stringBuilder = new StringBuilder("Array [ ");
              int num = 0;
              IEnumerator<KeyValuePair<string, JSValue>> enumerator = v.GetEnumerator(true, EnumerationMode.RequireValues);
              while (enumerator.MoveNext())
              {
                if (num++ > 0)
                  stringBuilder.Append(", ");
                stringBuilder.Append(enumerator.Current.Key).Append(": ");
                stringBuilder.Append(Tools.JSValueToObjectString(enumerator.Current.Value, maxRecursionDepth, recursionDepth + 1));
              }
              stringBuilder.Append(" ]");
              return stringBuilder.ToString();
            }
            long length = (long) array.length;
            if (recursionDepth >= maxRecursionDepth)
              return string.Format("Array[{0}]", (object) length);
            StringBuilder stringBuilder1 = new StringBuilder(string.Format("Array ({0}) [ ", (object) length));
            int num1 = 0;
            int index;
            for (index = 0; (long) index < length; ++index)
            {
              JSValue v1 = array[index];
              if (num1 > 1)
              {
                if (v1 != null && v1._valueType <= JSValueType.Undefined)
                {
                  ++num1;
                }
                else
                {
                  stringBuilder1.Append(" x ").Append(num1);
                  stringBuilder1.Append(", ");
                  stringBuilder1.Append(Tools.JSValueToObjectString(v1, maxRecursionDepth, recursionDepth + 1));
                  num1 = 0;
                }
              }
              else
              {
                if (v1 != null && v1._valueType <= JSValueType.Undefined)
                {
                  if (++num1 > 1)
                    continue;
                }
                else
                  num1 = 0;
                if (index > 0)
                  stringBuilder1.Append(", ");
                stringBuilder1.Append(Tools.JSValueToObjectString(v1, maxRecursionDepth, recursionDepth + 1));
              }
            }
            if (num1 > 0)
              stringBuilder1.Append(" x ").Append(num1);
            if (array._fields != null)
            {
              IEnumerator<KeyValuePair<string, JSValue>> enumerator = array._fields.GetEnumerator();
              while (enumerator.MoveNext())
              {
                if (index++ > 0)
                  stringBuilder1.Append(", ");
                stringBuilder1.Append(enumerator.Current.Key).Append(": ");
                stringBuilder1.Append(Tools.JSValueToObjectString(enumerator.Current.Value, maxRecursionDepth, recursionDepth + 1));
              }
            }
            stringBuilder1.Append(" ]");
            return stringBuilder1.ToString();
          }
          string typeName = Tools.GetTypeName(v);
          if (recursionDepth >= maxRecursionDepth)
            return typeName;
          StringBuilder stringBuilder2 = new StringBuilder(typeName);
          stringBuilder2.Append(" { ");
          if (!(v is JSObject jsObject))
            jsObject = v._oValue as JSObject;
          if (jsObject == null)
            return v.ToString();
          int num2 = 0;
          IEnumerator<KeyValuePair<string, JSValue>> enumerator1 = jsObject.GetEnumerator(true, EnumerationMode.RequireValues);
          while (enumerator1.MoveNext())
          {
            if (num2++ > 0)
              stringBuilder2.Append(", ");
            stringBuilder2.Append(enumerator1.Current.Key).Append(": ");
            stringBuilder2.Append(Tools.JSValueToObjectString(enumerator1.Current.Value, maxRecursionDepth, recursionDepth + 1));
          }
          stringBuilder2.Append(" }");
          return stringBuilder2.ToString();
        case JSValueType.Function:
          if (!(v.Value is Function function))
            return v.ToString();
          if (recursionDepth >= maxRecursionDepth)
            return function.name + "()";
          return recursionDepth == maxRecursionDepth - 1 ? function.ToString(true) : function.ToString();
        case JSValueType.Date:
          string str = v.ToString();
          return str == "Invalid date" ? str : "Date " + str;
        default:
          return v.ToString();
      }
    }

    internal static string JSValueToString(JSValue v)
    {
      if (v == null)
        return "null";
      if (v.ValueType == JSValueType.Object)
      {
        if (v._oValue == null)
          return "null";
        if (!(v is ObjectWrapper objectWrapper))
          objectWrapper = v._oValue as ObjectWrapper;
        if (objectWrapper != null)
          return objectWrapper.Value.ToString();
      }
      return v.ToString();
    }

    internal static string FormatArgs(IEnumerable args)
    {
      if (args == null)
        return (string) null;
      IEnumerable enumerable = args;
      if (args is IEnumerable<KeyValuePair<string, JSValue>>)
        enumerable = (IEnumerable) (args as IEnumerable<KeyValuePair<string, JSValue>>).Select<KeyValuePair<string, JSValue>, JSValue>((Func<KeyValuePair<string, JSValue>, JSValue>) (x => x.Value));
      IEnumerator enumerator = enumerable.GetEnumerator();
      if (!enumerator.MoveNext())
        return (string) null;
      object current1 = enumerator.Current;
      JSValue v1 = current1 as JSValue;
      string str1 = (string) null;
      if (str1 == null && current1 != null && current1 is string)
        str1 = v1.ToString();
      if (str1 == null && v1 != null && v1.ValueType == JSValueType.String)
        str1 = v1.ToString();
      bool flag1 = enumerator.MoveNext();
      StringBuilder stringBuilder = new StringBuilder();
      if (str1 != null & flag1)
      {
        int startIndex = 0;
        while (startIndex < str1.Length & flag1)
        {
          object current2;
          JSValue v2 = (current2 = enumerator.Current) as JSValue;
          bool flag2 = false;
          int num = str1.IndexOf('%', startIndex);
          if (num >= 0 && num != str1.Length - 1)
          {
            if (num > 0)
              stringBuilder.Append(str1.Substring(startIndex, num - startIndex));
            startIndex = num;
            bool flag3 = false;
            int length = 2;
            char ch = str1[startIndex + 1];
            int val2 = -1;
            if (ch == '.')
            {
              while (startIndex + length < str1.Length && Tools.IsDigit(str1[startIndex + length]))
                ++length;
              if (startIndex + length != str1.Length)
              {
                if (length > 12)
                {
                  val2 = int.MaxValue;
                }
                else
                {
                  long result = -1;
                  if (length > 2 && long.TryParse(str1.Substring(startIndex + 2, length - 2), out result))
                    val2 = (int) System.Math.Min(result, (long) int.MaxValue);
                }
                if (length > 2)
                  ch = str1[startIndex + length++];
              }
              else
                break;
            }
            double d;
            if (ch <= 'd')
            {
              if (ch != '%')
              {
                if (ch != 'O')
                {
                  if (ch == 'd')
                    goto label_45;
                  else
                    goto label_66;
                }
              }
              else if (length == 2)
              {
                stringBuilder.Append('%');
                goto label_67;
              }
              else
              {
                flag3 = true;
                goto label_67;
              }
            }
            else if (ch <= 'i')
            {
              if (ch != 'f')
              {
                if (ch == 'i')
                  goto label_45;
                else
                  goto label_66;
              }
              else
              {
                d = double.NaN;
                if (v2 != null)
                  d = (double) Tools.JSObjectToNumber(v2);
                else if (!Tools.ParseNumber((current2 ?? (object) "null").ToString(), out d, 0))
                  d = double.NaN;
                if (val2 >= 0)
                  d = System.Math.Round(d, System.Math.Min(15, val2));
                stringBuilder.Append(Tools.DoubleToString(d));
                flag2 = true;
                goto label_67;
              }
            }
            else if (ch != 'o')
            {
              if (ch == 's')
              {
                if (v2 != null)
                  stringBuilder.Append(Tools.JSValueToString(v2));
                else
                  stringBuilder.Append((current2 ?? (object) "null").ToString());
                flag2 = true;
                goto label_67;
              }
              else
                goto label_66;
            }
            int maxRecursionDepth = ch == 'o' ? 1 : 2;
            if (v2 != null)
            {
              stringBuilder.Append(Tools.JSValueToObjectString(v2, maxRecursionDepth));
            }
            else
            {
              switch (current2)
              {
                case null:
                  stringBuilder.Append("null");
                  break;
                case string _:
                case char _:
                case StringBuilder _:
                  stringBuilder.Append('"').Append(current2.ToString()).Append('"');
                  break;
                default:
                  stringBuilder.Append((current2 ?? (object) "null").ToString());
                  break;
              }
            }
            flag2 = true;
            goto label_67;
label_45:
            d = double.NaN;
            if (v2 != null)
              d = (double) Tools.JSObjectToNumber(v2);
            else if (!Tools.ParseNumber((current2 ?? (object) "null").ToString(), out d, 0))
              d = double.NaN;
            if (double.IsNaN(d) || double.IsInfinity(d))
              d = 0.0;
            d = System.Math.Truncate(d);
            string str2 = Tools.DoubleToString(System.Math.Abs(d));
            if (d < 0.0)
              stringBuilder.Append('-');
            if (str2.Length < val2)
              stringBuilder.Append(new string('0', val2 - str2.Length));
            stringBuilder.Append(str2);
            flag2 = true;
            goto label_67;
label_66:
            flag3 = true;
label_67:
            if (flag3)
              stringBuilder.Append(str1.Substring(startIndex, length));
            startIndex += length;
            if (flag2)
              flag1 = enumerator.MoveNext();
          }
          else
            break;
        }
        if (startIndex < str1.Length)
          stringBuilder.Append(str1.Substring(startIndex).Replace("%%", "%"));
        for (; flag1; flag1 = enumerator.MoveNext())
        {
          object current3;
          JSValue v3 = (current3 = enumerator.Current) as JSValue;
          stringBuilder.Append(' ');
          if (v3 != null)
          {
            if (v3.ValueType == JSValueType.Object)
              stringBuilder.Append(Tools.JSValueToObjectString(v3));
            else
              stringBuilder.Append(v3.ToString());
          }
          else
            stringBuilder.Append((current3 ?? (object) "null").ToString());
        }
      }
      else
      {
        if (v1 != null)
        {
          if (v1.ValueType == JSValueType.Object)
            stringBuilder.Append(Tools.JSValueToObjectString(v1));
          else
            stringBuilder.Append(v1.ToString());
        }
        else
          stringBuilder.Append((current1 ?? (object) "null").ToString());
        for (; flag1; flag1 = enumerator.MoveNext())
        {
          object current4;
          JSValue v4 = (current4 = enumerator.Current) as JSValue;
          stringBuilder.Append(' ');
          if (v4 != null)
          {
            if (v4.ValueType == JSValueType.Object)
              stringBuilder.Append(Tools.JSValueToObjectString(v4));
            else
              stringBuilder.Append(v4.ToString());
          }
          else
            stringBuilder.Append((current4 ?? (object) "null").ToString());
        }
      }
      return stringBuilder.ToString();
    }

    public static bool IsTaskOfT(Type type)
    {
      TypeInfo typeInfo = (object) type != null ? type.GetTypeInfo() : (TypeInfo) null;
      if (typeInfo == null)
        return false;
      return typeInfo.IsGenericType && (object) typeInfo.GetGenericTypeDefinition() == (object) typeof (Task<>) || Tools.IsTaskOfT(typeInfo.BaseType);
    }

    private struct DoubleStringCacheItem
    {
      public double key;
      public string value;
    }

    internal sealed class _ForcedEnumerator<T> : IEnumerator<T>, IEnumerator, IDisposable
    {
      private int index;
      private IEnumerable<T> owner;
      private IEnumerator<T> parent;

      public _ForcedEnumerator(IEnumerable<T> owner)
      {
        this.owner = owner;
        this.parent = owner.GetEnumerator();
      }

      public T Current => this.parent.Current;

      public void Dispose() => this.parent.Dispose();

      object IEnumerator.Current => (object) this.parent.Current;

      public bool MoveNext()
      {
        try
        {
          int num = this.parent.MoveNext() ? 1 : 0;
          if (num != 0)
            ++this.index;
          return num != 0;
        }
        catch
        {
          this.parent = this.owner.GetEnumerator();
          int num = 0;
          while (num < this.index && this.parent.MoveNext())
            ++num;
          return this.MoveNext();
        }
      }

      public void Reset() => this.parent.Reset();
    }

    private struct IntStringCacheItem
    {
      public int key;
      public string value;
    }
  }
}
