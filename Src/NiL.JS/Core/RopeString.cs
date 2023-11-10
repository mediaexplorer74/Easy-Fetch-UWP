// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.RopeString
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NiL.JS.Core
{
  public sealed class RopeString : IEnumerable<char>, IEnumerable, IEquatable<string>
  {
    private int _length;
    private object _firstPart;
    private object _secondPart;

    private string firstPart
    {
      get
      {
        if (this._firstPart == null)
          return (string) null;
        return this._firstPart is string firstPart ? firstPart : (this._firstPart = (object) this._firstPart.ToString()) as string;
      }
    }

    private string secondPart
    {
      get
      {
        if (this._secondPart == null)
          return (string) null;
        return this._secondPart is string secondPart ? secondPart : (this._secondPart = (object) this._secondPart.ToString()) as string;
      }
    }

    public RopeString()
    {
      this._firstPart = (object) "";
      this._secondPart = (object) "";
    }

    public RopeString(object source)
    {
      this._firstPart = source ?? (object) "";
      this._secondPart = (object) "";
    }

    public RopeString(object firstSource, object secondSource)
    {
      this._firstPart = firstSource ?? (object) "";
      this._secondPart = secondSource ?? (object) "";
      this._length = this.calcLength();
      if (this._length >= 0)
        return;
      ExceptionHelper.Throw((Error) new RangeError("String is too large"));
    }

    public char this[int index]
    {
      get
      {
        if (this._firstPart != null)
        {
          if ((object) (this._firstPart as RopeString) != null && (this._firstPart as RopeString).Length < index)
            return (this._firstPart as RopeString)[index];
          if (this._firstPart is StringBuilder && (this._firstPart as StringBuilder).Length < index)
            return (this._firstPart as StringBuilder)[index];
          if (this.firstPart.Length < index)
            return this.firstPart[index];
        }
        if (this._secondPart == null)
          throw new ArgumentOutOfRangeException();
        if ((object) (this._secondPart as RopeString) != null)
          return (this._secondPart as RopeString)[index];
        return this._secondPart is StringBuilder ? (this._secondPart as StringBuilder)[index] : this.secondPart[index];
      }
    }

    public int Length => this._length;

    public object Clone() => (object) new RopeString(this._firstPart, this._secondPart);

    public int CompareTo(string strB) => this.ToString().CompareTo(strB);

    public bool Contains(string value) => this.ToString().Contains(value);

    public void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count) => this.ToString().CopyTo(sourceIndex, destination, destinationIndex, count);

    public bool EndsWith(string value) => this.ToString().EndsWith(value);

    public bool EndsWith(string value, StringComparison comparisonType) => this.ToString().EndsWith(value, comparisonType);

    public override bool Equals(object obj) => this.ToString().Equals(obj);

    public bool Equals(string value) => this.ToString().Equals(value);

    public bool Equals(string value, StringComparison comparisonType) => this.ToString().Equals(value, comparisonType);

    public override int GetHashCode() => this.ToString().GetHashCode();

    public int IndexOf(char value)
    {
      int num = this.firstPart.IndexOf(value);
      return num == -1 ? this.secondPart.IndexOf(value) : num;
    }

    public int IndexOf(string value)
    {
      int num = this.firstPart.IndexOf(value);
      return num == -1 ? this.secondPart.IndexOf(value) : num;
    }

    public int IndexOf(char value, int startIndex)
    {
      int num = this.firstPart.IndexOf(value, startIndex);
      return num == -1 ? this.secondPart.IndexOf(value, startIndex) : num;
    }

    public int IndexOf(string value, int startIndex)
    {
      int num = this.firstPart.IndexOf(value, startIndex);
      return num == -1 ? this.secondPart.IndexOf(value, startIndex) : num;
    }

    public int IndexOf(string value, StringComparison comparisonType)
    {
      int num = this.firstPart.IndexOf(value, comparisonType);
      return num == -1 ? this.secondPart.IndexOf(value, comparisonType) : num;
    }

    public int IndexOf(char value, int startIndex, int count)
    {
      int num = this.firstPart.IndexOf(value, startIndex, count);
      return num == -1 ? this.secondPart.IndexOf(value, startIndex, count) : num;
    }

    public int IndexOf(string value, int startIndex, int count)
    {
      int num = this.firstPart.IndexOf(value, startIndex, count);
      return num == -1 ? this.secondPart.IndexOf(value, startIndex, count) : num;
    }

    public int IndexOf(string value, int startIndex, StringComparison comparisonType)
    {
      int num = this.firstPart.IndexOf(value, startIndex, comparisonType);
      return num == -1 ? this.secondPart.IndexOf(value, startIndex, comparisonType) : num;
    }

    public int IndexOf(string value, int startIndex, int count, StringComparison comparisonType)
    {
      int num = this.firstPart.IndexOf(value, startIndex, count, comparisonType);
      return num == -1 ? this.secondPart.IndexOf(value, startIndex, count, comparisonType) : num;
    }

    public int IndexOfAny(char[] anyOf) => this.ToString().IndexOfAny(anyOf);

    public int IndexOfAny(char[] anyOf, int startIndex) => this.ToString().IndexOfAny(anyOf, startIndex);

    public int IndexOfAny(char[] anyOf, int startIndex, int count) => this.ToString().IndexOfAny(anyOf, startIndex, count);

    public string Insert(int startIndex, string value) => this.ToString().Insert(startIndex, value);

    public int LastIndexOf(char value)
    {
      int num = this.firstPart.LastIndexOf(value);
      return num == -1 ? this.secondPart.LastIndexOf(value) : num;
    }

    public int LastIndexOf(string value)
    {
      int num = this.firstPart.LastIndexOf(value);
      return num == -1 ? this.secondPart.LastIndexOf(value) : num;
    }

    public int LastIndexOf(char value, int startIndex)
    {
      int num = this.firstPart.LastIndexOf(value, startIndex);
      return num == -1 ? this.secondPart.LastIndexOf(value, startIndex) : num;
    }

    public int LastIndexOf(string value, int startIndex)
    {
      int num = this.firstPart.LastIndexOf(value, startIndex);
      return num == -1 ? this.secondPart.LastIndexOf(value, startIndex) : num;
    }

    public int LastIndexOf(string value, StringComparison comparisonType)
    {
      int num = this.firstPart.LastIndexOf(value, comparisonType);
      return num == -1 ? this.secondPart.LastIndexOf(value, comparisonType) : num;
    }

    public int LastIndexOf(char value, int startIndex, int count)
    {
      int num = this.firstPart.LastIndexOf(value, startIndex, count);
      return num == -1 ? this.secondPart.LastIndexOf(value, startIndex, count) : num;
    }

    public int LastIndexOf(string value, int startIndex, int count)
    {
      int num = this.firstPart.LastIndexOf(value, startIndex, count);
      return num == -1 ? this.secondPart.LastIndexOf(value, startIndex, count) : num;
    }

    public int LastIndexOf(string value, int startIndex, StringComparison comparisonType)
    {
      int num = this.firstPart.LastIndexOf(value, startIndex, comparisonType);
      return num == -1 ? this.secondPart.LastIndexOf(value, startIndex, comparisonType) : num;
    }

    public int LastIndexOf(
      string value,
      int startIndex,
      int count,
      StringComparison comparisonType)
    {
      int num = this.firstPart.LastIndexOf(value, startIndex, count, comparisonType);
      return num == -1 ? this.secondPart.LastIndexOf(value, startIndex, count, comparisonType) : num;
    }

    public int LastIndexOfAny(char[] anyOf) => this.ToString().LastIndexOfAny(anyOf);

    public int LastIndexOfAny(char[] anyOf, int startIndex) => this.ToString().LastIndexOfAny(anyOf, startIndex);

    public int LastIndexOfAny(char[] anyOf, int startIndex, int count) => this.ToString().LastIndexOfAny(anyOf, startIndex, count);

    public string PadLeft(int totalWidth) => this.ToString().PadLeft(totalWidth);

    public string PadLeft(int totalWidth, char paddingChar) => this.ToString().PadLeft(totalWidth, paddingChar);

    public string PadRight(int totalWidth) => this.ToString().PadRight(totalWidth);

    public string PadRight(int totalWidth, char paddingChar) => this.ToString().PadRight(totalWidth, paddingChar);

    public string Remove(int startIndex) => this.ToString().Remove(startIndex);

    public string Remove(int startIndex, int count) => this.ToString().Remove(startIndex, count);

    public string Replace(char oldChar, char newChar) => this.ToString().Replace(oldChar, newChar);

    public string Replace(string oldValue, string newValue) => this.ToString().Replace(oldValue, newValue);

    public string[] Split(params char[] separator) => this.ToString().Split(separator);

    public string[] Split(char[] separator, int count) => this.ToString().Split(separator, count);

    public string[] Split(char[] separator, StringSplitOptions options) => this.ToString().Split(separator, options);

    public string[] Split(string[] separator, StringSplitOptions options) => this.ToString().Split(separator, options);

    public string[] Split(char[] separator, int count, StringSplitOptions options) => this.ToString().Split(separator, count, options);

    public string[] Split(string[] separator, int count, StringSplitOptions options) => this.ToString().Split(separator, count, options);

    public bool StartsWith(string value) => this.ToString().StartsWith(value);

    public bool StartsWith(string value, StringComparison comparisonType) => this.ToString().StartsWith(value, comparisonType);

    public string Substring(int startIndex) => this.ToString().Substring(startIndex);

    public string Substring(int startIndex, int length) => this.ToString().Substring(startIndex, length);

    public char[] ToCharArray() => this.ToString().ToCharArray();

    public char[] ToCharArray(int startIndex, int length) => this.ToString().ToCharArray(startIndex, length);

    public string ToLower() => this.ToString().ToLower();

    public string ToLowerInvariant() => this.ToString().ToLowerInvariant();

    private static void _append(StringBuilder sb, object arg)
    {
      string str = arg.ToString();
      int length = sb.Length;
      if (sb.Capacity < length + str.Length)
        sb.EnsureCapacity(System.Math.Max(sb.Capacity << 1, length + str.Length));
      sb.Length += str.Length;
      for (int index = 0; index < str.Length; ++index)
        sb[length + index] = str[index];
    }

    public override string ToString()
    {
      if (this._secondPart != null)
      {
        if ((object) (this._firstPart as RopeString) == null && (object) (this._secondPart as RopeString) == null)
        {
          this._firstPart = (object) (this.firstPart + this.secondPart);
          this._secondPart = (object) null;
        }
        else
        {
          Stack<RopeString> ropeStringStack = new Stack<RopeString>();
          Stack<int> intStack = new Stack<int>();
          StringBuilder sb = new StringBuilder(this.Length);
          ropeStringStack.Push(this);
          intStack.Push(0);
          while (ropeStringStack.Count != 0)
          {
            if (intStack.Peek() < 1)
            {
              if ((object) (ropeStringStack.Peek()._firstPart as RopeString) != null)
              {
                RopeString firstPart = ropeStringStack.Peek()._firstPart as RopeString;
                ropeStringStack.Push(firstPart);
                intStack.Pop();
                intStack.Push(1);
                intStack.Push(0);
                continue;
              }
              RopeString._append(sb, (object) (ropeStringStack.Peek().firstPart ?? ""));
              intStack.Pop();
              intStack.Push(1);
            }
            if (intStack.Peek() < 2)
            {
              if ((object) (ropeStringStack.Peek()._secondPart as RopeString) != null)
              {
                RopeString secondPart = ropeStringStack.Peek()._secondPart as RopeString;
                ropeStringStack.Push(secondPart);
                intStack.Pop();
                intStack.Push(2);
                intStack.Push(0);
                continue;
              }
              RopeString._append(sb, (object) (ropeStringStack.Peek().secondPart ?? ""));
              intStack.Pop();
              intStack.Push(2);
            }
            ropeStringStack.Pop();
            intStack.Pop();
          }
          this._firstPart = (object) sb.ToString();
          this._secondPart = (object) null;
        }
      }
      return this.firstPart;
    }

    public string ToString(IFormatProvider provider) => this.ToString();

    public string ToUpper() => this.ToString().ToUpper();

    public string ToUpperInvariant() => this.ToString().ToUpperInvariant();

    public string Trim() => this.ToString().Trim();

    public string Trim(params char[] trimChars) => this.ToString().Trim(trimChars);

    public string TrimEnd(params char[] trimChars) => this.ToString().TrimEnd(trimChars);

    public string TrimStart(params char[] trimChars) => this.ToString().TrimStart(trimChars);

    public static bool operator ==(RopeString left, RopeString right)
    {
      if ((object) left == (object) right)
        return true;
      return (object) left != null && (object) right != null && left.ToString() == right.ToString();
    }

    public static bool operator !=(RopeString left, RopeString rigth) => !(left == rigth);

    public static RopeString operator +(RopeString left, RopeString rigth) => new RopeString((object) left, (object) rigth);

    public static RopeString operator +(RopeString left, string rigth) => new RopeString((object) left, (object) rigth);

    public static RopeString operator +(string left, RopeString rigth) => new RopeString((object) left, (object) rigth);

    IEnumerator<char> IEnumerable<char>.GetEnumerator() => throw new NotImplementedException();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) ((IEnumerable<char>) this).GetEnumerator();

    private int calcLength()
    {
      int num = 0;
      if (this._firstPart != null)
      {
        RopeString firstPart1 = this._firstPart as RopeString;
        num = !(firstPart1 != (RopeString) null) ? (!(this._firstPart is StringBuilder firstPart2) ? this.firstPart.Length : firstPart2.Length) : firstPart1.Length;
      }
      if (this._secondPart != null)
      {
        RopeString secondPart1 = this._secondPart as RopeString;
        if (secondPart1 != (RopeString) null)
          num += secondPart1.Length;
        else if (this._secondPart is StringBuilder secondPart2)
          num += secondPart2.Length;
        else
          num += this.secondPart.Length;
      }
      return num;
    }
  }
}
