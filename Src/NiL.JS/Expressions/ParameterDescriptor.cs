// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.ParameterDescriptor
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;

namespace NiL.JS.Expressions
{
  public sealed class ParameterDescriptor : VariableDescriptor
  {
    public ObjectDesctructor Destructor { get; internal set; }

    public bool IsRest { get; private set; }

    internal ParameterDescriptor(string name, bool rest, int depth)
      : base(name, depth)
    {
      this.IsRest = rest;
      this.lexicalScope = true;
    }

    public override string ToString() => this.IsRest ? "..." + this.name : this.name;
  }
}
