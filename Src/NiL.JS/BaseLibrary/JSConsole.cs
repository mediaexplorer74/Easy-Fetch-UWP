// Decompiled with JetBrains decompiler
// Type: NiL.JS.BaseLibrary.JSConsole
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Core.Interop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NiL.JS.BaseLibrary
{
  public class JSConsole
  {
    private static Regex _lineSplitter;
    private Dictionary<string, int> _counters = new Dictionary<string, int>();
    private List<string> _groups = new List<string>();
    private Dictionary<string, Stopwatch> _timers = new Dictionary<string, Stopwatch>();
    private int _tableMaxColWidth = 100;

    [Hidden]
    public virtual int TableMaximumColumnWidth
    {
      get => this._tableMaxColWidth;
      set
      {
        if (value <= 0)
          ExceptionHelper.Throw((Error) new RangeError("TableMaximumColumnWidth needs to be at least 1"));
        this._tableMaxColWidth = value;
      }
    }

    [Hidden]
    public virtual TextWriter GetLogger(JSConsole.LogLevel ll) => ll == JSConsole.LogLevel.Error ? Console.Error : Console.Out;

    internal void LogArguments(JSConsole.LogLevel level, Arguments args) => this.LogArguments(level, args, 0);

    internal void LogArguments(JSConsole.LogLevel level, Arguments args, int argsStart)
    {
      if (args == null || args._iValue == 0 || args._iValue <= argsStart)
        return;
      this.LogMessage(level, Tools.FormatArgs((IEnumerable) args.Skip<KeyValuePair<string, JSValue>>(argsStart)));
    }

    [Hidden]
    public void LogMessage(JSConsole.LogLevel level, string message) => this.Print(level, this.GetLogger(level), message, this._groups.Count, "|   ");

    [Hidden]
    public void Print(JSConsole.LogLevel level, TextWriter textWriter, string message) => this.Print(level, textWriter, message, 0, "|   ");

    [Hidden]
    public void Print(
      JSConsole.LogLevel level,
      TextWriter textWriter,
      string message,
      int indent,
      string indentChar)
    {
      if (message == null || textWriter == null)
        return;
      if (indent > 0)
      {
        string str = "";
        for (int index = 0; index < indent; ++index)
          str += indentChar;
        int startIndex = 0;
        for (int index = 0; index < message.Length; ++index)
        {
          if (message[index] == '\n' || message[index] == '\r')
          {
            textWriter.WriteLine(str + message.Substring(startIndex, index - startIndex));
            if (message[index] == '\r' && index + 1 < message.Length && message[index + 1] == '\n')
              ++index;
            startIndex = index + 1;
          }
        }
        textWriter.WriteLine(str + message.Substring(startIndex));
      }
      else
        textWriter.WriteLine(message);
    }

    public JSValue assert(Arguments args)
    {
      if (!(bool) args[0])
        this.LogArguments(JSConsole.LogLevel.Log, args, 1);
      return JSValue.undefined;
    }

    public virtual JSValue clear(Arguments args)
    {
      this._groups.Clear();
      return JSValue.undefined;
    }

    public JSValue count(Arguments args)
    {
      string key = "";
      if (args._iValue > 0)
        key = (args[0] ?? (JSValue) "null").ToString();
      if (!this._counters.ContainsKey(key))
        this._counters.Add(key, 0);
      string str = Tools.Int32ToString(++this._counters[key]);
      if (key != "")
        key += ": ";
      this.LogMessage(JSConsole.LogLevel.Info, key + str);
      return JSValue.undefined;
    }

    public virtual JSValue debug(Arguments args)
    {
      this.LogArguments(JSConsole.LogLevel.Log, args);
      return JSValue.undefined;
    }

    public JSValue error(Arguments args)
    {
      this.LogArguments(JSConsole.LogLevel.Error, args);
      return JSValue.undefined;
    }

    public JSValue info(Arguments args)
    {
      this.LogArguments(JSConsole.LogLevel.Info, args);
      return JSValue.undefined;
    }

    public JSValue log(Arguments args)
    {
      this.LogArguments(JSConsole.LogLevel.Log, args);
      return JSValue.undefined;
    }

    public virtual JSValue table(Arguments args)
    {
      if (JSConsole._lineSplitter == null)
        JSConsole._lineSplitter = new Regex("\r\n?|\n");
      if (args[0] == null || !(args[0].Value is Array array1))
        return this.log(args);
      int length1 = (int) array1.length;
      if (length1 == 0)
        return this.log(args);
      HashSet<string> source1 = (HashSet<string>) null;
      if (args[1] != null && args[1].Value is Array array2 && (int) array2.length > 0)
      {
        source1 = new HashSet<string>();
        int length2 = (int) array2.length;
        for (int index = 0; index < length2; ++index)
          source1.Add(Tools.JSValueToString(array2[index]));
      }
      SortedDictionary<string, int> cols = new SortedDictionary<string, int>();
      List<Dictionary<string, string[]>> dictionaryList = new List<Dictionary<string, string[]>>();
      string key1 = "(index)";
      int val1_1 = key1.Length;
      for (int index = 0; index < length1; ++index)
      {
        JSValue jsValue = array1[index];
        if (jsValue != null && jsValue._valueType == JSValueType.Object)
        {
          Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
          foreach (KeyValuePair<string, JSValue> keyValuePair in jsValue)
          {
            if (source1 == null || source1.Contains(keyValuePair.Key))
            {
              string key2 = keyValuePair.Key ?? "";
              if (!cols.ContainsKey(key2))
                cols.Add(key2, key2.Length);
              int val1_2 = cols[key2];
              int num = System.Math.Max(cols[key2], this._tableMaxColWidth);
              string[] strArray = JSConsole._lineSplitter.Split(Tools.JSValueToObjectString(keyValuePair.Value, 0));
              List<string> source2 = new List<string>(strArray.Length);
              foreach (string str in strArray)
              {
                if (str.Length <= num)
                {
                  val1_2 = System.Math.Max(val1_2, str.Length);
                  source2.Add(str);
                }
                else
                {
                  val1_2 = num;
                  for (int startIndex = 0; startIndex < str.Length; startIndex += source2.Last<string>().Length)
                    source2.Add(str.Substring(startIndex, System.Math.Min(val1_2, str.Length - startIndex)));
                }
              }
              cols[key2] = val1_2;
              dictionary.Add(keyValuePair.Key, source2.ToArray());
            }
          }
          val1_1 = System.Math.Max(val1_1, index.ToString().Length);
          dictionary.Add(key1, new string[1]
          {
            index.ToString()
          });
          dictionaryList.Add(dictionary);
        }
      }
      if (dictionaryList.Count == 0 || cols.Count == 0)
        return this.log(args);
      List<string> stringList = new List<string>() { key1 };
      if (source1 != null)
        stringList.AddRange(source1.Where<string>((Func<string, bool>) (x => cols.ContainsKey(x))));
      else
        stringList.AddRange(cols.Select<KeyValuePair<string, int>, string>((Func<KeyValuePair<string, int>, string>) (x => x.Key)));
      cols.Add(key1, val1_1);
      int count1 = stringList.Count;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("+-");
      for (int index = 0; index < count1; ++index)
      {
        if (index > 0)
          stringBuilder.Append("-+-");
        int count2 = cols[stringList[index]];
        stringBuilder.Append(new string('-', count2));
      }
      stringBuilder.Append("-+\n");
      stringBuilder.Append("| ");
      for (int index = 0; index < count1; ++index)
      {
        if (index > 0)
          stringBuilder.Append(" | ");
        int totalWidth = cols[stringList[index]];
        stringBuilder.Append(stringList[index].PadRight(totalWidth));
      }
      stringBuilder.Append(" |\n");
      stringBuilder.Append("+-");
      for (int index = 0; index < count1; ++index)
      {
        if (index > 0)
          stringBuilder.Append("-+-");
        stringBuilder.Append(new string('-', cols[stringList[index]]));
      }
      stringBuilder.Append("-+\n");
      for (int index1 = 0; index1 < dictionaryList.Count; ++index1)
      {
        Dictionary<string, string[]> dictionary = dictionaryList[index1];
        bool flag = true;
        int index2 = 0;
        while (flag)
        {
          flag = false;
          stringBuilder.Append("| ");
          for (int index3 = 0; index3 < count1; ++index3)
          {
            if (index3 > 0)
              stringBuilder.Append(" | ");
            string key3 = stringList[index3];
            int totalWidth = cols[key3];
            string str = "";
            if (dictionary.ContainsKey(key3))
            {
              string[] strArray = dictionary[key3];
              if (index2 < strArray.Length)
                str = strArray[index2];
              if (strArray.Length > index2 + 1)
                flag = true;
            }
            stringBuilder.Append(str.PadRight(totalWidth));
          }
          stringBuilder.Append(" |\n");
          ++index2;
        }
      }
      stringBuilder.Append("+-");
      for (int index = 0; index < stringList.Count; ++index)
      {
        if (index > 0)
          stringBuilder.Append("-+-");
        stringBuilder.Append(new string('-', cols[stringList[index]]));
      }
      stringBuilder.Append("-+");
      this.LogMessage(JSConsole.LogLevel.Log, stringBuilder.ToString());
      return JSValue.undefined;
    }

    public JSValue trace(Arguments args)
    {
      Context context = Context.CurrentContext;
      StringBuilder stringBuilder = new StringBuilder();
      int num = 0;
      for (; context != null && context._parent != null; context = context._parent)
      {
        Function owner = context._owner;
        if (owner != null)
        {
          if (num++ > 0)
            stringBuilder.AppendLine();
          stringBuilder.Append(owner.name);
          if (owner._functionDefinition != null && owner._functionDefinition.Length > 0)
            stringBuilder.Append(" @" + owner._functionDefinition.Position.ToString());
        }
        else
          break;
      }
      if (args != null && args._iValue > 0)
      {
        this.group(args);
        this.LogMessage(JSConsole.LogLevel.Log, stringBuilder.ToString());
        this.groupEnd(args);
      }
      else
        this.LogMessage(JSConsole.LogLevel.Log, stringBuilder.ToString());
      return JSValue.undefined;
    }

    public JSValue warn(Arguments args)
    {
      this.LogArguments(JSConsole.LogLevel.Warn, args);
      return JSValue.undefined;
    }

    public virtual JSValue dir(Arguments args)
    {
      this.LogMessage(JSConsole.LogLevel.Log, Tools.JSValueToObjectString(args[0], 2));
      return JSValue.undefined;
    }

    public virtual JSValue dirxml(Arguments args) => this.dir(args);

    public virtual JSValue group(Arguments args)
    {
      string str = Tools.FormatArgs((IEnumerable) args);
      if (string.IsNullOrEmpty(str))
        str = "console.group";
      if (this._groups.Count > 0)
      {
        string group = this._groups[this._groups.Count - 1];
        this._groups.RemoveAt(this._groups.Count - 1);
        this.LogMessage(JSConsole.LogLevel.Info, "|---# " + str);
        this._groups.Add(group);
      }
      else
        this.LogMessage(JSConsole.LogLevel.Info, "# " + str);
      this._groups.Add(str);
      return JSValue.undefined;
    }

    public virtual JSValue groupCollapsed(Arguments args) => this.group(args);

    public virtual JSValue groupEnd(Arguments args)
    {
      if (this._groups.Count > 0)
        this._groups.RemoveAt(this._groups.Count - 1);
      return JSValue.undefined;
    }

    public JSValue time(Arguments args)
    {
      string key = "";
      if (args._iValue > 0)
        key = (args[0] ?? (JSValue) "null").ToString();
      if (this._timers.ContainsKey(key))
        this._timers[key].Restart();
      else
        this._timers.Add(key, Stopwatch.StartNew());
      return JSValue.undefined;
    }

    public JSValue timeEnd(Arguments args)
    {
      string key = "";
      if (args._iValue > 0)
        key = (args[0] ?? (JSValue) "null").ToString();
      double num = 0.0;
      if (this._timers.ContainsKey(key))
      {
        this._timers[key].Stop();
        num = (double) this._timers[key].ElapsedTicks / (double) Stopwatch.Frequency * 1000.0;
        this._timers.Remove(key);
      }
      if (key != "")
        key += ": ";
      this.LogMessage(JSConsole.LogLevel.Info, key + Tools.DoubleToString(System.Math.Round(num, 10)) + "ms");
      return JSValue.undefined;
    }

    [Hidden]
    public override bool Equals(object obj) => base.Equals(obj);

    [Hidden]
    public override int GetHashCode() => base.GetHashCode();

    [Hidden]
    public override string ToString() => base.ToString();

    [Hidden]
    public enum LogLevel
    {
      Log,
      Info,
      Warn,
      Error,
    }
  }
}
