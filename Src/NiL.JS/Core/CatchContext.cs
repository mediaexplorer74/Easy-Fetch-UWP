// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.CatchContext
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using System;

namespace NiL.JS.Core
{
  internal sealed class CatchContext : Context
  {
    private JSValue errorContainer;
    private Context prototype;
    private string errorVariableName;

    internal CatchContext(JSValue e, Context proto, string name)
      : base(proto, false, proto._owner)
    {
      if (e == null)
        throw new ArgumentNullException();
      if (proto == null)
        throw new ArgumentNullException();
      if (string.IsNullOrEmpty(name))
        throw new ArgumentException();
      this.errorContainer = e;
      this.prototype = proto;
      this.errorVariableName = name;
      this._strict = proto._strict;
      this._variables = proto._variables;
    }

    public override JSValue DefineVariable(string name, bool deletable) => this.prototype.DefineVariable(name);

    protected internal override JSValue GetVariable(string name, bool forWrite) => name == this.errorVariableName && this.errorContainer.Exists ? this.errorContainer : base.GetVariable(name, forWrite);
  }
}
