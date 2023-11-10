// Decompiled with JetBrains decompiler
// Type: NiL.JS.BaseLibrary.GlobalFunctions
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Core.Interop;
using System;
using System.Text;

namespace NiL.JS.BaseLibrary
{
  public static class GlobalFunctions
  {
    internal static JSValue isFinite(JSValue thisBind, Arguments x)
    {
      double d = Tools.JSObjectToDouble(x[0]);
      return (JSValue) (!double.IsNaN(d) && !double.IsInfinity(d));
    }

    internal static JSValue isNaN(JSValue thisBind, Arguments x)
    {
      JSValue valueValueString = x[0];
      if (valueValueString._valueType >= JSValueType.Object)
        valueValueString = valueValueString.ToPrimitiveValue_Value_String();
      if (valueValueString._valueType == JSValueType.Double)
        return (JSValue) double.IsNaN(valueValueString._dValue);
      if (valueValueString._valueType == JSValueType.Boolean || valueValueString._valueType == JSValueType.Integer)
        return (JSValue) false;
      if (valueValueString._valueType != JSValueType.String)
        return (JSValue) true;
      double d = 0.0;
      int index = 0;
      return Tools.ParseNumber(valueValueString._oValue.ToString(), ref index, out d, ParseNumberOptions.Default) ? (JSValue) double.IsNaN(d) : (JSValue) true;
    }

    internal static JSValue unescape(JSValue thisBind, Arguments x) => (JSValue) Uri.UnescapeDataString(x[0].ToString());

    [ArgumentsCount(2)]
    internal static JSValue parseInt(JSValue thisBind, Arguments args)
    {
      double d1 = double.NaN;
      JSValue jsValue1 = args[1];
      double d2 = jsValue1.Exists ? Tools.JSObjectToDouble(jsValue1) : 0.0;
      int radix = double.IsNaN(d2) || double.IsInfinity(d2) ? 0 : (int) ((long) d2 & (long) uint.MaxValue);
      if (radix != 0 && (radix < 2 || radix > 36))
        return Number.NaN;
      JSValue jsValue2 = args[0];
      if (jsValue2._valueType == JSValueType.Integer)
        return jsValue2;
      if (jsValue2._valueType == JSValueType.Double)
      {
        if (double.IsInfinity(jsValue2._dValue) || double.IsNaN(jsValue2._dValue))
          return Number.NaN;
        return jsValue2._dValue != 0.0 ? (JSValue) (Number) System.Math.Truncate(jsValue2._dValue) : (JSValue) (Number) 0;
      }
      string code = jsValue2.ToString().Trim(Tools.TrimChars);
      if (!string.IsNullOrEmpty(code))
        Tools.ParseNumber(code, out d1, radix, ParseNumberOptions.AllowAutoRadix);
      return double.IsInfinity(d1) ? Number.NaN : (JSValue) System.Math.Truncate(d1);
    }

    internal static JSValue parseFloat(JSValue thisBind, Arguments x)
    {
      double num = double.NaN;
      JSValue jsValue = x[0];
      if (jsValue._valueType == JSValueType.Integer)
        return jsValue;
      if (jsValue._valueType == JSValueType.Double)
        return jsValue._dValue != 0.0 ? jsValue : (JSValue) (Number) 0;
      string code = jsValue.ToString().Trim(Tools.TrimChars);
      if (!string.IsNullOrEmpty(code))
        Tools.ParseNumber(code, out num, ParseNumberOptions.AllowFloat);
      return (JSValue) num;
    }

    internal static JSValue escape(JSValue thisBind, Arguments x) => (JSValue) Uri.EscapeDataString(x[0].ToString());

    internal static JSValue decodeURIComponent(JSValue thisBind, Arguments args)
    {
      string str = args[0].ToString();
      if (string.IsNullOrEmpty(str))
        return (JSValue) str;
      StringBuilder stringBuilder = new StringBuilder(str.Length);
      for (int index1 = 0; index1 < str.Length; ++index1)
      {
        if (str[index1] == '%')
        {
          if (index1 + 2 >= str.Length)
            ExceptionHelper.Throw((Error) new URIError("Substring after \"%\" not represent valid code."));
          if (!Tools.isHex(str[index1 + 1]) || !Tools.isHex(str[index1 + 2]))
            ExceptionHelper.Throw((Error) new URIError("Substring after \"%\" not represent valid code."));
          int num1 = Tools.hexCharToInt(str[index1 + 1]) * 16 + Tools.hexCharToInt(str[index1 + 2]);
          index1 += 2;
          if ((num1 & 128) == 0)
          {
            stringBuilder.Append((char) num1);
          }
          else
          {
            int num2 = 1;
            while ((num1 << num2 & 128) != 0)
              ++num2;
            if (num2 == 1 || num2 > 4)
              ExceptionHelper.Throw((Error) new URIError("URI malformed"));
            if (index1 + 3 * (num2 - 1) >= str.Length)
              ExceptionHelper.Throw((Error) new URIError("URI malformed"));
            int num3 = (num1 & (1 << num2 * 7 - 1) - 1 >> 8 * (num2 - 1)) << (num2 - 1) * 6;
            for (int index2 = 1; index2 < num2; ++index2)
            {
              int index3 = index1 + 1;
              if (str[index3] != '%')
                ExceptionHelper.Throw((Error) new URIError(""));
              if (!Tools.isHex(str[index3 + 1]) || !Tools.isHex(str[index3 + 2]))
                ExceptionHelper.Throw((Error) new URIError("Substring after \"%\" not represent valid code."));
              int num4 = Tools.hexCharToInt(str[index3 + 1]) * 16 + Tools.hexCharToInt(str[index3 + 2]);
              if ((num4 & 192) != 128)
                ExceptionHelper.Throw((Error) new URIError("URI malformed"));
              num3 |= (num4 & 63) << (num2 - index2 - 1) * 6;
              index1 = index3 + 2;
            }
            if (num3 < 65536)
            {
              char ch = (char) num3;
              stringBuilder.Append(ch);
            }
            else
            {
              stringBuilder.Append((char) ((num3 - 65536 >> 10 & 1023) + 55296));
              stringBuilder.Append((char) ((num3 - 65536 & 1023) + 56320));
            }
          }
        }
        else
          stringBuilder.Append(str[index1]);
      }
      return (JSValue) stringBuilder.ToString();
    }

    internal static JSValue decodeURI(JSValue thisBind, Arguments args)
    {
      string str = args[0].ToString();
      if (string.IsNullOrEmpty(str))
        return (JSValue) str;
      StringBuilder stringBuilder = new StringBuilder(str.Length);
      for (int index1 = 0; index1 < str.Length; ++index1)
      {
        if (str[index1] == '%')
        {
          if (index1 + 2 >= str.Length)
            ExceptionHelper.Throw((Error) new URIError("Substring after \"%\" not represent valid code."));
          if (!Tools.isHex(str[index1 + 1]) || !Tools.isHex(str[index1 + 2]))
            ExceptionHelper.Throw((Error) new URIError("Substring after \"%\" not represent valid code."));
          int num1 = Tools.hexCharToInt(str[index1 + 1]) * 16 + Tools.hexCharToInt(str[index1 + 2]);
          index1 += 2;
          if ((num1 & 128) == 0)
          {
            if (";/?:@&=+$,#".IndexOf((char) num1) == -1)
              stringBuilder.Append((char) num1);
            else
              stringBuilder.Append('%').Append(str[index1 - 1]).Append(str[index1]);
          }
          else
          {
            int index2 = index1 - 2;
            int num2 = 1;
            while ((num1 << num2 & 128) != 0)
              ++num2;
            if (num2 == 1 || num2 > 4)
              ExceptionHelper.Throw((Error) new URIError("URI malformed"));
            if (index1 + 3 * (num2 - 1) >= str.Length)
              ExceptionHelper.Throw((Error) new URIError("URI malformed"));
            int num3 = (num1 & (1 << num2 * 7 - 1) - 1 >> 8 * (num2 - 1)) << (num2 - 1) * 6;
            for (int index3 = 1; index3 < num2; ++index3)
            {
              int index4 = index1 + 1;
              if (str[index4] != '%')
                ExceptionHelper.Throw((Error) new URIError(""));
              if (!Tools.isHex(str[index4 + 1]) || !Tools.isHex(str[index4 + 2]))
                ExceptionHelper.Throw((Error) new URIError("Substring after \"%\" not represent valid code."));
              int num4 = Tools.hexCharToInt(str[index4 + 1]) * 16 + Tools.hexCharToInt(str[index4 + 2]);
              if ((num4 & 192) != 128)
                ExceptionHelper.Throw((Error) new URIError("URI malformed"));
              num3 |= (num4 & 63) << (num2 - index3 - 1) * 6;
              index1 = index4 + 2;
            }
            if (num3 < 65536)
            {
              char ch = (char) num3;
              if (";/?:@&=+$,#".IndexOf(ch) != -1)
              {
                for (; index2 < index1; ++index2)
                  stringBuilder.Append(str[index2]);
              }
              else
                stringBuilder.Append(ch);
            }
            else
            {
              stringBuilder.Append((char) ((num3 - 65536 >> 10 & 1023) + 55296));
              stringBuilder.Append((char) ((num3 - 65536 & 1023) + 56320));
            }
          }
        }
        else
          stringBuilder.Append(str[index1]);
      }
      return (JSValue) stringBuilder.ToString();
    }

    private static bool doNotEscape(char c)
    {
      if (c >= '0' && c <= '9' || c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z')
        return true;
      if (c <= '.')
      {
        if (c != '!')
        {
          switch ((int) c - 39)
          {
            case 0:
            case 1:
            case 2:
            case 3:
            case 6:
            case 7:
              break;
            default:
              goto label_7;
          }
        }
      }
      else if (c != '_' && c != '~')
        goto label_7;
      return true;
label_7:
      return false;
    }

    internal static JSValue encodeURIComponent(JSValue thisBind, Arguments args)
    {
      string str = args[0].ToString();
      StringBuilder stringBuilder = new StringBuilder(str.Length);
      for (int index = 0; index < str.Length; ++index)
      {
        if (GlobalFunctions.doNotEscape(str[index]))
        {
          stringBuilder.Append(str[index]);
        }
        else
        {
          if (str[index] >= '\uDC00' && str[index] <= '\uDFFF')
            ExceptionHelper.Throw((Error) new URIError(""));
          int num1;
          if (str[index] < '\uD800' || str[index] > '\uDBFF')
          {
            num1 = (int) str[index];
          }
          else
          {
            ++index;
            if (index == str.Length)
              ExceptionHelper.Throw((Error) new URIError(""));
            if (str[index] < '\uDC00' || str[index] > '\uDFFF')
              ExceptionHelper.Throw((Error) new URIError(""));
            num1 = ((int) str[index - 1] - 55296) * 1024 + ((int) str[index] - 56320) + 65536;
          }
          int num2 = 1;
          if (num1 > (int) sbyte.MaxValue)
          {
            while (num1 >> num2 * 6 - (num2 - 1) != 0)
              ++num2;
            if (num2 > 4)
              ExceptionHelper.Throw((Error) new URIError(""));
            int num3 = num1 >> (num2 - 1) * 6 | ~((1 << 8 - num2) - 1);
            stringBuilder.Append('%').Append(Tools.NumChars[num3 >> 4 & 15]).Append(Tools.NumChars[num3 & 15]);
            while (--num2 > 0)
            {
              int num4 = num1 >> (num2 - 1) * 6 & 63 | (int) sbyte.MinValue;
              stringBuilder.Append('%').Append(Tools.NumChars[num4 >> 4 & 15]).Append(Tools.NumChars[num4 & 15]);
            }
          }
          else
            stringBuilder.Append('%').Append(Tools.NumChars[(int) str[index] >> 4 & 15]).Append(Tools.NumChars[(int) str[index] & 15]);
        }
      }
      return (JSValue) stringBuilder.ToString();
    }

    internal static JSValue encodeURI(JSValue thisBind, Arguments args)
    {
      string str = args[0].ToString();
      StringBuilder stringBuilder = new StringBuilder(str.Length);
      for (int index = 0; index < str.Length; ++index)
      {
        if (GlobalFunctions.doNotEscape(str[index]) || ";/?:@&=+$,#".IndexOf(str[index]) != -1)
        {
          stringBuilder.Append(str[index]);
        }
        else
        {
          if (str[index] >= '\uDC00' && str[index] <= '\uDFFF')
            ExceptionHelper.Throw((Error) new URIError(""));
          int num1;
          if (str[index] < '\uD800' || str[index] > '\uDBFF')
          {
            num1 = (int) str[index];
          }
          else
          {
            ++index;
            if (index == str.Length)
              ExceptionHelper.Throw((Error) new URIError(""));
            if (str[index] < '\uDC00' || str[index] > '\uDFFF')
              ExceptionHelper.Throw((Error) new URIError(""));
            num1 = ((int) str[index - 1] - 55296) * 1024 + ((int) str[index] - 56320) + 65536;
          }
          int num2 = 1;
          if (num1 > (int) sbyte.MaxValue)
          {
            while (num1 >> num2 * 6 - (num2 - 1) != 0)
              ++num2;
            if (num2 > 4)
              ExceptionHelper.Throw((Error) new URIError(""));
            int num3 = num1 >> (num2 - 1) * 6 | ~((1 << 8 - num2) - 1);
            stringBuilder.Append('%').Append(Tools.NumChars[num3 >> 4 & 15]).Append(Tools.NumChars[num3 & 15]);
            while (--num2 > 0)
            {
              int num4 = num1 >> (num2 - 1) * 6 & 63 | (int) sbyte.MinValue;
              stringBuilder.Append('%').Append(Tools.NumChars[num4 >> 4 & 15]).Append(Tools.NumChars[num4 & 15]);
            }
          }
          else
            stringBuilder.Append('%').Append(Tools.NumChars[(int) str[index] >> 4 & 15]).Append(Tools.NumChars[(int) str[index] & 15]);
        }
      }
      return (JSValue) stringBuilder.ToString();
    }
  }
}
