// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.ContextDebuggerProxy
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using System.Collections.Generic;
using System.Diagnostics;

namespace NiL.JS.Core
{
  public sealed class ContextDebuggerProxy
  {
    private readonly Context _context;

    public ContextDebuggerProxy(Context context) => this._context = context;

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public IEnumerable<KeyValuePair<string, JSValue>> Variables
    {
      get
      {
        List<KeyValuePair<string, JSValue>> keyValuePairList = new List<KeyValuePair<string, JSValue>>();
        foreach (string str in this._context)
          keyValuePairList.Add(new KeyValuePair<string, JSValue>(str, this._context.GetVariable(str, false)));
        return (IEnumerable<KeyValuePair<string, JSValue>>) keyValuePairList.ToArray();
      }
    }
  }
}
