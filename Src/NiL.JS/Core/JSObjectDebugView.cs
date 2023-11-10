// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.JSObjectDebugView
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Backward;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NiL.JS.Core
{
  internal sealed class JSObjectDebugView
  {
    private JSValue _jsObject;

    public JSObjectDebugView(JSValue jsValue) => this._jsObject = jsValue;

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public KeyValuePair<string, JSValue>[] Items => this._jsObject != null ? this._jsObject.ToArray<KeyValuePair<string, JSValue>>() : EmpryArrayHelper.Empty<KeyValuePair<string, JSValue>>();
  }
}
