// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.StringMap`1
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Backward;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace NiL.JS.Core
{
  [DebuggerDisplay("Count = {Count}")]
  [DebuggerTypeProxy(typeof (StringMapDebugView<>))]
  public class StringMap<TValue> : 
    IDictionary<string, TValue>,
    ICollection<KeyValuePair<string, TValue>>,
    IEnumerable<KeyValuePair<string, TValue>>,
    IEnumerable
  {
    private const int InitialSize = 2;
    private const int MaxAsListSize = 4;
    private int _version;
    private int _count;
    private int _eicount;
    private int _previousIndex;
    private StringMap<TValue>.Record[] _records = EmpryArrayHelper.Empty<StringMap<TValue>.Record>();
    private int[] _existsedIndexes;
    private bool _emptyKeyValueExists;
    private TValue _emptyKeyValue;

    public StringMap() => this.Clear();

    private void insert(string key, TValue value, int hash, bool @throw, bool allowIncrease)
    {
      if (key == null)
        ExceptionHelper.ThrowArgumentNull(nameof (key));
      if (key.Length == 0)
      {
        if (@throw && this._emptyKeyValueExists)
          ExceptionHelper.Throw((Exception) new InvalidOperationException("Item already exists"));
        this._emptyKeyValueExists = true;
        this._emptyKeyValue = value;
      }
      else
      {
        int num1 = 0;
        int num2 = this._records.Length - 1;
        if (this._records.Length == 0)
          num2 = this.increaseSize() - 1;
        if (this._records.Length <= 4)
        {
          for (int index = 0; index < this._records.Length; ++index)
          {
            if (this._records[index].key == null)
            {
              this._records[index].hash = -1;
              this._records[index].key = key;
              this._records[index].value = value;
              this.ensureExistedIndexCapacity();
              this._existsedIndexes[this._eicount] = index;
              ++this._count;
              ++this._eicount;
              ++this._version;
              return;
            }
            if (string.CompareOrdinal(this._records[index].key, key) == 0)
            {
              if (@throw)
                ExceptionHelper.Throw((Exception) new InvalidOperationException("Item already exists"));
              this._records[index].value = value;
              return;
            }
          }
          if (this._records.Length * 2 <= 4)
          {
            int length = this._records.Length;
            this.increaseSize();
            this._records[length].hash = -1;
            this._records[length].key = key;
            this._records[length].value = value;
            this.ensureExistedIndexCapacity();
            this._existsedIndexes[this._eicount] = length;
            ++this._count;
            ++this._eicount;
            ++this._version;
            return;
          }
        }
        else
        {
          int index = hash & num2;
          while (this._records[index].hash != hash || string.CompareOrdinal(this._records[index].key, key) != 0)
          {
            index = this._records[index].next - 1;
            if (index < 0)
              goto label_26;
          }
          if (@throw)
            ExceptionHelper.Throw((Exception) new InvalidOperationException("Item already Exists"));
          this._records[index].value = value;
          return;
        }
label_26:
        if (allowIncrease && (this._count == num2 + 1 || this._count > 50 && this._count * 8 / 5 >= num2))
          num2 = this.increaseSize() - 1;
        int index1 = -1;
        int index2 = hash & num2;
        if (this._records[index2].key != null)
        {
          while (this._records[index2].next > 0)
          {
            index2 = this._records[index2].next - 1;
            ++num1;
          }
          index1 = index2;
          while (this._records[index2].key != null)
            index2 = index2 + 3 & num2;
        }
        this._records[index2].hash = hash;
        this._records[index2].key = key;
        this._records[index2].value = value;
        if (index1 >= 0)
          this._records[index1].next = index2 + 1;
        this.ensureExistedIndexCapacity();
        this._existsedIndexes[this._eicount] = index2;
        ++this._eicount;
        ++this._count;
        ++this._version;
        if (!(num1 > 17 & allowIncrease))
          return;
        this.increaseSize();
      }
    }

    private void ensureExistedIndexCapacity()
    {
      if (this._eicount != this._existsedIndexes.Length)
        return;
      int[] destinationArray = new int[this._existsedIndexes.Length << 1];
      Array.Copy((Array) this._existsedIndexes, (Array) destinationArray, this._existsedIndexes.Length);
      this._existsedIndexes = destinationArray;
    }

    private static int computeHash(string key)
    {
      int length = key.Length;
      int num1 = int.MinValue & -length;
      int num2 = length * 197379 ^ 12040119;
      for (int index = 0; index < length; ++index)
      {
        char ch = key[index];
        int num3 = num2;
        num2 = num2 >> 3 ^ num3 * 22085937 ^ (int) ch * 886589569;
        num1 &= 47 - (int) ch & (int) ch - 57 - 1;
      }
      return num2 & int.MaxValue | num1;
    }

    public bool TryGetValue(string key, out TValue value)
    {
      switch (key)
      {
        case null:
          throw new ArgumentNullException(nameof (key));
        case "":
          if (!this._emptyKeyValueExists)
          {
            value = default (TValue);
            return false;
          }
          value = this._emptyKeyValue;
          return true;
        default:
          int previousIndex = this._previousIndex;
          StringMap<TValue>.Record[] records = this._records;
          if (records.Length <= 4)
          {
            for (int index = 0; index < records.Length; ++index)
            {
              StringMap<TValue>.Record record = records[index];
              if (record.key != null)
              {
                if (string.CompareOrdinal(record.key, key) == 0)
                {
                  value = record.value;
                  return true;
                }
              }
              else
                break;
            }
            value = default (TValue);
            return false;
          }
          if (previousIndex != -1 && string.CompareOrdinal(records[previousIndex].key, key) == 0)
          {
            value = records[previousIndex].value;
            return true;
          }
          int length = records.Length;
          if (length == 0)
          {
            value = default (TValue);
            return false;
          }
          int hash = StringMap<TValue>.computeHash(key);
          int index1 = hash & length - 1;
          while (records[index1].hash != hash || string.CompareOrdinal(records[index1].key, key) != 0)
          {
            index1 = records[index1].next - 1;
            if (index1 < 0)
            {
              value = default (TValue);
              return false;
            }
          }
          value = records[index1].value;
          this._previousIndex = index1;
          return true;
      }
    }

    public bool Remove(string key)
    {
      switch (key)
      {
        case null:
          throw new ArgumentNullException();
        case "":
          if (!this._emptyKeyValueExists)
            return false;
          this._emptyKeyValue = default (TValue);
          this._emptyKeyValueExists = false;
          return true;
        default:
          if (this._records.Length <= 4)
          {
            bool flag = false;
            for (int index = 0; index < this._records.Length; ++index)
            {
              if (flag)
              {
                this._records[index - 1].key = this._records[index].key;
                this._records[index - 1].value = this._records[index].value;
                this._records[index].key = (string) null;
                this._records[index].value = default (TValue);
              }
              else if (string.CompareOrdinal(this._records[index].key, key) == 0)
              {
                --this._count;
                --this._eicount;
                ++this._version;
                flag = true;
                this._records[index].key = (string) null;
                this._records[index].value = default (TValue);
              }
            }
            return flag;
          }
          if (this._records.Length == 0)
            return false;
          int num = this._records.Length - 1;
          int hash = StringMap<TValue>.computeHash(key);
          int index1 = -1;
          for (int index2 = hash & num; index2 >= 0; index2 = this._records[index2].next - 1)
          {
            if (this._records[index2].hash == hash && string.CompareOrdinal(this._records[index2].key, key) == 0)
            {
              if (this._records[index2].next > 0)
              {
                int index3 = index1;
                int index4 = index2;
                int index5 = this._records[index2].next - 1;
                do
                {
                  if ((this._records[index5].hash & num) == index4)
                  {
                    this._records[index4] = this._records[index5];
                    this._records[index4].next = index5 + 1;
                    index3 = index4;
                    index4 = index5;
                  }
                  index5 = this._records[index5].next - 1;
                }
                while (index5 >= 0);
                this._records[index4].key = (string) null;
                this._records[index4].value = default (TValue);
                this._records[index4].hash = 0;
                if (index4 == this._previousIndex)
                  this._previousIndex = -1;
                if (index3 >= 0)
                  this._records[index3].next = 0;
                index2 = index4;
              }
              else
              {
                if (index2 == this._previousIndex)
                  this._previousIndex = -1;
                this._records[index2].key = (string) null;
                this._records[index2].value = default (TValue);
                this._records[index2].hash = 0;
                if (index1 >= 0)
                  this._records[index1].next = 0;
              }
              --this._count;
              --this._eicount;
              ++this._version;
              int destinationIndex = Array.IndexOf<int>(this._existsedIndexes, index2);
              Array.Copy((Array) this._existsedIndexes, destinationIndex + 1, (Array) this._existsedIndexes, destinationIndex, this._existsedIndexes.Length - destinationIndex - 1);
              return true;
            }
            index1 = index2;
          }
          return false;
      }
    }

    private int increaseSize()
    {
      if (this._records.Length == 0)
      {
        this._records = new StringMap<TValue>.Record[2];
        this._existsedIndexes = new int[2];
      }
      else
      {
        StringMap<TValue>.Record[] records = this._records;
        int length = this._records.Length << 1;
        this._records = new StringMap<TValue>.Record[length];
        int eicount = this._eicount;
        this._count = 0;
        this._eicount = 0;
        if (length == 8)
        {
          for (int index = 0; index < eicount; ++index)
          {
            int existsedIndex = this._existsedIndexes[index];
            StringMap<TValue>.Record record = records[existsedIndex];
            if (record.key != null)
            {
              record.hash = StringMap<TValue>.computeHash(record.key);
              records[existsedIndex] = record;
            }
          }
        }
        for (int index = 0; index < eicount; ++index)
        {
          int existsedIndex = this._existsedIndexes[index];
          StringMap<TValue>.Record record = records[existsedIndex];
          if (record.key != null)
            this.insert(record.key, record.value, record.hash, false, false);
        }
      }
      this._previousIndex = -1;
      return this._records.Length;
    }

    public void Add(string key, TValue value)
    {
      lock (this._records)
        this.insert(key, value, StringMap<TValue>.computeHash(key), true, true);
    }

    public bool ContainsKey(string key) => this.TryGetValue(key, out TValue _);

    public ICollection<string> Keys => (ICollection<string>) ((IEnumerable<StringMap<TValue>.Record>) this._records).Where<StringMap<TValue>.Record>((Func<StringMap<TValue>.Record, bool>) (item => item.key != null)).Select<StringMap<TValue>.Record, string>((Func<StringMap<TValue>.Record, string>) (item => item.key)).ToArray<string>();

    public ICollection<TValue> Values => (ICollection<TValue>) ((IEnumerable<StringMap<TValue>.Record>) this._records).Where<StringMap<TValue>.Record>((Func<StringMap<TValue>.Record, bool>) (item => item.key != null)).Select<StringMap<TValue>.Record, TValue>((Func<StringMap<TValue>.Record, TValue>) (item => item.value)).ToArray<TValue>();

    public TValue this[string key]
    {
      get
      {
        TValue obj;
        if (!this.TryGetValue(key, out obj))
          throw new KeyNotFoundException();
        return obj;
      }
      set
      {
        lock (this._records)
          this.insert(key, value, StringMap<TValue>.computeHash(key), false, true);
      }
    }

    public virtual void Add(KeyValuePair<string, TValue> item) => this.Add(item.Key, item.Value);

    public void Clear()
    {
      if (this._existsedIndexes != null)
        Array.Clear((Array) this._existsedIndexes, 0, this._existsedIndexes.Length);
      Array.Clear((Array) this._records, 0, this._records.Length);
      this._count = 0;
      this._eicount = 0;
      ++this._version;
      this._emptyKeyValue = default (TValue);
      this._emptyKeyValueExists = false;
      this._previousIndex = -1;
    }

    public virtual bool Contains(KeyValuePair<string, TValue> item) => throw new NotImplementedException();

    public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
    {
      foreach (KeyValuePair<string, TValue> keyValuePair in this)
      {
        if (arrayIndex >= array.Length)
          break;
        array[arrayIndex++] = keyValuePair;
      }
    }

    public int Count => this._count + (this._emptyKeyValueExists ? 1 : 0);

    public bool IsReadOnly => false;

    public virtual bool Remove(KeyValuePair<string, TValue> item) => throw new NotImplementedException();

    public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
    {
      StringMap<TValue> stringMap1 = this;
      if (stringMap1._emptyKeyValueExists)
        yield return new KeyValuePair<string, TValue>("", stringMap1._emptyKeyValue);
      List<KeyValuePair<uint, string>> numbers = (List<KeyValuePair<uint, string>>) null;
      uint exprected = 0;
      bool forceCheckNum = stringMap1._records.Length <= 4;
      int i = 0;
label_22:
      uint result1;
      while (i < stringMap1._eicount)
      {
        int prevVersion = stringMap1._version;
        for (; i < stringMap1._records.Length; ++i)
        {
          int index = i;
          uint result2;
          if (stringMap1._records[index].key != null && stringMap1._records[index].hash < 0 | forceCheckNum && uint.TryParse(stringMap1._records[index].key, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture, out result2))
          {
            if ((int) exprected == (int) result2)
            {
              yield return new KeyValuePair<string, TValue>(stringMap1._records[index].key, stringMap1._records[index].value);
              result1 = exprected++;
            }
            else
            {
              if (numbers == null)
                numbers = new List<KeyValuePair<uint, string>>();
              numbers.Add(new KeyValuePair<uint, string>(result2, stringMap1._records[i].key));
            }
          }
        }
        if (prevVersion != stringMap1._version)
          i = 0;
        if (numbers != null)
        {
          numbers.Sort((Comparison<KeyValuePair<uint, string>>) ((x, y) => x.Key.CompareTo(y.Key)));
          int ni = 0;
          while (true)
          {
            if (ni < numbers.Count && prevVersion == stringMap1._version)
            {
              KeyValuePair<uint, string> keyValuePair = numbers[ni];
              if (keyValuePair.Key >= exprected)
              {
                StringMap<TValue> stringMap2 = stringMap1;
                keyValuePair = numbers[ni];
                string key = keyValuePair.Value;
                TValue obj;
                ref TValue local = ref obj;
                // ISSUE: explicit non-virtual call
                if (__nonvirtual (stringMap2.TryGetValue(key, out local)))
                {
                  keyValuePair = numbers[ni];
                  yield return new KeyValuePair<string, TValue>(keyValuePair.Value, obj);
                  exprected = numbers[ni].Key + 1U;
                }
              }
              ++ni;
            }
            else
              goto label_22;
          }
        }
      }
      i = 0;
      while (i < stringMap1._eicount)
      {
        for (; i < stringMap1._eicount; ++i)
        {
          int existsedIndex = stringMap1._existsedIndexes[i];
          if (stringMap1._records[existsedIndex].key != null && (stringMap1._records[existsedIndex].hash >= 0 || !uint.TryParse(stringMap1._records[existsedIndex].key, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture, out result1)))
            yield return new KeyValuePair<string, TValue>(stringMap1._records[existsedIndex].key, stringMap1._records[existsedIndex].value);
        }
      }
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    private struct Record
    {
      public int hash;
      public string key;
      public int next;
      public TValue value;
    }
  }
}
