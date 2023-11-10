// Decompiled with JetBrains decompiler
// Type: NiL.JS.BaseLibrary.String
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Core.Interop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NiL.JS.BaseLibrary
{
  public sealed class String : JSObject
  {
    private Number _length;

    [DoNotEnumerate]
    public static JSValue fromCharCode(Arguments args)
    {
      if (args == null || args.Length == 0)
        return (JSValue) new String();
      string str = "";
      for (int index = 0; index < args.Length; ++index)
      {
        int int32 = Tools.JSObjectToInt32(args[index]);
        str += ((char) int32).ToString();
      }
      return (JSValue) str;
    }

    [DoNotEnumerate]
    public static JSValue fromCodePoint(Arguments args)
    {
      if (args == null || args.Length == 0)
        return (JSValue) new String();
      int codePoint = 0;
      string str = "";
      for (int index = 0; index < args.Length; ++index)
      {
        JSValue number = Tools.JSObjectToNumber(args[index]);
        if (number._valueType == JSValueType.Integer)
        {
          if (number._iValue < 0 || number._iValue > 1114111)
            ExceptionHelper.Throw((Error) new RangeError("Invalid code point " + Tools.Int32ToString(number._iValue)));
          codePoint = number._iValue;
        }
        else if (number._valueType == JSValueType.Double)
        {
          if (number._dValue < 0.0 || number._dValue > 1114111.0 || double.IsInfinity(number._dValue) || double.IsNaN(number._dValue) || number._dValue % 1.0 != 0.0)
            ExceptionHelper.Throw((Error) new RangeError("Invalid code point " + Tools.DoubleToString(number._dValue)));
          codePoint = (int) number._dValue;
        }
        str += Tools.CodePointToString(codePoint);
      }
      return (JSValue) str;
    }

    [DoNotEnumerate]
    public String()
      : this("")
    {
    }

    [DoNotEnumerate]
    public String(Arguments args)
      : this(args.Length == 0 ? "" : args[0].ToPrimitiveValue_String_Value().ToString())
    {
    }

    [DoNotEnumerate]
    [StrictConversion]
    public String(string s)
    {
      this._oValue = (object) (s ?? "null");
      this._valueType = JSValueType.String;
      this._attributes |= JSValueAttributesInternal.SystemObject;
    }

    [Hidden]
    public JSValue this[int pos]
    {
      [Hidden] get
      {
        if (pos < 0 || pos >= this._oValue.ToString().Length)
          return JSValue.notExists;
        return new JSValue()
        {
          _valueType = JSValueType.String,
          _oValue = (object) this._oValue.ToString()[pos].ToString(),
          _attributes = JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.NonConfigurable
        };
      }
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static String charAt(JSValue self, Arguments args)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.charAt called on null or undefined"));
      string str = self.BaseToString();
      int int32 = Tools.JSObjectToInt32(args[0], true);
      return int32 < 0 || int32 >= str.Length ? (String) "" : (String) str[int32].ToString();
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue charCodeAt(JSValue self, Arguments args)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.charCodeAt called on null or undefined"));
      string str = self.BaseToString();
      int int32 = Tools.JSObjectToInt32(args[0], true);
      return int32 < 0 || int32 >= str.Length ? Number.NaN : (JSValue) new Number((int) str[int32]);
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue codePointAt(JSValue self, Arguments args)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.codePointAt called on null or undefined"));
      string str = self.BaseToString();
      int int32 = Tools.JSObjectToInt32(args[0], true);
      return int32 < 0 || int32 >= str.Length ? JSValue.undefined : (JSValue) Tools.NextCodePoint(str, ref int32);
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue concat(JSValue self, Arguments args)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.concat called on null or undefined"));
      string str = self.BaseToString();
      if (args == null || args.Length == 0)
        return (JSValue) str;
      if (args.Length == 1)
        return (JSValue) (str + args[0].ToString());
      if (args.Length == 2)
        return (JSValue) (str + args[0].ToString() + args[1].ToString());
      if (args.Length == 3)
        return (JSValue) (str + args[0].ToString() + args[1].ToString() + args[2].ToString());
      if (args.Length == 4)
        return (JSValue) (str + args[0].ToString() + args[1].ToString() + args[2].ToString() + args[3].ToString());
      StringBuilder stringBuilder = new StringBuilder().Append(str);
      for (int index = 0; index < args.Length; ++index)
        stringBuilder.Append((object) args[index]);
      return (JSValue) stringBuilder.ToString();
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue endsWith(JSValue self, Arguments args)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.endsWith called on null or undefined"));
      return !self.BaseToString().EndsWith((args?[0] ?? JSValue.undefinedString).ToString(), StringComparison.Ordinal) ? (JSValue) Boolean.False : (JSValue) Boolean.True;
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue includes(JSValue self, Arguments args)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.includes called on null or undefined"));
      return self.BaseToString().IndexOf((args?[0] ?? JSValue.undefinedString).ToString(), StringComparison.Ordinal) == -1 ? (JSValue) Boolean.False : (JSValue) Boolean.True;
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue indexOf(JSValue self, Arguments args)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.indexOf called on null or undefined"));
      string str1 = self.BaseToString();
      if (args == null || args.Length == 0)
        return (JSValue) -1;
      string str2 = args[0].ToString();
      int startIndex = 0;
      if (args.Length > 1)
      {
        startIndex = Tools.JSObjectToInt32(args[1], 0, 0, true);
        if (startIndex < 0)
          startIndex = 0;
        if (startIndex > str1.Length)
          startIndex = str1.Length - 1;
      }
      return (JSValue) str1.IndexOf(str2, startIndex, StringComparison.Ordinal);
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue lastIndexOf(JSValue self, Arguments args)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.lastIndexOf called on null or undefined"));
      string str1 = self.BaseToString();
      if (args == null || args.Length == 0)
        return (JSValue) -1;
      string str2 = args[0].ToString();
      int num1 = str1.Length;
      if (args.Length > 1)
      {
        JSValue valueValueString = args[1];
        if (valueValueString.ValueType >= JSValueType.Object)
          valueValueString = valueValueString.ToPrimitiveValue_Value_String();
        int num2 = Tools.JSObjectToInt32(valueValueString, num1, num1, true);
        if (num2 < 0)
          num2 = 0;
        num1 = num2 + str2.Length;
        if (num1 > str1.Length)
          num1 = str1.Length;
      }
      return (JSValue) str1.LastIndexOf(str2, num1, StringComparison.Ordinal);
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue localeCompare(JSValue self, Arguments args)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.localeCompare called on null or undefined"));
      return (JSValue) string.CompareOrdinal(self.BaseToString(), args[0].ToString());
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue match(JSValue self, Arguments args)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.match called on null or undefined"));
      JSValue jsValue = args[0];
      if (!(jsValue.Value is RegExp regExp))
        regExp = new RegExp((jsValue._valueType > JSValueType.Undefined ? (object) jsValue : (object) "").ToString(), "");
      if (!regExp._global)
        return regExp.exec(self);
      regExp.lastIndex = (JSValue) 0;
      if ((bool) regExp.sticky)
      {
        Match match = regExp._regex.Match(self.ToString());
        if (!match.Success || match.Index != 0)
          return (JSValue) null;
        Array array = new Array();
        array._data[0] = (JSValue) match.Value;
        int index = 0;
        while (true)
        {
          match = match.NextMatch();
          if (match.Success && match.Index == ++index)
            array._data[index] = (JSValue) match.Value;
          else
            break;
        }
        return (JSValue) array;
      }
      MatchCollection matchCollection = regExp._regex.Matches(self.ToString());
      if (matchCollection.Count == 0)
        return (JSValue) null;
      JSValue[] data = new JSValue[matchCollection.Count];
      for (int i = 0; i < matchCollection.Count; ++i)
        data[i] = (JSValue) matchCollection[i].Value;
      return (JSValue) new Array(data);
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(2)]
    public static JSValue padEnd(JSValue self, Arguments args)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.codePointAt called on null or undefined"));
      string str1 = self.BaseToString();
      if (args == null || args.Length == 0)
        return (JSValue) str1;
      int int32 = Tools.JSObjectToInt32(args[0], true);
      if (int32 <= str1.Length)
        return (JSValue) str1;
      if (args.Length < 2 || args[1] == null || !args[1].Defined)
        return (JSValue) str1.PadRight(int32, ' ');
      string str2 = args[1].ToString();
      if (string.IsNullOrEmpty(str2))
        return (JSValue) str1;
      if (str2.Length == 1)
        return (JSValue) str1.PadRight(int32, str2[0]);
      StringBuilder stringBuilder = new StringBuilder(int32);
      stringBuilder.Append(str1);
      for (int index = int32 / str2.Length - 1; index >= 0; --index)
        stringBuilder.Append(str2);
      stringBuilder.Append(str2.Substring(0, int32 % str2.Length));
      return (JSValue) stringBuilder.ToString();
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(2)]
    public static JSValue padStart(JSValue self, Arguments args)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.codePointAt called on null or undefined"));
      string str1 = self.BaseToString();
      if (args == null || args.Length == 0)
        return (JSValue) str1;
      int int32 = Tools.JSObjectToInt32(args[0], true);
      if (int32 <= str1.Length)
        return (JSValue) str1;
      if (args.Length < 2 || args[1] == null || !args[1].Defined)
        return (JSValue) str1.PadLeft(int32, ' ');
      string str2 = args[1].ToString();
      if (string.IsNullOrEmpty(str2))
        return (JSValue) str1;
      if (str2.Length == 1)
        return (JSValue) str1.PadLeft(int32, str2[0]);
      StringBuilder stringBuilder = new StringBuilder(int32);
      for (int index = int32 / str2.Length - 1; index >= 0; --index)
        stringBuilder.Append(str2);
      stringBuilder.Append(str2.Substring(0, int32 % str2.Length));
      stringBuilder.Append(str1);
      return (JSValue) stringBuilder.ToString();
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue repeat(JSValue self, Arguments args)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.repeat called on null or undefined"));
      string str = self.BaseToString();
      if (args == null || args.Length == 0)
        return (JSValue) "";
      if (str.Length == 0)
        return (JSValue) "";
      double d1 = 0.0;
      if (args.Length > 0)
        d1 = Tools.JSObjectToDouble(args[0]);
      if (double.IsNaN(d1))
        d1 = 0.0;
      double d2 = System.Math.Truncate(d1);
      if (d2 < 0.0 || double.IsInfinity(d2))
        ExceptionHelper.Throw((Error) new RangeError("Invalid count value"));
      int num = (int) d2;
      switch (num)
      {
        case 0:
          return (JSValue) "";
        case 1:
          return (JSValue) str;
        default:
          StringBuilder stringBuilder = new StringBuilder(str.Length * num);
          for (int index = 0; index < num; ++index)
            stringBuilder.Append(str);
          return (JSValue) stringBuilder.ToString();
      }
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(2)]
    [AllowNullArguments]
    public static JSValue replace(JSValue self, Arguments args)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.replace called on null or undefined"));
      string selfStr = self.BaseToString();
      if (args == null || args.Length == 0)
        return (JSValue) selfStr;
      JSValue jsValue1 = args[0];
      RegExp regExp = jsValue1?.Value as RegExp;
      JSValue jsValue2 = args[1];
      Function f = jsValue2?.Value as Function;
      if (regExp != null)
      {
        Regex regex = regExp._regex;
        if (f != null)
        {
          String str = new String(selfStr);
          String match = new String();
          Arguments fArgs = new Arguments(Context.CurrentContext);
          return (JSValue) regex.Replace(selfStr, (MatchEvaluator) (m =>
          {
            str._oValue = (object) selfStr;
            str._valueType = JSValueType.String;
            match._oValue = (object) m.Value;
            match._valueType = JSValueType.String;
            fArgs._iValue = m.Groups.Count + 2;
            fArgs[0] = (JSValue) match;
            for (int index = 1; index < m.Groups.Count; ++index)
              fArgs[index] = (JSValue) m.Groups[index].Value;
            fArgs[fArgs._iValue - 2] = (JSValue) m.Index;
            fArgs[fArgs._iValue - 1] = (JSValue) str;
            return f.Call(fArgs).ToString();
          }), regExp._global ? int.MaxValue : 1);
        }
        string replacement = args.Length > 1 ? (jsValue2 ?? (JSValue) "").ToString() : "undefined";
        return (JSValue) regex.Replace(selfStr, replacement, regExp._global ? int.MaxValue : 1);
      }
      string str1 = (jsValue1 ?? (JSValue) "").ToString();
      if (f != null)
      {
        int length = selfStr.IndexOf(str1, StringComparison.Ordinal);
        if (length == -1)
          return (JSValue) selfStr;
        Arguments arguments = new Arguments(Context.CurrentContext);
        arguments._iValue = 3;
        Arguments args1 = arguments;
        args1[0] = (JSValue) str1;
        args1[1] = (JSValue) length;
        args1[2] = self;
        return (JSValue) (selfStr.Substring(0, length) + f.Call(args1).ToString() + selfStr.Substring(length + str1.Length));
      }
      string str2 = args.Length > 1 ? (jsValue2 ?? (JSValue) "").ToString() : "undefined";
      if (str1.Length == 0)
        return (JSValue) (str2 + selfStr);
      int length1 = selfStr.IndexOf(str1, StringComparison.Ordinal);
      return length1 == -1 ? (JSValue) selfStr : (JSValue) (selfStr.Substring(0, length1) + str2 + selfStr.Substring(length1 + str1.Length));
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue search(JSValue self, Arguments args)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.search called on null or undefined"));
      string input = self.BaseToString();
      if (args == null || args.Length == 0)
        return (JSValue) 0;
      JSValue jsValue = args[0];
      if (!(jsValue.Value is RegExp regExp))
        return (JSValue) input.IndexOf(jsValue.ToString(), StringComparison.Ordinal);
      Match match = regExp._regex.Match(input);
      if (!match.Success)
        return (JSValue) -1;
      return (bool) regExp.sticky && match.Index != 0 ? (JSValue) -1 : (JSValue) match.Index;
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(2)]
    public static JSValue slice(JSValue self, Arguments args)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.slice called on null or undefined"));
      string str = self.BaseToString();
      if (args == null || args.Length == 0)
        return (JSValue) self.BaseToString();
      int int32_1 = Tools.JSObjectToInt32(args[0], 0, 0, 0, true);
      int int32_2 = Tools.JSObjectToInt32(args[1], 0, str.Length, 0, true);
      if (int32_1 < 0)
        int32_1 += str.Length;
      if (int32_2 < 0)
        int32_2 += str.Length;
      int startIndex = System.Math.Min(System.Math.Max(0, int32_1), str.Length);
      int num = System.Math.Min(System.Math.Max(0, int32_2), str.Length);
      return startIndex >= num ? (JSValue) new String() : (JSValue) str.Substring(startIndex, num - startIndex);
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(2)]
    public static JSValue split(JSValue self, Arguments args)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.split called on null or undefined"));
      string input = self.BaseToString();
      if (args == null || args.Length == 0 || !args[0].Defined)
        return (JSValue) new Array() { (JSValue) input };
      JSValue jsValue = args[0];
      RegExp regExp = jsValue.Value as RegExp;
      uint int64 = (uint) Tools.JSObjectToInt64(args[1], long.MaxValue, true);
      if (int64 == 0U)
        return (JSValue) new Array();
      if (regExp != null)
      {
        Match match = regExp._regex.Match(input, 0);
        if (!match.Success)
          return (JSValue) new Array(new JSValue[1]
          {
            (JSValue) input
          });
        Array array = new Array();
        int startIndex = 0;
        while (array._data.Length < int64)
        {
          if (startIndex > 0)
            match = match.NextMatch();
          if (!match.Success)
          {
            array._data.Add((JSValue) input.Substring(startIndex, input.Length - startIndex));
            break;
          }
          if (match.Index < input.Length)
          {
            int num = match.Index + (match.Length == 0 ? 1 : 0);
            string str = input.Substring(startIndex, num - startIndex);
            array._data.Add((JSValue) str);
            if (num < input.Length)
            {
              for (int groupnum = 1; groupnum < match.Groups.Count && array._data.Length < int64; ++groupnum)
                array._data.Add(match.Groups[groupnum].Success ? (JSValue) match.Groups[groupnum].Value : JSValue.undefined);
            }
            startIndex = num + match.Length;
          }
          else
            break;
        }
        return (JSValue) array;
      }
      string str1 = jsValue.ToString();
      if (string.IsNullOrEmpty(str1))
      {
        int length = System.Math.Min(input.Length, (int) System.Math.Min((uint) int.MaxValue, int64));
        JSValue[] data = new JSValue[length];
        for (int index = 0; index < length; ++index)
          data[index] = (JSValue) new String(input[index].ToString());
        return (JSValue) new Array(data);
      }
      Array array1 = new Array();
      int startIndex1 = 0;
      while (array1._data.Length < int64)
      {
        int num = input.IndexOf(str1, startIndex1, StringComparison.Ordinal);
        if (num == -1)
        {
          array1._data.Add((JSValue) input.Substring(startIndex1, input.Length - startIndex1));
          break;
        }
        array1._data.Add((JSValue) input.Substring(startIndex1, num - startIndex1));
        startIndex1 = num + str1.Length;
      }
      return (JSValue) array1;
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(1)]
    public static JSValue startsWith(JSValue self, Arguments args)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.startsWith called on null or undefined"));
      return !self.BaseToString().StartsWith((args?[0] ?? JSValue.undefinedString).ToString(), StringComparison.Ordinal) ? (JSValue) Boolean.False : (JSValue) Boolean.True;
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(2)]
    public static JSValue substring(JSValue self, Arguments args)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.substring called on null or undefined"));
      string str = self.BaseToString();
      if (args == null || args.Length == 0)
        return (JSValue) self.BaseToString();
      int val1 = Tools.JSObjectToInt32(args[0], 0, 0, 0, true);
      int int32 = Tools.JSObjectToInt32(args[1], 0, str.Length, 0, true);
      if (val1 > int32)
      {
        int num = val1 ^ int32;
        int32 ^= num;
        val1 = num ^ int32;
      }
      int startIndex = System.Math.Max(0, System.Math.Min(val1, str.Length));
      int num1 = System.Math.Max(0, System.Math.Min(int32, str.Length));
      return (JSValue) str.Substring(startIndex, System.Math.Min(str.Length, System.Math.Max(0, num1 - startIndex)));
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(2)]
    public static JSValue substr(JSValue self, Arguments args)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.substr called on null or undefined"));
      string str = self.BaseToString();
      if (args.Length == 0)
        return self;
      int startIndex = Tools.JSObjectToInt32(args[0], 0, 0, 0, true);
      int length = Tools.JSObjectToInt32(args[1], 0, str.Length, 0, true);
      if (startIndex < 0)
        startIndex += str.Length;
      if (startIndex < 0)
        startIndex = 0;
      if (startIndex >= str.Length || length <= 0)
        return (JSValue) "";
      if (str.Length < startIndex + length)
        length = str.Length - startIndex;
      return (JSValue) str.Substring(startIndex, length);
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(0)]
    public static JSValue toLocaleLowerCase(JSValue self)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.toLocaleLowerCase called on null or undefined"));
      return (JSValue) self.ToString().ToLower();
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(0)]
    public static JSValue toLocaleUpperCase(JSValue self)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.toLocaleUpperCase called on null or undefined"));
      return (JSValue) self.ToString().ToUpper();
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(0)]
    public static JSValue toLowerCase(JSValue self)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.toLowerCase called on null or undefined"));
      return (JSValue) self.ToString().ToLowerInvariant();
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(0)]
    public static JSValue toUpperCase(JSValue self)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.toUpperCase called on null or undefined"));
      return (JSValue) self.ToString().ToUpperInvariant();
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(0)]
    public static JSValue trim(JSValue self)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.trim called on null or undefined"));
      string str = self.BaseToString();
      return str == "" ? (JSValue) str : (JSValue) str.Trim(Tools.TrimChars);
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(0)]
    [CLSCompliant(false)]
    public static JSValue toString(JSValue self)
    {
      if (self is String && self._valueType == JSValueType.Object)
        return (JSValue) self.BaseToString();
      if (self._valueType != JSValueType.String)
        ExceptionHelper.Throw((Error) new TypeError("Try to call String.toString for not string object."));
      return self;
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(0)]
    public static JSValue valueOf(JSValue self)
    {
      if (self is String && self._valueType == JSValueType.Object)
        return (JSValue) self.BaseToString();
      if (self._valueType != JSValueType.String)
        ExceptionHelper.Throw((Error) new TypeError("Try to call String.valueOf for not string object."));
      return self;
    }

    [Field]
    [ReadOnly]
    [DoNotDelete]
    [DoNotEnumerate]
    [NotConfigurable]
    public JSValue length
    {
      [Hidden] get
      {
        int length = this._oValue.ToString().Length;
        if (this._length == null)
        {
          Number number = new Number(length);
          number._attributes = JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.NonConfigurable;
          this._length = number;
        }
        else
          this._length._iValue = length;
        return (JSValue) this._length;
      }
    }

    [Hidden]
    public override string ToString()
    {
      if (this._valueType != JSValueType.String)
        ExceptionHelper.Throw((Error) new TypeError("Try to call String.toString for not string object."));
      return this._oValue.ToString();
    }

    [Hidden]
    public override bool Equals(object obj) => obj is String && this._oValue.Equals((obj as String)._oValue);

    [Hidden]
    public override int GetHashCode() => this._oValue.GetHashCode();

    [Hidden]
    protected internal override JSValue GetProperty(
      JSValue key,
      bool forWrite,
      PropertyScope memberScope)
    {
      if (memberScope < PropertyScope.Super && key._valueType != JSValueType.Symbol)
      {
        int num = 0;
        double d = Tools.JSObjectToDouble(key);
        int pos;
        if (!double.IsInfinity(d) && !double.IsNaN(d) && (double) (num = (int) d) == d && (double) (pos = (int) d) == d && pos < this._oValue.ToString().Length && pos >= 0)
          return this[pos];
        if (key.ToString() == "length")
          return this.length;
      }
      return base.GetProperty(key, forWrite, memberScope);
    }

    [Hidden]
    protected internal override IEnumerator<KeyValuePair<string, JSValue>> GetEnumerator(
      bool hideNonEnum,
      EnumerationMode enumeratorMode)
    {
      String @string = this;
      string str = @string._oValue.ToString();
      int len = str.Length;
      for (int i = 0; i < len; ++i)
      {
        if (str[i] >= '\uD800' && str[i] <= '\uDBFF' && i + 1 < len && str[i + 1] >= '\uDC00' && str[i + 1] <= '\uDFFF')
        {
          yield return new KeyValuePair<string, JSValue>(Tools.Int32ToString(i), (JSValue) (enumeratorMode > EnumerationMode.KeysOnly ? str.Substring(i, 2) : (string) null));
          ++i;
        }
        else
          yield return new KeyValuePair<string, JSValue>(Tools.Int32ToString(i), (JSValue) (enumeratorMode > EnumerationMode.KeysOnly ? str[i].ToString() : (string) null));
      }
      if (!hideNonEnum)
        yield return new KeyValuePair<string, JSValue>("length", @string.length);
      if (@string._fields != null)
      {
        foreach (KeyValuePair<string, JSValue> field in (IEnumerable<KeyValuePair<string, JSValue>>) @string._fields)
        {
          if (field.Value.Exists && (!hideNonEnum || (field.Value._attributes & JSValueAttributesInternal.DoNotEnumerate) == JSValueAttributesInternal.None))
            yield return field;
        }
      }
    }

    public static JSValue raw(Arguments args)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (!(args[0].Value is Array array1))
        array1 = Tools.arraylikeToArray(args[0], true, false, false, -1L);
      Array array2 = array1;
      for (int index = 0; (long) index < (long) array2._data.Length; ++index)
      {
        if (index > 0)
          stringBuilder.Append((object) args[index]);
        stringBuilder.Append((object) array2._data[index]);
      }
      return (JSValue) stringBuilder.ToString();
    }

    [DoNotEnumerate]
    [InstanceMember]
    public static JSValue anchor(JSValue self, Arguments arg)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.anchor called on null or undefined"));
      return (JSValue) ("<a name=\"" + arg[0].Value?.ToString() + "\">" + self.ToString() + "</a>");
    }

    [DoNotEnumerate]
    [InstanceMember]
    public static JSValue big(JSValue self)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.big called on null or undefined"));
      return (JSValue) ("<big>" + self.ToString() + "</big>");
    }

    [DoNotEnumerate]
    [InstanceMember]
    public static JSValue blink(JSValue self)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.blink called on null or undefined"));
      return (JSValue) ("<blink>" + self.ToString() + "</blink>");
    }

    [DoNotEnumerate]
    [InstanceMember]
    public static JSValue bold(JSValue self)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.bold called on null or undefined"));
      return (JSValue) ("<bold>" + self.ToString() + "</bold>");
    }

    [DoNotEnumerate]
    [InstanceMember]
    public static JSValue @fixed(JSValue self)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.fixed called on null or undefined"));
      return (JSValue) ("<tt>" + self.ToString() + "</tt>");
    }

    [DoNotEnumerate]
    [InstanceMember]
    public static JSValue fontcolor(JSValue self, Arguments arg)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.fontcolor called on null or undefined"));
      return (JSValue) ("<font color=\"" + arg[0].Value?.ToString() + "\">" + self.ToString() + "</font>");
    }

    [DoNotEnumerate]
    [InstanceMember]
    public static JSValue fontsize(JSValue self, Arguments arg)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.fontsize called on null or undefined"));
      return (JSValue) ("<font size=\"" + arg.Value?.ToString() + "\">" + self.ToString() + "</font>");
    }

    [DoNotEnumerate]
    [InstanceMember]
    public static JSValue italics(JSValue self)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.italics called on null or undefined"));
      return (JSValue) ("<i>" + self.ToString() + "</i>");
    }

    [DoNotEnumerate]
    [InstanceMember]
    public static JSValue link(JSValue self, Arguments arg)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.link called on null or undefined"));
      return (JSValue) ("<a href=\"" + arg[0].Value?.ToString() + "\">" + self.ToString() + "</a>");
    }

    [DoNotEnumerate]
    [InstanceMember]
    public static JSValue small(JSValue self)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.small called on null or undefined"));
      return (JSValue) ("<small>" + self.ToString() + "</small>");
    }

    [DoNotEnumerate]
    [InstanceMember]
    public static JSValue strike(JSValue self)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.strike called on null or undefined"));
      return (JSValue) ("<strike>" + self.ToString() + "</strike>");
    }

    [DoNotEnumerate]
    [InstanceMember]
    public static JSValue sub(JSValue self)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.sub called on null or undefined"));
      return (JSValue) ("<sub>" + self.ToString() + "</sub>");
    }

    [DoNotEnumerate]
    [InstanceMember]
    public static JSValue sup(JSValue self)
    {
      if (self == null || self._valueType <= JSValueType.Undefined || self._valueType >= JSValueType.Object && self.Value == null)
        ExceptionHelper.Throw((Error) new TypeError("String.prototype.sup called on null or undefined"));
      return (JSValue) ("<sup>" + self.ToString() + "</sup>");
    }

    [Hidden]
    public static implicit operator String(string val) => new String(val);

    [Hidden]
    public static implicit operator string(String val) => val._oValue.ToString();
  }
}
