// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.StringMapDebugView`1
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using System.Collections.Generic;
using System.Diagnostics;

namespace NiL.JS.Core
{
  internal sealed class StringMapDebugView<TValue>
  {
    private StringMap<TValue> stringMap;

    public StringMapDebugView(StringMap<TValue> stringMap) => this.stringMap = stringMap;

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public KeyValuePair<string, TValue>[] Items => new List<KeyValuePair<string, TValue>>((IEnumerable<KeyValuePair<string, TValue>>) this.stringMap).ToArray();
  }
}
