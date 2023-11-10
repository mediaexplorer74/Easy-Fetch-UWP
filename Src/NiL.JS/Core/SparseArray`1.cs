// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.SparseArray`1
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace NiL.JS.Core
{
  public sealed class SparseArray<TValue> : 
    IList<TValue>,
    ICollection<TValue>,
    IEnumerable<TValue>,
    IEnumerable
  {
    private static readonly SparseArray<TValue>._NavyItem[] emptyNavyData = new SparseArray<TValue>._NavyItem[0];
    private static readonly TValue[] emptyData = new TValue[0];
    private ArrayMode _mode;
    private uint _pseudoLength;
    private uint _allocatedCount;
    private SparseArray<TValue>._NavyItem[] _navyData;
    private TValue[] _values;
    private bool _zeroExists;

    public ArrayMode Mode => this._mode;

    [CLSCompliant(false)]
    public uint Length => this._pseudoLength;

    public SparseArray(ArrayMode arrayMode = ArrayMode.Flat)
    {
      this._mode = arrayMode;
      this._values = SparseArray<TValue>.emptyData;
      this._navyData = SparseArray<TValue>.emptyNavyData;
    }

    public SparseArray(int capacity)
    {
      this._mode = ArrayMode.Flat;
      this._values = SparseArray<TValue>.emptyData;
      this._navyData = SparseArray<TValue>.emptyNavyData;
      if (capacity <= 0)
        return;
      this.ensureCapacity(capacity);
    }

    public SparseArray(TValue[] values)
    {
      this._mode = ArrayMode.Flat;
      this._values = values;
      this._navyData = SparseArray<TValue>.emptyNavyData;
      this._allocatedCount = this._pseudoLength = (uint) values.Length;
    }

    public int IndexOf(TValue item)
    {
      for (int index = 0; (long) index < (long) this._allocatedCount; ++index)
      {
        if (object.Equals((object) this._values[index], (object) item))
          return this._mode == ArrayMode.Flat ? index : (int) this._navyData[index].index;
      }
      return -1;
    }

    void IList<TValue>.Insert(int index, TValue item) => throw new NotImplementedException();

    public void RemoveAt(int index)
    {
      if (this._pseudoLength == 0U || index != (int) this._pseudoLength - 1)
        throw new InvalidOperationException();
      this[(int) this._pseudoLength - 1] = default (TValue);
      --this._pseudoLength;
    }

    public TValue this[int index]
    {
      get
      {
        if (this._mode == ArrayMode.Flat)
          return index < 0 || (long) this._pseudoLength <= (long) index || this._values.Length <= index ? default (TValue) : this._values[index];
        if (this._navyData.Length == 0)
          return default (TValue);
        uint num1 = (uint) index;
        int num2 = 31;
        uint index1 = 0;
        if (num1 < this._allocatedCount && (int) this._navyData[index].index == (int) num1)
          return this._values[index];
        while (true)
        {
          index1 = ((long) num1 & (long) (1 << num2)) == 0L ? this._navyData[(int) index1].zeroContinue : this._navyData[(int) index1].oneContinue;
          if (index1 != 0U)
          {
            if ((int) this._navyData[(int) index1].index != (int) num1)
              --num2;
            else
              goto label_11;
          }
          else
            break;
        }
        return default (TValue);
label_11:
        return this._values[(int) index1];
      }
      set
      {
        bool flag1 = (object) value == null;
        uint num1 = (uint) index;
        if (this._mode == ArrayMode.Flat)
        {
          if (index < 0 || (long) index > (long) this._pseudoLength)
          {
            if (flag1)
            {
              if (num1 < this._pseudoLength)
                return;
              this._pseudoLength = num1 + 1U;
              return;
            }
            if (num1 < 32U)
            {
              this._pseudoLength = num1 + 1U;
              this.ensureCapacity((int) this._pseudoLength);
              this[index] = value;
              return;
            }
            this.RebuildToSparse();
          }
          else
          {
            if (this._values.Length <= index)
              this.ensureCapacity(Math.Max(index + 1, this._values.Length * 2));
            if ((long) this._pseudoLength == (long) index)
              this._pseudoLength = num1 + 1U;
            this._values[index] = value;
            return;
          }
        }
        if (this._allocatedCount == 0U)
        {
          this.ensureCapacity(1);
          this._allocatedCount = 1U;
        }
        if (num1 < this._allocatedCount && (int) this._navyData[index].index == (int) num1)
        {
          if (index == 0)
            this._zeroExists = true;
          if ((long) this._pseudoLength <= (long) index)
            this._pseudoLength = num1 + 1U;
          this._values[index] = value;
        }
        else
        {
          int num2 = 31;
          uint index1 = 0;
          while (this._navyData[(int) index1].index <= num1)
          {
            if (this._navyData[(int) index1].index < num1)
            {
              bool flag2 = ((long) num1 & (long) (1 << num2)) == 0L;
              uint num3 = flag2 ? this._navyData[(int) index1].zeroContinue : this._navyData[(int) index1].oneContinue;
              if (num3 == 0U)
              {
                if (this._pseudoLength <= num1)
                  this._pseudoLength = num1 + 1U;
                if (flag1)
                  return;
                uint index2;
                if (flag2)
                {
                  ref SparseArray<TValue>._NavyItem local = ref this._navyData[(int) index1];
                  uint num4 = this._allocatedCount++;
                  int num5;
                  index2 = (uint) (num5 = (int) num4);
                  local.zeroContinue = (uint) num5;
                }
                else
                {
                  ref SparseArray<TValue>._NavyItem local = ref this._navyData[(int) index1];
                  uint num6 = this._allocatedCount++;
                  int num7;
                  index2 = (uint) (num7 = (int) num6);
                  local.oneContinue = (uint) num7;
                }
                if ((long) this._navyData.Length <= (long) this._allocatedCount)
                  this.ensureCapacity(this._navyData.Length * 2);
                this._navyData[(int) index2].index = num1;
                this._values[(int) index2] = value;
                return;
              }
              index1 = num3;
              --num2;
            }
            else
            {
              this._values[(int) index1] = value;
              if ((long) this._pseudoLength <= (long) index)
                this._pseudoLength = num1 + 1U;
              if (this._allocatedCount > index1)
                return;
              this._allocatedCount = index1 + 1U;
              return;
            }
          }
          if (flag1)
          {
            if (this._pseudoLength > num1)
              return;
            this._pseudoLength = num1 + 1U;
          }
          else
          {
            uint index3 = this._navyData[(int) index1].index;
            TValue obj = this._values[(int) index1];
            this._navyData[(int) index1].index = num1;
            this._values[(int) index1] = value;
            if (index3 >= this._pseudoLength)
              return;
            this[(int) index3] = obj;
          }
        }
      }
    }

    public void Add(TValue item)
    {
      if (this._pseudoLength == uint.MaxValue)
        throw new InvalidOperationException();
      this[(int) this._pseudoLength] = item;
    }

    public void Clear()
    {
      while (this._allocatedCount > 0U)
      {
        this._navyData[(int) --this._allocatedCount] = new SparseArray<TValue>._NavyItem();
        this._values[(int) this._allocatedCount] = default (TValue);
      }
      this._pseudoLength = 0U;
    }

    public bool Contains(TValue item) => this.IndexOf(item) != -1;

    public void CopyTo(TValue[] array, int arrayIndex)
    {
      if (array == null)
        throw new NullReferenceException();
      if (arrayIndex < 0)
        throw new ArgumentOutOfRangeException();
      if ((long) Math.Min(this._pseudoLength, (uint) int.MaxValue) - (long) arrayIndex > (long) array.Length)
        throw new ArgumentOutOfRangeException();
      long index = (long) Math.Min(this._pseudoLength, (uint) int.MaxValue) + (long) arrayIndex;
      while (index-- > (long) arrayIndex)
        array[(IntPtr) index] = default (TValue);
      foreach (KeyValuePair<int, TValue> keyValuePair in this.DirectOrder)
      {
        if (keyValuePair.Key >= 0)
          array[keyValuePair.Key + arrayIndex] = keyValuePair.Value;
      }
    }

    int ICollection<TValue>.Count => (int) this._pseudoLength;

    public bool IsReadOnly => false;

    public bool Remove(TValue item) => throw new NotImplementedException();

    public IEnumerator<TValue> GetEnumerator()
    {
      for (uint i = 0; i < this._pseudoLength; ++i)
        yield return this[(int) i];
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public long NearestIndexNotLess(long index)
    {
      if (this._mode == ArrayMode.Sparse)
      {
        if (this._navyData.Length == 0)
          return index;
        int num = 31;
        long index1 = 0;
        long index2 = -1;
        while (true)
        {
          if (this._navyData[index1].oneContinue != 0U)
            index2 = index1;
          index1 = (index & (long) (1 << num)) == 0L ? (long) this._navyData[index1].zeroContinue : (long) this._navyData[index1].oneContinue;
          if (index1 == 0L)
          {
            if (index2 != -1L)
            {
              index1 = (long) this._navyData[index2].oneContinue;
              while (true)
              {
                for (; this._navyData[index1].zeroContinue == 0U; index1 = (long) this._navyData[index1].oneContinue)
                {
                  if (this._navyData[index1].oneContinue == 0U)
                    goto label_17;
                }
                index1 = (long) this._navyData[index1].zeroContinue;
              }
            }
            else
              break;
          }
label_17:
          if (this._navyData[index1].index < (uint) index)
            --num;
          else
            goto label_18;
        }
        return 0;
label_18:
        return (long) this._navyData[index1].index;
      }
      return (long) this._values.Length < index ? 0L : index;
    }

    public long NearestIndexNotMore(long index)
    {
      if (this._mode != ArrayMode.Sparse)
        return Math.Min((long) this._values.Length, index);
      if (this._navyData.Length == 0)
        return 0;
      int num = 31;
      long index1 = 0;
      while (true)
      {
        uint index2 = (index & (long) (1 << num)) == 0L ? this._navyData[index1].zeroContinue : this._navyData[index1].oneContinue;
        if (index2 == 0U || (long) this._navyData[(int) index2].index > index)
          index = (long) this._navyData[index1].index;
        else
          index1 = (long) index2;
        if ((long) this._navyData[index1].index != index)
          --num;
        else
          break;
      }
      return (long) this._navyData[index1].index;
    }

    public IEnumerable<KeyValuePair<int, TValue>> DirectOrder
    {
      get
      {
        uint index = 1;
        bool skipFirst = !this._zeroExists;
        if (this._mode == ArrayMode.Flat)
        {
          for (int i = 0; (long) i < (long) this._pseudoLength; ++i)
          {
            skipFirst = true;
            if (i >= this._values.Length)
            {
              yield return new KeyValuePair<int, TValue>((int) this._pseudoLength - 1, default (TValue));
              yield break;
            }
            else
            {
              yield return new KeyValuePair<int, TValue>(i, this._values[i]);
              if (this._mode != ArrayMode.Flat)
              {
                index = (uint) (i + 1);
                break;
              }
            }
          }
        }
        if (this._mode == ArrayMode.Sparse)
        {
          if (this._allocatedCount > 0U)
          {
            if (!skipFirst)
              yield return new KeyValuePair<int, TValue>(0, this._values[0]);
            while (index < this._pseudoLength)
            {
              int num1 = 31;
              long index1 = 0;
              long num2 = -1;
              for (; num1 >= 0; --num1)
              {
                int num3 = ((int) index & 1 << num1) == 0 ? 1 : 0;
                if (num3 != 0 && this._navyData[index1].oneContinue != 0U)
                  num2 = (long) this._navyData[index1].oneContinue;
                index1 = num3 != 0 ? (long) this._navyData[index1].zeroContinue : (long) this._navyData[index1].oneContinue;
                if (index1 == 0L)
                {
                  if (num2 == -1L)
                  {
                    yield return new KeyValuePair<int, TValue>((int) this._pseudoLength - 1, default (TValue));
                    yield break;
                  }
                  else
                  {
                    index1 = num2;
                    while (true)
                    {
                      for (; this._navyData[index1].zeroContinue == 0U; index1 = (long) this._navyData[index1].oneContinue)
                      {
                        if (this._navyData[index1].oneContinue == 0U)
                          goto label_26;
                      }
                      index1 = (long) this._navyData[index1].zeroContinue;
                    }
                  }
                }
label_26:
                if (this._navyData[index1].index >= index)
                {
                  index = this._navyData[index1].index;
                  yield return new KeyValuePair<int, TValue>((int) index, this._values[index1]);
                  ++index;
                  break;
                }
              }
            }
          }
          else
            yield return new KeyValuePair<int, TValue>((int) this._pseudoLength - 1, default (TValue));
        }
      }
    }

    public IEnumerable<KeyValuePair<int, TValue>> ReversOrder
    {
      get
      {
        uint index = this._pseudoLength - 1U;
        if (this._mode == ArrayMode.Flat)
        {
          if ((long) this._pseudoLength > (long) this._values.Length)
            yield return new KeyValuePair<int, TValue>((int) this._pseudoLength - 1, default (TValue));
          long i = Math.Min((long) this._values.Length, (long) this._pseudoLength);
          while (i-- > 0L)
          {
            if (this._mode != ArrayMode.Flat)
            {
              index = (uint) i;
              break;
            }
            yield return new KeyValuePair<int, TValue>((int) i, this._values[i]);
          }
        }
        if (this._mode == ArrayMode.Sparse && this._allocatedCount != 0U)
        {
          for (; index > 0U; --index)
          {
            int num = 31;
            long index1 = 0;
            while (true)
            {
              uint index2 = ((long) index & (long) (1 << num)) == 0L ? this._navyData[index1].zeroContinue : this._navyData[index1].oneContinue;
              if (index2 == 0U || this._navyData[(int) index2].index > index)
                index = this._navyData[index1].index;
              else
                index1 = (long) index2;
              if ((int) this._navyData[index1].index != (int) index)
                --num;
              else
                break;
            }
            yield return new KeyValuePair<int, TValue>((int) index, this._values[index1]);
            if (index == 0U)
              yield break;
          }
          yield return new KeyValuePair<int, TValue>(0, this._values[0]);
        }
      }
    }

    public void Trim()
    {
      long num = -1;
      if (this._mode == ArrayMode.Flat)
      {
        int length = this._values.Length;
        while (length-- > 0)
        {
          if (!object.Equals((object) this._values[length], (object) default (TValue)))
          {
            num = (long) length;
            break;
          }
        }
      }
      else
      {
        uint allocatedCount = this._allocatedCount;
        while (allocatedCount-- > 0U)
        {
          if ((long) this._navyData[(int) allocatedCount].index > num && !object.Equals((object) this._values[(int) allocatedCount], (object) default (TValue)))
            num = (long) this._navyData[(int) allocatedCount].index;
        }
      }
      this._pseudoLength = (uint) ((ulong) num + 1UL);
    }

    private void ensureCapacity(int p)
    {
      p = Math.Max(4, p);
      if (this._values.Length >= p)
        return;
      TValue[] objArray = new TValue[p];
      if (this._values != null)
      {
        for (int index = 0; index < this._values.Length; ++index)
          objArray[index] = this._values[index];
      }
      this._values = objArray;
      if (this._mode != ArrayMode.Sparse)
        return;
      SparseArray<TValue>._NavyItem[] navyItemArray = new SparseArray<TValue>._NavyItem[p];
      for (int index = 0; index < this._navyData.Length; ++index)
        navyItemArray[index] = this._navyData[index];
      this._navyData = navyItemArray;
    }

    public void RebuildToSparse()
    {
      this._allocatedCount = 0U;
      this._mode = ArrayMode.Sparse;
      uint pseudoLength = this._pseudoLength;
      if (pseudoLength == 0U)
      {
        this.ensureCapacity(0);
      }
      else
      {
        this._navyData = new SparseArray<TValue>._NavyItem[this._values.Length];
        TValue[] values = this._values;
        this._values = new TValue[this._values.Length];
        uint num = (uint) Math.Min((long) values.Length, (long) pseudoLength);
        for (int index = 0; (long) index < (long) num; ++index)
          this[index] = values[index];
        if ((long) this._values.Length >= (long) num)
          return;
        this[(int) num - 1] = default (TValue);
      }
    }

    private struct _NavyItem
    {
      public uint index;
      public uint zeroContinue;
      public uint oneContinue;

      public override string ToString() => this.index.ToString() + "[" + this.zeroContinue.ToString() + ";" + this.oneContinue.ToString() + "]";
    }
  }
}
