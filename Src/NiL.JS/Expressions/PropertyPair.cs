// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.PropertyPair
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using System;
using System.Collections.Generic;

namespace NiL.JS.Expressions
{
  public sealed class PropertyPair : Expression
  {
    public Expression Getter
    {
      get => this._left;
      internal set => this._left = value;
    }

    public Expression Setter
    {
      get => this._right;
      internal set => this._right = value;
    }

    protected internal override bool ContextIndependent => false;

    public PropertyPair(Expression getter, Expression setter)
      : base(getter, setter, true)
    {
      this._tempContainer._valueType = JSValueType.Property;
    }

    public override JSValue Evaluate(Context context)
    {
      this._tempContainer._oValue = (object) new NiL.JS.Core.PropertyPair(this.Getter == null ? (Function) null : (Function) this.Getter.Evaluate(context), this.Setter == null ? (Function) null : (Function) this.Setter.Evaluate(context));
      return this._tempContainer;
    }

    public override void Decompose(ref Expression self, IList<CodeNode> result) => throw new InvalidOperationException();
  }
}
