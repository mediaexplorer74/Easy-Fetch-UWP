// Decompiled with JetBrains decompiler
// Type: NiL.JS.BaseLibrary.Symbol
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Core.Interop;
using System.Collections.Generic;

namespace NiL.JS.BaseLibrary
{
  [DisallowNewKeyword]
  public sealed class Symbol : JSValue
  {
    private static readonly Dictionary<string, Symbol> symbolsCache = new Dictionary<string, Symbol>();
    public static readonly Symbol iterator = new Symbol(nameof (iterator));
    public static readonly Symbol toStringTag = new Symbol(nameof (toStringTag));

    [Hidden]
    public string Description { get; private set; }

    public Symbol()
      : this("")
    {
    }

    public Symbol(string description)
    {
      this.Description = description;
      this._oValue = (object) this;
      this._valueType = JSValueType.Symbol;
      if (Symbol.symbolsCache.ContainsKey(description))
        return;
      Symbol.symbolsCache[description] = this;
    }

    public static Symbol @for(string description)
    {
      Symbol symbol = (Symbol) null;
      Symbol.symbolsCache.TryGetValue(description, out symbol);
      return symbol ?? new Symbol(description);
    }

    public static string keyFor(Symbol symbol)
    {
      if (symbol == null)
        ExceptionHelper.ThrowTypeError("Invalid argument");
      return symbol.Description;
    }

    public override JSValue toString(Arguments args) => (JSValue) this.ToString();

    [Hidden]
    public override string ToString() => "Symbol(" + this.Description + ")";

    protected internal override JSValue GetProperty(
      JSValue name,
      bool forWrite,
      PropertyScope memberScope)
    {
      return forWrite ? JSValue.undefined : base.GetProperty(name, false, memberScope);
    }
  }
}
