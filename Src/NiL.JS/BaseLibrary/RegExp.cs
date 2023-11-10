// Decompiled with JetBrains decompiler
// Type: NiL.JS.BaseLibrary.RegExp
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
  public sealed class RegExp : CustomType
  {
    private static RegExp.RegExpCacheItem[] _cache = new RegExp.RegExpCacheItem[16];
    private static int _cacheIndex = -1;
    private const int _cacheSize = 16;
    private string _source;
    private JSValue _lastIndex;
    internal Regex _regex;
    internal bool _global;
    internal bool _sticky;
    internal bool _unicode;

    [DoNotEnumerate]
    public RegExp()
    {
      this._source = "";
      this._global = false;
      this._sticky = false;
      this._unicode = false;
      this._regex = new Regex("");
    }

    private void makeRegex(Arguments args)
    {
      JSValue jsValue = args[0];
      if (jsValue._valueType == JSValueType.Object && jsValue.Value is RegExp)
      {
        if (args.GetProperty("length")._iValue > 1 && args[1]._valueType > JSValueType.Undefined)
          ExceptionHelper.Throw((Error) new TypeError("Cannot supply flags when constructing one RegExp from another"));
        this._oValue = jsValue._oValue;
        this._regex = (jsValue.Value as RegExp)._regex;
        this._global = (jsValue.Value as RegExp)._global;
        this._sticky = (jsValue.Value as RegExp)._sticky;
        this._unicode = (jsValue.Value as RegExp)._unicode;
        this._source = (jsValue.Value as RegExp)._source;
      }
      else
        this.makeRegex(jsValue._valueType > JSValueType.Undefined ? jsValue.ToString() : "", args.GetProperty("length")._iValue <= 1 || args[1]._valueType <= JSValueType.Undefined ? "" : args[1].ToString());
    }

    private void makeRegex(string pattern, string flags)
    {
      pattern = pattern ?? "null";
      flags = flags ?? "null";
      this._global = false;
      this._sticky = false;
      this._unicode = false;
      try
      {
        RegexOptions options = RegexOptions.ECMAScript | RegexOptions.CultureInvariant;
        char flag;
        for (int index = 0; index < flags.Length; ++index)
        {
          switch (flags[index])
          {
            case 'g':
              if (this._global)
              {
                flag = flags[index];
                ExceptionHelper.Throw((Error) new SyntaxError("Try to double use RegExp flag \"" + flag.ToString() + "\""));
              }
              this._global = true;
              break;
            case 'i':
              if ((options & RegexOptions.IgnoreCase) != RegexOptions.None)
              {
                flag = flags[index];
                ExceptionHelper.Throw((Error) new SyntaxError("Try to double use RegExp flag \"" + flag.ToString() + "\""));
              }
              options |= RegexOptions.IgnoreCase;
              break;
            case 'm':
              if ((options & RegexOptions.Multiline) != RegexOptions.None)
              {
                flag = flags[index];
                ExceptionHelper.Throw((Error) new SyntaxError("Try to double use RegExp flag \"" + flag.ToString() + "\""));
              }
              options |= RegexOptions.Multiline;
              break;
            case 'u':
              if (this._unicode)
              {
                flag = flags[index];
                ExceptionHelper.Throw((Error) new SyntaxError("Try to double use RegExp flag \"" + flag.ToString() + "\""));
              }
              this._unicode = true;
              break;
            case 'y':
              if (this._sticky)
              {
                flag = flags[index];
                ExceptionHelper.Throw((Error) new SyntaxError("Try to double use RegExp flag \"" + flag.ToString() + "\""));
              }
              this._sticky = true;
              break;
            default:
              flag = flags[index];
              ExceptionHelper.Throw((Error) new SyntaxError("Invalid RegExp flag \"" + flag.ToString() + "\""));
              break;
          }
        }
        this._source = pattern;
        string str = this._source + "/" + ((options & RegexOptions.IgnoreCase) != RegexOptions.None ? "i" : "") + ((options & RegexOptions.Multiline) != RegexOptions.None ? "m" : "") + (this._unicode ? "u" : "");
        lock (RegExp._cache)
        {
          if (RegExp._cacheIndex >= 0)
          {
            int num = 15;
            for (int index = 16 + RegExp._cacheIndex; index > RegExp._cacheIndex; --index)
            {
              if (RegExp._cache[index & num].key == str)
              {
                this._regex = RegExp._cache[index & num].re;
                return;
              }
            }
          }
          pattern = Tools.Unescape(pattern, false, false, true, this._unicode);
          if (this._unicode)
            pattern = RegExp.translateToUnicodePattern(pattern);
          this._regex = new Regex(pattern, options);
          RegExp._cacheIndex = (RegExp._cacheIndex + 1) % 16;
          RegExp._cache[RegExp._cacheIndex].key = str;
          RegExp._cache[RegExp._cacheIndex].re = this._regex;
        }
      }
      catch (ArgumentException ex)
      {
        ExceptionHelper.Throw((Error) new SyntaxError(ex.Message));
      }
    }

    private static string translateToUnicodePattern(string pattern)
    {
      if (pattern == null || pattern == "")
        return "";
      StringBuilder stringBuilder = new StringBuilder(pattern.Length);
      for (int index1 = 0; index1 < pattern.Length; ++index1)
      {
        char ch1 = pattern[index1];
        if (ch1 == '.')
          stringBuilder.Append("(?:[\uD800-\uDBFF][\uDC00-\uDFFF]|.)");
        else if (ch1 == '[' && index1 + 1 < pattern.Length)
        {
          int index2;
          for (index2 = index1 + 1; index1 < pattern.Length && pattern[index2] != ']'; ++index2)
          {
            if (pattern[index2] == '\\')
              ++index2;
          }
          if (index2 >= pattern.Length)
          {
            stringBuilder.Append('[');
          }
          else
          {
            bool inverted = pattern[index1 + 1] == '^';
            if (inverted)
              ++index1;
            stringBuilder.Append(RegExp.translateCharSet(pattern.Substring(index1 + 1, index2 - index1 - 1), inverted));
            index1 = index2;
          }
        }
        else if (ch1 == '\\' && index1 + 1 < pattern.Length)
        {
          char ch2 = pattern[++index1];
          switch (ch2)
          {
            case 'D':
            case 'S':
            case 'W':
              stringBuilder.Append("(?:[\uD800-\uDBFF][\uDC00-\uDFFF]|\\" + ch2.ToString() + ")");
              continue;
            default:
              stringBuilder.Append('\\').Append(ch2);
              continue;
          }
        }
        else if (Tools.IsSurrogatePair(pattern, index1))
          stringBuilder.Append("(?:").Append(ch1).Append(pattern[++index1]).Append(')');
        else
          stringBuilder.Append(ch1);
      }
      return stringBuilder.ToString();
    }

    private static string translateCharSet(string set, bool inverted)
    {
      RegExp.CharRange[] charRangeArray = RegExp.analyzeCharSet(set);
      if (inverted)
        charRangeArray = RegExp.invertCharSet(charRangeArray);
      if (charRangeArray.Length == 0)
        return "[]";
      if (charRangeArray.Length == 1 && charRangeArray[0].start == 0 && charRangeArray[0].stop == RegExp.CharRange.MaxValue)
        return "(?:[\\uD800-\\uDBFF][\\uDC00-\\uDFFF]|[\\s\\S])";
      List<RegExp.CharRange> charRangeList1 = new List<RegExp.CharRange>((IEnumerable<RegExp.CharRange>) charRangeArray);
      for (int index = charRangeList1.Count - 1; index >= 0; --index)
      {
        if (charRangeList1[index].start > (int) ushort.MaxValue)
        {
          charRangeList1.RemoveAt(index);
        }
        else
        {
          if (charRangeList1[index].stop > (int) ushort.MaxValue)
          {
            charRangeList1[index] = new RegExp.CharRange(charRangeList1[index].start, (int) ushort.MaxValue);
            break;
          }
          break;
        }
      }
      List<RegExp.CharRange> charRangeList2 = new List<RegExp.CharRange>(charRangeArray.Length - charRangeList1.Count + 1);
      for (int index = 0; index < charRangeArray.Length; ++index)
      {
        if (charRangeArray[index].start > (int) ushort.MaxValue)
          charRangeList2.Add(charRangeArray[index]);
        else if (charRangeArray[index].stop > (int) ushort.MaxValue)
          charRangeList2.Add(new RegExp.CharRange(65536, charRangeArray[index].stop));
      }
      StringBuilder stringBuilder = new StringBuilder("(?:");
      if (charRangeList2.Count > 0)
      {
        stringBuilder.Append("(?:");
        for (int index = 0; index < charRangeList2.Count; ++index)
        {
          if (index > 0)
            stringBuilder.Append('|');
          RegExp.CharRange charRange = charRangeList2[index];
          if (charRange.start == charRange.stop)
          {
            stringBuilder.Append(Tools.CodePointToString(charRange.start));
          }
          else
          {
            string str1 = Tools.CodePointToString(charRange.start);
            string str2 = Tools.CodePointToString(charRange.stop);
            if ((int) str1[0] == (int) str2[0])
            {
              stringBuilder.Append(str1[0]).Append('[').Append(str1[1]).Append('-').Append(str2[1]).Append(']');
            }
            else
            {
              int num1 = str1[1] > '\uDC00' ? 1 : 0;
              int num2 = str2[1] < '\uDFFF' ? 1 : 0;
              if (num1 != 0)
                stringBuilder.Append(str1[0]).Append('[').Append(str1[1]).Append("-\uDFFF]|");
              if ((int) str2[0] - (int) str1[0] >= num1 + num2)
              {
                stringBuilder.Append('[');
                stringBuilder.Append((char) ((uint) str1[0] + (uint) num1));
                stringBuilder.Append('-');
                stringBuilder.Append((char) ((uint) str2[0] - (uint) num2));
                stringBuilder.Append(']');
                stringBuilder.Append("[\uDC00-\uDFFF]|");
              }
              if (num2 != 0)
                stringBuilder.Append(str2[0]).Append("[\uDC00-").Append(str2[1]).Append(']');
            }
          }
        }
        stringBuilder.Append(")");
      }
      if (charRangeList1.Count > 0)
      {
        if (charRangeList2.Count > 0)
          stringBuilder.Append('|');
        stringBuilder.Append("[");
        for (int index = 0; index < charRangeList1.Count; ++index)
        {
          RegExp.CharRange charRange = charRangeList1[index];
          if (charRange.start == charRange.stop)
          {
            stringBuilder.Append("\\u").Append(charRange.start.ToString("X4"));
          }
          else
          {
            stringBuilder.Append("\\u").Append(charRange.start.ToString("X4"));
            if (charRange.stop > charRange.start + 1)
              stringBuilder.Append('-');
            stringBuilder.Append("\\u").Append(charRange.stop.ToString("X4"));
          }
        }
        stringBuilder.Append("]");
      }
      stringBuilder.Append(")");
      return stringBuilder.ToString();
    }

    private static RegExp.CharRange[] analyzeCharSet(string set)
    {
      List<RegExp.CharRange> charRangeList1 = new List<RegExp.CharRange>();
      for (int i = 0; i < set.Length; ++i)
      {
        char ch = set[i];
        int num = Tools.NextCodePoint(set, ref i);
        if (ch == '\\' && i + 1 < set.Length)
        {
          switch (set[++i])
          {
            case 'D':
              charRangeList1.Add(new RegExp.CharRange(0, 47));
              charRangeList1.Add(new RegExp.CharRange(58, RegExp.CharRange.MaxValue));
              continue;
            case 'S':
              charRangeList1.Add(new RegExp.CharRange(0, 8));
              charRangeList1.Add(new RegExp.CharRange(11, 12));
              charRangeList1.Add(new RegExp.CharRange(14, 31));
              charRangeList1.Add(new RegExp.CharRange(33, RegExp.CharRange.MaxValue));
              continue;
            case 'W':
              charRangeList1.Add(new RegExp.CharRange(0, 47));
              charRangeList1.Add(new RegExp.CharRange(58, 64));
              charRangeList1.Add(new RegExp.CharRange(91, 94));
              charRangeList1.Add(new RegExp.CharRange(96, 96));
              charRangeList1.Add(new RegExp.CharRange(123, RegExp.CharRange.MaxValue));
              continue;
            case 'd':
              charRangeList1.Add(new RegExp.CharRange(48, 57));
              continue;
            case 's':
              charRangeList1.Add(new RegExp.CharRange(9, 10));
              charRangeList1.Add(new RegExp.CharRange(13, 13));
              charRangeList1.Add(new RegExp.CharRange(32, 32));
              continue;
            case 'w':
              charRangeList1.Add(new RegExp.CharRange(48, 57));
              charRangeList1.Add(new RegExp.CharRange(65, 90));
              charRangeList1.Add(new RegExp.CharRange(95, 95));
              charRangeList1.Add(new RegExp.CharRange(97, 122));
              continue;
            default:
              --i;
              break;
          }
        }
        if (i + 2 < set.Length && set[i + 1] == '-')
        {
          i += 2;
          int stop = Tools.NextCodePoint(set, ref i, true);
          if (stop < num)
            ExceptionHelper.Throw((Error) new SyntaxError("Range out of order in character class"));
          charRangeList1.Add(new RegExp.CharRange(num, stop));
        }
        else
          charRangeList1.Add(new RegExp.CharRange(num, num));
      }
      if (charRangeList1.Count <= 1)
        return charRangeList1.ToArray();
      charRangeList1.Sort(new Comparison<RegExp.CharRange>(((Func<RegExp.CharRange, RegExp.CharRange, int>) ((x, y) => x.start - y.start)).Invoke));
      List<RegExp.CharRange> charRangeList2 = new List<RegExp.CharRange>();
      RegExp.CharRange charRange = charRangeList1[0];
      for (int index = 1; index < charRangeList1.Count; ++index)
      {
        if (charRangeList1[index].stop > charRange.stop)
        {
          if (charRange.stop >= charRangeList1[index].start - 1)
          {
            charRange.stop = charRangeList1[index].stop;
          }
          else
          {
            charRangeList2.Add(charRange);
            charRange = charRangeList1[index];
          }
        }
      }
      charRangeList2.Add(charRange);
      return charRangeList2.ToArray();
    }

    private static RegExp.CharRange[] invertCharSet(RegExp.CharRange[] set)
    {
      if (set.Length == 0)
        return new RegExp.CharRange[1]
        {
          new RegExp.CharRange(0, RegExp.CharRange.MaxValue)
        };
      List<RegExp.CharRange> charRangeList = new List<RegExp.CharRange>();
      if (set[0].start > 0)
        charRangeList.Add(new RegExp.CharRange(0, set[0].start - 1));
      for (int index = 1; index < set.Length; ++index)
        charRangeList.Add(new RegExp.CharRange(set[index - 1].stop + 1, set[index].start - 1));
      if (set[set.Length - 1].stop < RegExp.CharRange.MaxValue)
        charRangeList.Add(new RegExp.CharRange(set[set.Length - 1].stop + 1, RegExp.CharRange.MaxValue));
      return charRangeList.ToArray();
    }

    [DoNotEnumerate]
    public RegExp(Arguments args) => this.makeRegex(args);

    [DoNotEnumerate]
    public RegExp(string pattern, string flags) => this.makeRegex(pattern, flags);

    [Field]
    [ReadOnly]
    [DoNotDelete]
    [DoNotEnumerate]
    [NotConfigurable]
    public Boolean global
    {
      [Hidden] get => (Boolean) this._global;
    }

    [Field]
    [ReadOnly]
    [DoNotDelete]
    [DoNotEnumerate]
    [NotConfigurable]
    public Boolean ignoreCase => (Boolean) ((this._regex.Options & RegexOptions.IgnoreCase) != 0);

    [Field]
    [ReadOnly]
    [DoNotDelete]
    [DoNotEnumerate]
    [NotConfigurable]
    public Boolean multiline => (Boolean) ((this._regex.Options & RegexOptions.Multiline) != 0);

    [Field]
    [ReadOnly]
    [DoNotDelete]
    [DoNotEnumerate]
    [NotConfigurable]
    public Boolean sticky
    {
      [Hidden] get => (Boolean) this._sticky;
    }

    [Field]
    [ReadOnly]
    [DoNotDelete]
    [DoNotEnumerate]
    [NotConfigurable]
    public Boolean unicode
    {
      [Hidden] get => (Boolean) this._unicode;
    }

    [Field]
    [ReadOnly]
    [DoNotDelete]
    [DoNotEnumerate]
    [NotConfigurable]
    public String source => new String(this._source);

    [Field]
    [DoNotDelete]
    [DoNotEnumerate]
    [NotConfigurable]
    public JSValue lastIndex
    {
      get => this._lastIndex ?? (this._lastIndex = (JSValue) 0);
      set => this._lastIndex = (value ?? JSValue.undefined).CloneImpl(false);
    }

    [DoNotEnumerate]
    public RegExp compile(Arguments args)
    {
      this.makeRegex(args);
      return this;
    }

    [DoNotEnumerate]
    public JSValue exec(JSValue arg)
    {
      string input = (arg ?? (JSValue) "null").ToString();
      if (!this._global && !this._sticky)
      {
        Match match = this._regex.Match(input);
        if (!match.Success)
          return (JSValue) JSValue.@null;
        Array array = new Array(match.Groups.Count);
        for (int index = 0; index < match.Groups.Count; ++index)
          array._data[index] = match.Groups[index].Success ? (JSValue) match.Groups[index].Value : (JSValue) null;
        array.DefineProperty("index").Assign((JSValue) match.Index);
        array.DefineProperty("input").Assign((JSValue) input);
        return (JSValue) array;
      }
      this._lastIndex = Tools.JSObjectToNumber(this._lastIndex);
      if ((this._lastIndex._attributes & JSValueAttributesInternal.SystemObject) != JSValueAttributesInternal.None)
        this._lastIndex = this._lastIndex.CloneImpl(false);
      if (this._lastIndex._valueType == JSValueType.Double)
      {
        this._lastIndex._valueType = JSValueType.Integer;
        this._lastIndex._iValue = (int) this._lastIndex._dValue;
      }
      int iValue = this._lastIndex._iValue < 0 ? 0 : this._lastIndex._iValue;
      this._lastIndex._iValue = 0;
      if (iValue >= input.Length && input.Length > 0)
        return (JSValue) JSValue.@null;
      Match match1 = this._regex.Match(input, iValue);
      if (!match1.Success || this._sticky && match1.Index != iValue)
        return (JSValue) JSValue.@null;
      Array array1 = new Array(match1.Groups.Count);
      for (int index = 0; index < match1.Groups.Count; ++index)
        array1._data[index] = match1.Groups[index].Success ? (JSValue) match1.Groups[index].Value : (JSValue) null;
      this._lastIndex._iValue = match1.Index + match1.Length;
      array1.DefineProperty("index").Assign((JSValue) match1.Index);
      array1.DefineProperty("input").Assign((JSValue) input);
      return (JSValue) array1;
    }

    [DoNotEnumerate]
    public JSValue test(JSValue arg)
    {
      string input = (arg ?? (JSValue) "null").ToString();
      if (!this._global && !this._sticky)
        return (JSValue) this._regex.IsMatch(input);
      this._lastIndex = Tools.JSObjectToNumber(this._lastIndex);
      if ((this._lastIndex._attributes & JSValueAttributesInternal.SystemObject) != JSValueAttributesInternal.None)
        this._lastIndex = this._lastIndex.CloneImpl(false);
      if (this._lastIndex._valueType == JSValueType.Double)
      {
        this._lastIndex._valueType = JSValueType.Integer;
        this._lastIndex._iValue = (int) this._lastIndex._dValue;
      }
      int iValue = this._lastIndex._iValue < 0 ? 0 : this._lastIndex._iValue;
      this._lastIndex._iValue = 0;
      if (iValue >= input.Length && input.Length > 0)
        return (JSValue) false;
      Match match = this._regex.Match(input, iValue);
      if (!match.Success || this._sticky && match.Index != iValue)
        return (JSValue) false;
      this._lastIndex._iValue = match.Index + match.Length;
      return (JSValue) true;
    }

    [CLSCompliant(false)]
    [DoNotEnumerate]
    public JSValue toString() => (JSValue) this.ToString();

    [Hidden]
    public override string ToString() => "/" + this._source + "/" + (this._global ? "g" : "") + ((this._regex.Options & RegexOptions.IgnoreCase) != RegexOptions.None ? "i" : "") + ((this._regex.Options & RegexOptions.Multiline) != RegexOptions.None ? "m" : "") + (this._unicode ? "u" : "") + (this._sticky ? "y" : "");

    private struct RegExpCacheItem
    {
      public string key;
      public Regex re;

      public RegExpCacheItem(string key, Regex re)
      {
        this.key = key;
        this.re = re;
      }
    }

    private struct CharRange
    {
      public int start;
      public int stop;
      public static int MaxValue = 1114111;
      public static int MinValue = 0;

      public CharRange(int start, int stop)
      {
        this.start = start;
        this.stop = stop;
      }
    }
  }
}
