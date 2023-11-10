// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.ParameterReference
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using System;

namespace NiL.JS.Expressions
{
  public sealed class ParameterReference : VariableReference
  {
    public override string Name => this._descriptor.name;

    internal ParameterReference(string name, bool rest, int depth)
    {
      this._descriptor = (VariableDescriptor) new ParameterDescriptor(name, rest, depth);
      this._descriptor.references.Add((VariableReference) this);
    }

    public override JSValue Evaluate(Context context)
    {
      if (this._descriptor.cacheContext != context)
        throw new InvalidOperationException();
      return this._descriptor.cacheRes;
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit((VariableReference) this);

    public override string ToString() => this.Descriptor.ToString();
  }
}
