// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.BinaryTree`2
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace NiL.JS.Core
{
  public class BinaryTree<TKey, TValue> : 
    IDictionary<TKey, TValue>,
    ICollection<KeyValuePair<TKey, TValue>>,
    IEnumerable<KeyValuePair<TKey, TValue>>,
    IEnumerable
    where TKey : IComparable<TKey>
  {
    private IComparer<TKey> comparer;
    private long state;
    private Stack<BinaryTree<TKey, TValue>.Node> stack = new Stack<BinaryTree<TKey, TValue>.Node>();
    private ICollection<TKey> keys;
    private ICollection<TValue> values;
    private BinaryTree<TKey, TValue>.Node root;

    public int Height => this.root != null ? this.root.height : 0;

    public int Count { get; private set; }

    public bool IsReadOnly => false;

    public ICollection<TKey> Keys => this.keys ?? (this.keys = (ICollection<TKey>) new BinaryTree<TKey, TValue>._Keys(this));

    public ICollection<TValue> Values => this.values ?? (this.values = (ICollection<TValue>) new BinaryTree<TKey, TValue>._Values(this));

    internal BinaryTree<TKey, TValue>.Node Root => this.root;

    public BinaryTree()
    {
      if (!TypeExtensions.IsAssignableFrom(typeof (IComparable), typeof (TKey)) && !TypeExtensions.IsAssignableFrom(typeof (IComparable<TKey>), typeof (TKey)))
        throw new ArgumentException("Compaper is not defined.");
      this.root = (BinaryTree<TKey, TValue>.Node) null;
      this.Count = 0;
      this.state = DateTime.UtcNow.Ticks;
    }

    public BinaryTree(IComparer<TKey> comparer)
    {
      this.root = (BinaryTree<TKey, TValue>.Node) null;
      this.Count = 0;
      this.state = DateTime.UtcNow.Ticks;
      this.comparer = comparer;
    }

    public TValue this[TKey key]
    {
      get
      {
        if ((object) key == null)
          throw new ArgumentNullException(nameof (key));
        TValue obj;
        if (!this.TryGetValue(key, out obj))
          throw new ArgumentException("Key not found.");
        return obj;
      }
      set
      {
        lock (this)
        {
          if ((object) key == null)
            throw new ArgumentNullException(nameof (key));
          if (this.root == null)
          {
            this.root = new BinaryTree<TKey, TValue>.Node()
            {
              value = value,
              key = key
            };
            ++this.Count;
            this.state ^= this.state << 1;
          }
          else
          {
            BinaryTree<TKey, TValue>.Node node = this.root;
            this.stack.Clear();
            while (true)
            {
              int num;
              do
              {
                num = this.comparer != null ? this.comparer.Compare(key, node.key) : key.CompareTo(node.key);
                if (num == 0)
                {
                  node.value = value;
                  return;
                }
                if (num > 0)
                {
                  if (node.greater == null)
                  {
                    node.greater = new BinaryTree<TKey, TValue>.Node()
                    {
                      key = key,
                      value = value
                    };
                    node.height = 0;
                    while (this.stack.Count != 0)
                      this.stack.Pop().height = 0;
                    this.root.Balance(ref this.root);
                    ++this.Count;
                    this.state ^= this.state << 1;
                    return;
                  }
                  this.stack.Push(node);
                  node = node.greater;
                }
              }
              while (num >= 0);
              if (node.less != null)
              {
                this.stack.Push(node);
                node = node.less;
              }
              else
                break;
            }
            node.less = new BinaryTree<TKey, TValue>.Node()
            {
              key = key,
              value = value
            };
            node.height = 0;
            while (this.stack.Count != 0)
              this.stack.Pop().height = 0;
            this.root.Balance(ref this.root);
            ++this.Count;
            this.state ^= this.state << 1;
          }
        }
      }
    }

    public void Clear()
    {
      this.Count = 0;
      this.root = (BinaryTree<TKey, TValue>.Node) null;
      this.state ^= this.state << 1;
    }

    public void Add(KeyValuePair<TKey, TValue> item) => this.Add(item.Key, item.Value);

    public void Add(TKey key, TValue value) => this.Insert(key, value, true);

    public bool Insert(TKey key, TValue value, bool throwIfExists)
    {
      lock (this)
      {
        if ((object) key == null)
          throw new ArgumentNullException(nameof (key));
        if (this.root == null)
        {
          this.root = new BinaryTree<TKey, TValue>.Node()
          {
            value = value,
            key = key
          };
          ++this.Count;
          this.state ^= this.state << 1;
          return true;
        }
        BinaryTree<TKey, TValue>.Node node = this.root;
        this.stack.Clear();
        while (true)
        {
          int num;
          do
          {
            num = this.comparer != null ? this.comparer.Compare(key, node.key) : key.CompareTo(node.key);
            if (num == 0)
            {
              if (throwIfExists)
                throw new ArgumentException("Element Exists");
              return false;
            }
            if (num > 0)
            {
              if (node.greater == null)
              {
                node.greater = new BinaryTree<TKey, TValue>.Node()
                {
                  key = key,
                  value = value
                };
                node.height = 0;
                while (this.stack.Count != 0)
                  this.stack.Pop().height = 0;
                this.root.Balance(ref this.root);
                ++this.Count;
                this.state ^= this.state << 1;
                return true;
              }
              this.stack.Push(node);
              node = node.greater;
            }
          }
          while (num >= 0);
          if (node.less != null)
          {
            this.stack.Push(node);
            node = node.less;
          }
          else
            break;
        }
        node.less = new BinaryTree<TKey, TValue>.Node()
        {
          key = key,
          value = value
        };
        node.height = 0;
        while (this.stack.Count != 0)
          this.stack.Pop().height = 0;
        this.root.Balance(ref this.root);
        ++this.Count;
        this.state ^= this.state << 1;
        return true;
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetValue(TKey key, out TValue value)
    {
      lock (this)
      {
        if (this.root == null)
        {
          value = default (TValue);
          return false;
        }
        BinaryTree<TKey, TValue>.Node node = this.root;
        while (true)
        {
          int num = this.comparer != null ? this.comparer.Compare(key, node.key) : key.CompareTo(node.key);
          if (num != 0)
          {
            if (num > 0)
            {
              if (node.greater != null)
                node = node.greater;
              else
                goto label_8;
            }
            else if (node.less != null)
              node = node.less;
            else
              goto label_11;
          }
          else
            break;
        }
        value = node.value;
        return true;
label_8:
        value = default (TValue);
        return false;
label_11:
        value = default (TValue);
        return false;
      }
    }

    public bool ContainsKey(TKey key) => this.TryGetValue(key, out TValue _);

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
      TValue obj;
      return this.TryGetValue(item.Key, out obj) && item.Value.Equals((object) obj);
    }

    public bool Remove(TKey key)
    {
      if (this.root == null)
        return false;
      lock (this)
      {
        BinaryTree<TKey, TValue>.Node node1 = (BinaryTree<TKey, TValue>.Node) null;
        BinaryTree<TKey, TValue>.Node node2 = this.root;
        this.stack.Clear();
        while (true)
        {
          int num = this.comparer != null ? this.comparer.Compare(key, node2.key) : key.CompareTo(node2.key);
          if (num != 0)
          {
            if (num > 0)
            {
              if (node2.greater != null)
              {
                node1 = node2;
                this.stack.Push(node2);
                node2 = node2.greater;
              }
              else
                goto label_38;
            }
            else if (node2.less != null)
            {
              node1 = node2;
              this.stack.Push(node2);
              node2 = node2.less;
            }
            else
              goto label_41;
          }
          else
            break;
        }
        if (node2.greater == null)
        {
          if (node1 == null)
            this.root = node2.less;
          else if (node1.greater == node2)
            node1.greater = node2.less;
          else
            node1.less = node2.less;
        }
        else if (node2.less == null)
        {
          if (node1 == null)
            this.root = node2.greater;
          else if (node1.greater == node2)
            node1.greater = node2.greater;
          else
            node1.less = node2.greater;
        }
        else
        {
          BinaryTree<TKey, TValue>.Node node3 = node2.less;
          if (node3.greater != null)
          {
            node3.height = 0;
            BinaryTree<TKey, TValue>.Node node4 = node2;
            while (node3.greater != null)
            {
              node4 = node3;
              node3 = node3.greater;
              node3.height = 0;
            }
            node4.greater = node3.less;
            node3.greater = node2.greater;
            node3.less = node2.less;
            if (node1 == null)
              this.root = node3;
            else if (node1.greater == node2)
              node1.greater = node3;
            else
              node1.less = node3;
          }
          else
          {
            node3.height = 0;
            node3.greater = node2.greater;
            if (node1 == null)
              this.root = node3;
            else if (node1.greater == node2)
              node1.greater = node3;
            else
              node1.less = node3;
          }
        }
        while (this.stack.Count > 0)
          this.stack.Pop().height = 0;
        if (this.root != null)
          this.root.Balance(ref this.root);
        --this.Count;
        return true;
label_38:
        return false;
label_41:
        return false;
      }
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
      if (this.root == null)
        return false;
      lock (this)
      {
        TKey key = item.Key;
        BinaryTree<TKey, TValue>.Node node1 = (BinaryTree<TKey, TValue>.Node) null;
        BinaryTree<TKey, TValue>.Node node2 = this.root;
        this.stack.Clear();
        while (true)
        {
          int num = this.comparer != null ? this.comparer.Compare(key, node2.key) : key.CompareTo(node2.key);
          if (num != 0)
          {
            if (num > 0)
            {
              if (node2.greater != null)
              {
                node1 = node2;
                this.stack.Push(node2);
                node2 = node2.greater;
              }
              else
                goto label_38;
            }
            else if (node2.less != null)
            {
              node1 = node2;
              this.stack.Push(node2);
              node2 = node2.less;
            }
            else
              goto label_41;
          }
          else
            break;
        }
        if (!item.Value.Equals((object) node2.value))
          return false;
        if (node2.greater == null)
        {
          if (node1 == null)
            this.root = node2.less;
          else if (node1.greater == node2)
            node1.greater = node2.less;
          else
            node1.less = node2.less;
        }
        else if (node2.less == null)
        {
          if (node1 == null)
            this.root = node2.greater;
          else if (node1.greater == node2)
            node1.greater = node2.greater;
          else
            node1.less = node2.greater;
        }
        else
        {
          BinaryTree<TKey, TValue>.Node node3 = node2.less;
          if (node3.greater != null)
          {
            node3.height = 0;
            BinaryTree<TKey, TValue>.Node node4 = node2;
            while (node3.greater != null)
            {
              node4 = node3;
              node3 = node3.greater;
              node3.height = 0;
            }
            node4.greater = node3.less;
            node3.greater = node2.greater;
            node3.less = node2.less;
            if (node1 == null)
              this.root = node3;
            else if (node1.greater == node2)
              node1.greater = node3;
            else
              node1.less = node3;
          }
          else
          {
            node3.height = 0;
            node3.greater = node2.greater;
            if (node1 == null)
              this.root = node3;
            else if (node1.greater == node2)
              node1.greater = node3;
            else
              node1.less = node3;
          }
        }
        while (this.stack.Count > 0)
          this.stack.Pop().height = 0;
        this.root.Balance(ref this.root);
        return true;
label_38:
        return false;
label_41:
        return false;
      }
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
      if (array == null)
        throw new ArgumentNullException(nameof (array));
      if (arrayIndex < 0)
        throw new ArgumentOutOfRangeException("index");
      if (array.Length - arrayIndex < this.Count)
        throw new ArgumentException("index and array incompatible with count of elements");
      foreach (KeyValuePair<TKey, TValue> keyValuePair in this)
        array[arrayIndex++] = keyValuePair;
    }

    internal IEnumerator<BinaryTree<TKey, TValue>.Node> enumerateReversed(
      BinaryTree<TKey, TValue>.Node node)
    {
      if (node != null)
      {
        long sstate = this.state;
        BinaryTree<TKey, TValue>.Node[] stack = new BinaryTree<TKey, TValue>.Node[node.height];
        int[] step = new int[node.height];
        int sindex = -1;
        stack[++sindex] = node;
        while (sindex >= 0)
        {
          if (step[sindex] == 0 && stack[sindex].greater != null)
          {
            stack[sindex + 1] = stack[sindex].greater;
            step[sindex] = 1;
            ++sindex;
            step[sindex] = 0;
          }
          else
          {
            if (step[sindex] < 2)
            {
              step[sindex] = 2;
              yield return stack[sindex];
              if (sstate != this.state && node.height > stack.Length)
              {
                BinaryTree<TKey, TValue>.Node[] nodeArray = new BinaryTree<TKey, TValue>.Node[node.height];
                for (int index = 0; index < stack.Length; ++index)
                  nodeArray[index] = stack[index];
                stack = nodeArray;
                int[] numArray = new int[node.height];
                for (int index = 0; index < step.Length; ++index)
                  numArray[index] = step[index];
                step = numArray;
              }
            }
            if (step[sindex] < 3 && stack[sindex].less != null)
            {
              stack[sindex + 1] = stack[sindex].less;
              step[sindex] = 3;
              ++sindex;
              step[sindex] = 0;
            }
            else
              --sindex;
          }
        }
        stack = (BinaryTree<TKey, TValue>.Node[]) null;
        step = (int[]) null;
      }
    }

    internal IEnumerator<BinaryTree<TKey, TValue>.Node> enumerate(BinaryTree<TKey, TValue>.Node node)
    {
      if (node != null)
      {
        long sstate = this.state;
        BinaryTree<TKey, TValue>.Node[] stack = new BinaryTree<TKey, TValue>.Node[node.height];
        int[] step = new int[node.height];
        int sindex = -1;
        stack[++sindex] = node;
        while (sindex >= 0)
        {
          if (step[sindex] == 0 && stack[sindex].less != null)
          {
            stack[sindex + 1] = stack[sindex].less;
            step[sindex] = 1;
            ++sindex;
            step[sindex] = 0;
          }
          else
          {
            if (step[sindex] < 2)
            {
              step[sindex] = 2;
              yield return stack[sindex];
              if (sstate != this.state && node.height > stack.Length)
              {
                BinaryTree<TKey, TValue>.Node[] nodeArray = new BinaryTree<TKey, TValue>.Node[node.height];
                for (int index = 0; index < stack.Length; ++index)
                  nodeArray[index] = stack[index];
                stack = nodeArray;
                int[] numArray = new int[node.height];
                for (int index = 0; index < step.Length; ++index)
                  numArray[index] = step[index];
                step = numArray;
              }
            }
            if (step[sindex] < 3 && stack[sindex].greater != null)
            {
              stack[sindex + 1] = stack[sindex].greater;
              step[sindex] = 3;
              ++sindex;
              step[sindex] = 0;
            }
            else
            {
              step[sindex] = 0;
              stack[sindex] = (BinaryTree<TKey, TValue>.Node) null;
              --sindex;
            }
          }
        }
        stack = (BinaryTree<TKey, TValue>.Node[]) null;
        step = (int[]) null;
      }
    }

    internal IEnumerable<BinaryTree<TKey, TValue>.Node> Nodes
    {
      get
      {
        IEnumerator<BinaryTree<TKey, TValue>.Node> e = this.enumerate(this.root);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator<BinaryTree<TKey, TValue>.Node>) null;
      }
    }

    public IEnumerable<KeyValuePair<TKey, TValue>> Reversed
    {
      get
      {
        IEnumerator<BinaryTree<TKey, TValue>.Node> e = this.enumerateReversed(this.root);
        while (e.MoveNext())
          yield return new KeyValuePair<TKey, TValue>(e.Current.key, e.Current.value);
        e = (IEnumerator<BinaryTree<TKey, TValue>.Node>) null;
      }
    }

    public IEnumerable<KeyValuePair<TKey, TValue>> NotLess(TKey keyValue) => this.NotLess(keyValue, false, 0L, (long) int.MaxValue);

    public IEnumerable<KeyValuePair<TKey, TValue>> NotLess(TKey keyValue, bool reversed) => this.NotLess(keyValue, reversed, 0L, (long) int.MaxValue);

    public IEnumerable<KeyValuePair<TKey, TValue>> NotLess(
      TKey keyValue,
      bool reversed,
      long offset)
    {
      return this.NotLess(keyValue, reversed, offset, (long) int.MaxValue);
    }

    public IEnumerable<KeyValuePair<TKey, TValue>> NotLess(
      TKey keyValue,
      bool reversed,
      long offset,
      long count)
    {
      BinaryTree<TKey, TValue>.Node node = this.Root;
      if (node != null)
      {
        do
        {
          int num = this.comparer != null ? this.comparer.Compare(keyValue, node.key) : ((object) keyValue as IComparable).CompareTo((object) node.key);
          if (num <= 0)
          {
            IEnumerator<BinaryTree<TKey, TValue>.Node> enmrtr = reversed ? this.enumerateReversed(node) : this.enumerate(node);
            while (count-- > 0L && enmrtr.MoveNext())
            {
              if (offset-- > 0L)
              {
                ++count;
              }
              else
              {
                BinaryTree<TKey, TValue>.Node current = enmrtr.Current;
                if (((object) keyValue as IComparable).CompareTo((object) current.key) <= 0)
                  yield return new KeyValuePair<TKey, TValue>(current.key, current.value);
              }
            }
            goto label_4;
          }
          else if (num > 0)
            node = node.greater;
        }
        while (node != null);
        goto label_2;
label_4:
        yield break;
label_2:;
      }
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
      IEnumerator<BinaryTree<TKey, TValue>.Node> e = this.enumerate(this.root);
      while (e.MoveNext())
        yield return new KeyValuePair<TKey, TValue>(e.Current.key, e.Current.value);
      e = (IEnumerator<BinaryTree<TKey, TValue>.Node>) null;
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    private sealed class _Values : ICollection<TValue>, IEnumerable<TValue>, IEnumerable
    {
      private BinaryTree<TKey, TValue> owner;

      public _Values(BinaryTree<TKey, TValue> owner) => this.owner = owner;

      public int Count => this.owner.Count;

      public bool IsReadOnly => true;

      public void Add(TValue item) => throw new NotSupportedException();

      public void Clear() => throw new NotSupportedException();

      public bool Contains(TValue item)
      {
        foreach (KeyValuePair<TKey, TValue> keyValuePair in this.owner)
        {
          if (keyValuePair.Value.Equals((object) item))
            return true;
        }
        return false;
      }

      public void CopyTo(TValue[] array, int arrayIndex)
      {
        if (array == null)
          throw new ArgumentNullException(nameof (array));
        if (arrayIndex < 0)
          throw new ArgumentOutOfRangeException(nameof (arrayIndex));
        if (array.Length - arrayIndex < this.owner.Count)
          throw new ArgumentOutOfRangeException(nameof (arrayIndex));
        foreach (KeyValuePair<TKey, TValue> keyValuePair in this.owner)
          array[arrayIndex++] = keyValuePair.Value;
      }

      public bool Remove(TValue item) => throw new NotSupportedException();

      public IEnumerator<TValue> GetEnumerator()
      {
        foreach (KeyValuePair<TKey, TValue> keyValuePair in this.owner)
          yield return keyValuePair.Value;
      }

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
    }

    private sealed class _Keys : ICollection<TKey>, IEnumerable<TKey>, IEnumerable
    {
      private BinaryTree<TKey, TValue> owner;

      public _Keys(BinaryTree<TKey, TValue> owner) => this.owner = owner;

      public int Count => this.owner.Count;

      public bool IsReadOnly => true;

      public void Add(TKey item) => throw new NotSupportedException();

      public void Clear() => throw new NotSupportedException();

      public bool Contains(TKey item) => this.owner.ContainsKey(item);

      public void CopyTo(TKey[] array, int arrayIndex)
      {
        if (array == null)
          throw new ArgumentNullException(nameof (array));
        if (arrayIndex < 0)
          throw new ArgumentOutOfRangeException(nameof (arrayIndex));
        if (array.Length - arrayIndex < this.owner.Count)
          throw new ArgumentOutOfRangeException(nameof (arrayIndex));
        foreach (KeyValuePair<TKey, TValue> keyValuePair in this.owner)
          array[arrayIndex++] = keyValuePair.Key;
      }

      public bool Remove(TKey item) => throw new NotSupportedException();

      public IEnumerator<TKey> GetEnumerator()
      {
        foreach (KeyValuePair<TKey, TValue> keyValuePair in this.owner)
          yield return keyValuePair.Key;
      }

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
    }

    internal sealed class Node
    {
      public TKey key;
      public TValue value;
      public BinaryTree<TKey, TValue>.Node less;
      public BinaryTree<TKey, TValue>.Node greater;
      public int height;

      private void rotateLtM(ref BinaryTree<TKey, TValue>.Node _this)
      {
        BinaryTree<TKey, TValue>.Node greater = this.less.greater;
        this.less.greater = _this;
        _this = this.less;
        this.less = greater;
      }

      private void rotateMtL(ref BinaryTree<TKey, TValue>.Node _this)
      {
        BinaryTree<TKey, TValue>.Node less = this.greater.less;
        this.greater.less = _this;
        _this = this.greater;
        this.greater = less;
      }

      private void validateHeight() => this.height = Math.Max(this.less != null ? this.less.height : 0, this.greater != null ? this.greater.height : 0) + 1;

      public void Balance(ref BinaryTree<TKey, TValue>.Node _this)
      {
        int num1 = 0;
        int num2 = 0;
        if (this.less != null)
        {
          num1 = this.less.height;
          if (num1 == 0)
          {
            this.less.Balance(ref this.less);
            num1 = this.less.height;
          }
        }
        if (this.greater != null)
        {
          num2 = this.greater.height;
          if (num2 == 0)
          {
            this.greater.Balance(ref this.greater);
            num2 = this.greater.height;
          }
        }
        int num3 = num1 - num2;
        if (num3 > 1)
        {
          if ((this.less.less != null ? this.less.less.height : 0) < (this.less.greater != null ? this.less.greater.height : 0))
          {
            this.less.rotateMtL(ref this.less);
            this.less.less.validateHeight();
            this.less.validateHeight();
          }
          _this.rotateLtM(ref _this);
          this.validateHeight();
          _this.validateHeight();
        }
        else if (num3 < -1)
        {
          if ((this.greater.less != null ? this.greater.less.height : 0) > (this.greater.greater != null ? this.greater.greater.height : 0))
          {
            this.greater.rotateLtM(ref this.greater);
            this.greater.greater.validateHeight();
            this.greater.validateHeight();
          }
          _this.rotateMtL(ref _this);
          this.validateHeight();
          _this.validateHeight();
        }
        else
          this.height = Math.Max(this.less != null ? this.less.height : 0, this.greater != null ? this.greater.height : 0) + 1;
      }

      public override string ToString() => this.key?.ToString() + ": " + this.value?.ToString();

      public Node() => this.height = 1;
    }
  }
}
