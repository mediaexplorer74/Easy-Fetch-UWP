// Decompiled with JetBrains decompiler
// Type: NiL.JS.BaseLibrary.JSON
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Core.Interop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace NiL.JS.BaseLibrary
{
  public static class JSON
  {
    [DoNotEnumerate]
    [ArgumentsCount(2)]
    public static JSValue parse(Arguments args)
    {
      int int32 = Tools.JSObjectToInt32((JSValue) args._iValue);
      return JSON.parse(args[0].ToString(), int32 > 1 ? args[1]._oValue as Function : (Function) null);
    }

    [Hidden]
    public static JSValue parse(string code) => JSON.parse(code, (Function) null);

    private static bool isSpace(char c) => c != '\v' && c != '\f' && c != ' ' && c != ' ' && c != '\u180E' && c != ' ' && c != ' ' && c != ' ' && c != ' ' && c != ' ' && c != ' ' && c != ' ' && c != ' ' && c != ' ' && c != ' ' && c != ' ' && c != '\u2028' && c != '\u2029' && c != ' ' && c != ' ' && c != '　' && char.IsWhiteSpace(c);

    [Hidden]
    public static JSValue parse(string code, Function reviewer)
    {
      Stack<JSON.StackFrame> stackFrameStack = new Stack<JSON.StackFrame>();
      int index1 = 0;
      Arguments arguments;
      if (reviewer == null)
      {
        arguments = (Arguments) null;
      }
      else
      {
        arguments = new Arguments();
        arguments._iValue = 2;
      }
      Arguments args = arguments;
      stackFrameStack.Push(new JSON.StackFrame()
      {
        container = (JSValue) null,
        value = (JSValue) null,
        state = JSON.ParseState.Value
      });
      while (code.Length > index1 && JSON.isSpace(code[index1]))
        ++index1;
      while (index1 < code.Length)
      {
        bool flag = false;
        int index2 = index1;
        JSON.StackFrame stackFrame1 = stackFrameStack.Peek();
        int num1;
        if (Tools.IsDigit(code[index2]) || code[index2] == '-' && Tools.IsDigit(code[index2 + 1]))
        {
          if (stackFrame1.state != JSON.ParseState.Value)
            ExceptionHelper.ThrowSyntaxError("Unexpected token at position " + index1.ToString());
          double num2;
          if (!Tools.ParseNumber(code, ref index1, out num2))
            ExceptionHelper.ThrowSyntaxError("Invalid number definition.");
          int num3 = (int) num2;
          stackFrame1.state = JSON.ParseState.End;
          stackFrame1.value = (double) num3 != num2 ? (JSValue) num2 : (JSValue) num3;
        }
        else if (code[index2] == '"')
        {
          if (!Parser.ValidateString(code, ref index1, true))
            ExceptionHelper.ThrowSyntaxError("Unexpected token at position " + index1.ToString());
          string code1 = code.Substring(index2 + 1, index1 - index2 - 2);
          int length = code1.Length;
          while (length-- > 0)
          {
            if (code1[length] >= char.MinValue && code1[length] <= '\u001F')
            {
              num1 = (int) code1[length];
              ExceptionHelper.ThrowSyntaxError("Invalid string char '\\u000" + num1.ToString() + "'.");
            }
          }
          if (stackFrame1.state == JSON.ParseState.Name)
          {
            stackFrame1.fieldName = (JSValue) code1;
            stackFrame1.state = JSON.ParseState.Value;
            while (JSON.isSpace(code[index1]))
              ++index1;
            if (code[index1] != ':')
              ExceptionHelper.ThrowSyntaxError("Unexpected token at position " + index1.ToString());
            ++index1;
          }
          else
          {
            if (stackFrame1.state != JSON.ParseState.Value)
              ExceptionHelper.ThrowSyntaxError("Unexpected token at position " + index1.ToString());
            string str = Tools.Unescape(code1, false);
            JSON.StackFrame stackFrame2 = stackFrame1;
            stackFrame2.state = JSON.ParseState.End;
            stackFrame2.value = (JSValue) str;
          }
        }
        else if (Parser.Validate(code, "null", ref index1))
        {
          if (stackFrame1.state != JSON.ParseState.Value)
            ExceptionHelper.ThrowSyntaxError("Unexpected token at position " + index1.ToString());
          JSON.StackFrame stackFrame3 = stackFrame1;
          stackFrame3.state = JSON.ParseState.End;
          stackFrame3.value = (JSValue) JSValue.@null;
        }
        else if (Parser.Validate(code, "true", ref index1))
        {
          if (stackFrame1.state != JSON.ParseState.Value)
            ExceptionHelper.ThrowSyntaxError("Unexpected token at position " + index1.ToString());
          JSON.StackFrame stackFrame4 = stackFrame1;
          stackFrame4.state = JSON.ParseState.End;
          stackFrame4.value = (JSValue) true;
        }
        else if (Parser.Validate(code, "false", ref index1))
        {
          if (stackFrame1.state != JSON.ParseState.Value)
            ExceptionHelper.ThrowSyntaxError("Unexpected token at position " + index1.ToString());
          stackFrame1.state = JSON.ParseState.End;
          stackFrame1.value = (JSValue) false;
        }
        else if (code[index1] == '{')
        {
          if (stackFrame1.state != JSON.ParseState.Value)
            ExceptionHelper.ThrowSyntaxError("Unexpected token at position " + index1.ToString());
          stackFrame1.value = (JSValue) JSObject.CreateObject();
          stackFrame1.state = JSON.ParseState.Object;
          flag = true;
          ++index1;
        }
        else if (code[index1] == '[')
        {
          if (stackFrame1.state != JSON.ParseState.Value)
            ExceptionHelper.ThrowSyntaxError("Unexpected token at position " + index1.ToString());
          stackFrame1.value = (JSValue) new Array();
          stackFrame1.state = JSON.ParseState.Array;
          flag = true;
          ++index1;
        }
        else if (stackFrame1.state == JSON.ParseState.Value)
          ExceptionHelper.ThrowSyntaxError("Unexpected token at position " + index1.ToString());
        while (code.Length > index1 && JSON.isSpace(code[index1]))
          ++index1;
        if (stackFrame1.state == JSON.ParseState.End)
        {
          stackFrameStack.Pop();
          if (reviewer != null)
          {
            args[0] = stackFrame1.fieldName;
            args[1] = stackFrame1.value;
            JSValue jsValue = reviewer.Call(args);
            if (jsValue.Defined)
            {
              if (stackFrame1.container != null)
              {
                stackFrame1.container.GetProperty(stackFrame1.fieldName, true, PropertyScope.Own).Assign(jsValue);
              }
              else
              {
                stackFrame1.value = jsValue;
                stackFrameStack.Push(stackFrame1);
              }
            }
          }
          else if (stackFrame1.container != null)
            stackFrame1.container.GetProperty(stackFrame1.fieldName, true, PropertyScope.Own).Assign(stackFrame1.value);
          else
            stackFrameStack.Push(stackFrame1);
          stackFrame1 = stackFrameStack.Peek();
        }
        if (code.Length <= index1)
        {
          if (stackFrame1.state != JSON.ParseState.End)
            ExceptionHelper.ThrowSyntaxError(Strings.UnexpectedEndOfSource);
          else
            break;
        }
        switch (code[index1])
        {
          case ',':
            if (flag)
              ExceptionHelper.ThrowSyntaxError("Unexpected token at position " + index1.ToString());
            if (stackFrame1.state == JSON.ParseState.Array)
            {
              JSON.StackFrame stackFrame5 = new JSON.StackFrame();
              stackFrame5.state = JSON.ParseState.Value;
              num1 = stackFrame1.valuesCount++;
              stackFrame5.fieldName = (JSValue) num1.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              stackFrame5.container = stackFrame1.value;
              stackFrame1 = stackFrame5;
            }
            else if (stackFrame1.state == JSON.ParseState.Object)
              stackFrame1 = new JSON.StackFrame()
              {
                state = JSON.ParseState.Name,
                container = stackFrame1.value
              };
            else
              ExceptionHelper.ThrowSyntaxError("Unexpected token at position " + index1.ToString());
            stackFrameStack.Push(stackFrame1);
            ++index1;
            break;
          case ']':
            if (stackFrame1.state != JSON.ParseState.Array)
              ExceptionHelper.ThrowSyntaxError("Unexpected token at position " + index1.ToString());
            stackFrame1.state = JSON.ParseState.End;
            ++index1;
            break;
          case '}':
            if (stackFrame1.state != JSON.ParseState.Object)
              ExceptionHelper.ThrowSyntaxError("Unexpected token at position " + index1.ToString());
            stackFrame1.state = JSON.ParseState.End;
            ++index1;
            break;
          default:
            if (flag)
            {
              --index1;
              flag = false;
              goto case ',';
            }
            else
            {
              if (stackFrame1.state != JSON.ParseState.Value)
              {
                ExceptionHelper.ThrowSyntaxError("Unexpected token at position " + index1.ToString());
                break;
              }
              break;
            }
        }
        while (code.Length > index1 && JSON.isSpace(code[index1]))
          ++index1;
        if (code.Length <= index1 && stackFrame1.state != JSON.ParseState.End)
          ExceptionHelper.ThrowSyntaxError(Strings.UnexpectedEndOfSource);
      }
      if (stackFrameStack.Count != 1 || code.Length > index1 || stackFrameStack.Peek().state != JSON.ParseState.End)
        ExceptionHelper.ThrowSyntaxError(Strings.UnexpectedEndOfSource);
      return stackFrameStack.Pop().value;
    }

    [Hidden]
    public static string stringify(JSValue value) => JSON.stringify(new Arguments()
    {
      value
    }).ToString();

    [DoNotEnumerate]
    [ArgumentsCount(3)]
    public static JSValue stringify(Arguments args)
    {
      int iValue = args._iValue;
      Function oValue1 = iValue > 1 ? args[1]._oValue as Function : (Function) null;
      Array oValue2 = iValue > 1 ? args[1]._oValue as Array : (Array) null;
      string space = (string) null;
      if (args._iValue > 2)
      {
        JSValue jsValue1 = args[2];
        if (jsValue1._valueType >= JSValueType.Object)
        {
          if (!(jsValue1._oValue is JSValue jsValue2))
            jsValue2 = jsValue1;
          jsValue1 = jsValue2;
        }
        if (jsValue1 is ObjectWrapper)
        {
          if (!(jsValue1.Value is JSValue jsValue3))
            jsValue3 = jsValue1;
          jsValue1 = jsValue3;
        }
        if (jsValue1._valueType == JSValueType.Integer || jsValue1._valueType == JSValueType.Double || jsValue1._valueType == JSValueType.String)
        {
          if (jsValue1._valueType == JSValueType.Integer)
          {
            if (jsValue1._iValue > 0)
              space = "          ".Substring(10 - System.Math.Max(0, System.Math.Min(10, jsValue1._iValue)));
          }
          else if (jsValue1._valueType == JSValueType.Double)
          {
            if ((int) jsValue1._dValue > 0)
              space = "          ".Substring(10 - System.Math.Max(0, System.Math.Min(10, (int) jsValue1._dValue)));
          }
          else
          {
            space = jsValue1.ToString();
            if (space.Length > 10)
              space = space.Substring(0, 10);
            else if (space.Length == 0)
              space = (string) null;
          }
        }
      }
      JSValue jsValue4 = args[0];
      HashSet<string> keys = oValue2 == null ? (HashSet<string>) null : new HashSet<string>();
      if (keys != null)
      {
        foreach (JSValue jsValue5 in oValue2._data)
          keys.Add(jsValue5.ToString());
      }
      string str = JSON.stringify(jsValue4, oValue1, keys, space);
      return str == null ? JSValue.undefined : (JSValue) str;
    }

    [Hidden]
    public static string stringify(
      JSValue obj,
      Function replacer,
      HashSet<string> keys,
      string space)
    {
      return obj._valueType >= JSValueType.Object && obj.Value == null ? "null" : JSON.stringifyImpl("", obj, replacer, keys, space, new HashSet<JSValue>(), new Arguments());
    }

    internal static void escapeIfNeed(StringBuilder sb, char c)
    {
      if (c >= char.MinValue && c <= '\u001F' || c == '\\' || c == '"')
      {
        switch (c)
        {
          case '\b':
            sb.Append("\\b");
            break;
          case '\t':
            sb.Append("\\t");
            break;
          case '\n':
            sb.Append("\\n");
            break;
          case '\f':
            sb.Append("\\f");
            break;
          case '\r':
            sb.Append("\\r");
            break;
          case '"':
            sb.Append("\\\"");
            break;
          case '\\':
            sb.Append("\\\\");
            break;
          default:
            sb.Append("\\u").Append(((int) c).ToString("x4"));
            break;
        }
      }
      else
        sb.Append(c);
    }

    private static string stringifyImpl(
      string key,
      JSValue obj,
      Function replacer,
      HashSet<string> keys,
      string space,
      HashSet<JSValue> processed,
      Arguments args)
    {
      if (replacer != null)
      {
        args[0] = (JSValue) "";
        args[0]._oValue = (object) key;
        args[1] = obj;
        args._iValue = 2;
        JSValue jsValue = replacer.Call(args);
        if (jsValue._valueType >= JSValueType.Object && jsValue._oValue == null)
          return "null";
        if (jsValue._valueType <= JSValueType.Undefined)
          return (string) null;
        obj = jsValue;
      }
      if (!(obj.Value is JSValue jsValue1))
        jsValue1 = obj;
      obj = jsValue1;
      if (processed.Contains(obj))
        ExceptionHelper.Throw((Error) new TypeError("Unable to convert circular structure to JSON."));
      processed.Add(obj);
      try
      {
        if (obj._valueType < JSValueType.Object)
        {
          if (obj._valueType <= JSValueType.Undefined)
            return (string) null;
          if (obj._valueType == JSValueType.String)
          {
            StringBuilder sb = new StringBuilder("\"");
            foreach (char c in obj.ToString())
              JSON.escapeIfNeed(sb, c);
            sb.Append('"');
            return sb.ToString();
          }
          return obj.ValueType == JSValueType.Double && double.IsNaN(obj._dValue) || double.IsInfinity(obj._dValue) ? "null" : obj.ToString();
        }
        if (obj.Value == null || obj._valueType == JSValueType.Function)
          return (string) null;
        JSValue jsValue2 = obj["toJSON"];
        if (!(jsValue2.Value is JSValue jsValue3))
          jsValue3 = jsValue2;
        JSValue jsValue4 = jsValue3;
        if (jsValue4._valueType == JSValueType.Function)
          return JSON.stringifyImpl("", (jsValue4._oValue as Function).Call(obj, (Arguments) null), (Function) null, (HashSet<string>) null, space, processed, (Arguments) null);
        if (obj._valueType >= JSValueType.Object && !typeof (JSValue).GetTypeInfo().IsAssignableFrom(obj.Value.GetType().GetTypeInfo()))
        {
          GlobalContext currentGlobalContext = Context.CurrentGlobalContext;
          if (currentGlobalContext != null)
          {
            object obj1 = obj.Value;
            JsonSerializer suitableJsonSerializer = currentGlobalContext.JsonSerializersRegistry?.GetSuitableJsonSerializer(obj1);
            if (suitableJsonSerializer != null)
              return suitableJsonSerializer.Serialize(key, obj1, replacer, keys, space, processed);
          }
        }
        StringBuilder sb1 = new StringBuilder(obj is Array ? "[" : "{");
        string str1 = (string) null;
        foreach (KeyValuePair<string, JSValue> keyValuePair in obj)
        {
          if (keys == null || keys.Contains(keyValuePair.Key))
          {
            JSValue jsValue5 = keyValuePair.Value;
            if (!(jsValue5.Value is JSValue jsValue6))
              jsValue6 = jsValue5;
            JSValue property = jsValue6;
            if (property._valueType >= JSValueType.Undefined)
            {
              JSValue jsValue7 = Tools.InvokeGetter(property, obj);
              string str2 = JSON.stringifyImpl(keyValuePair.Key, jsValue7, replacer, (HashSet<string>) null, space, processed, args);
              if (str2 == null)
              {
                if (obj is Array)
                  str2 = "null";
                else
                  continue;
              }
              if (str1 != null)
                sb1.Append(",");
              if (space != null)
                sb1.Append(Environment.NewLine).Append(space);
              if (sb1[0] == '[')
              {
                int result;
                if (int.TryParse(keyValuePair.Key, out result))
                {
                  int num1 = int.Parse(str1 ?? "-1");
                  int capacity = sb1.Length + "null,".Length * (result - num1);
                  if (capacity > sb1.Length)
                    sb1.EnsureCapacity(capacity);
                  int num2 = result - 1;
                  while (num2-- > num1)
                    sb1.Append("null,");
                  sb1.Append(str2);
                  str1 = keyValuePair.Key;
                }
              }
              else
              {
                sb1.Append('"');
                for (int index = 0; index < keyValuePair.Key.Length; ++index)
                  JSON.escapeIfNeed(sb1, keyValuePair.Key[index]);
                sb1.Append("\":").Append(space == null ? string.Empty : " ");
                for (int index = 0; index < str2.Length; ++index)
                {
                  sb1.Append(str2[index]);
                  if (index >= Environment.NewLine.Length && str2.IndexOf(Environment.NewLine, index - Environment.NewLine.Length + 1, Environment.NewLine.Length) != -1)
                    sb1.Append(space);
                }
                str1 = keyValuePair.Key;
              }
            }
          }
        }
        if (space != null && str1 != null)
          sb1.Append(Environment.NewLine);
        return sb1.Append(obj is Array ? "]" : "}").ToString();
      }
      finally
      {
        processed.Remove(obj);
      }
    }

    private enum ParseState
    {
      Value,
      Name,
      Object,
      Array,
      End,
    }

    private class StackFrame
    {
      public JSValue container;
      public JSValue value;
      public JSValue fieldName;
      public int valuesCount;
      public JSON.ParseState state;
    }
  }
}
