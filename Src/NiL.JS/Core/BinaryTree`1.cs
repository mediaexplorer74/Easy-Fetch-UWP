// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.BinaryTree`1
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using System.Collections.Generic;

namespace NiL.JS.Core
{
  public sealed class BinaryTree<TValue> : BinaryTree<string, TValue>
  {
    public IEnumerable<KeyValuePair<string, TValue>> StartsWith(string prefix) => this.StartedWith(prefix, false, 0L, (long) int.MaxValue);

    public IEnumerable<KeyValuePair<string, TValue>> StartedWith(string prefix, bool reversed) => this.StartedWith(prefix, reversed, 0L, (long) int.MaxValue);

    public IEnumerable<KeyValuePair<string, TValue>> StartedWith(
      string prefix,
      bool reversed,
      long offset)
    {
      return this.StartedWith(prefix, reversed, offset, (long) int.MaxValue);
    }

    public IEnumerable<KeyValuePair<string, TValue>> StartedWith(
      string prefix,
      bool reversed,
      long offset,
      long count)
    {
      BinaryTree<TValue> binaryTree = this;
      HashSet<string> stringSet = new HashSet<string>();
      BinaryTree<string, TValue>.Node node = binaryTree.Root;
      if (node != null)
      {
        do
        {
          int num = node.key.StartsWith(prefix) ? 0 : prefix.CompareTo(node.key);
          if (num == 0)
          {
            IEnumerator<BinaryTree<string, TValue>.Node> enmrtr = reversed ? binaryTree.enumerateReversed(node) : binaryTree.enumerate(node);
            while (count-- > 0L && enmrtr.MoveNext())
            {
              if (offset-- > 0L)
              {
                ++count;
              }
              else
              {
                BinaryTree<string, TValue>.Node current = enmrtr.Current;
                if (current.key.StartsWith(prefix))
                  yield return new KeyValuePair<string, TValue>(current.key, current.value);
              }
            }
            goto label_2;
          }
          else
            node = num <= 0 ? node.less : node.greater;
        }
        while (node != null);
        goto label_4;
label_2:
        yield break;
label_4:;
      }
    }
  }
}
