// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.VariableReference
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Expressions;
using System;
using System.Collections.Generic;

namespace NiL.JS.Core
{
  public abstract class VariableReference : Expression
  {
    internal VariableDescriptor _descriptor;
    internal int _scopeLevel;
    private int _scopeBias;

    public VariableDescriptor Descriptor => this._descriptor;

    public int ScopeLevel
    {
      get => this._scopeLevel;
      internal set => this._scopeLevel = value + this._scopeBias;
    }

    public bool IsCacheEnabled => this._scopeLevel >= 0;

    public int ScopeBias
    {
      get => this._scopeBias;
      internal set
      {
        int num = Math.Sign(this._scopeLevel);
        this._scopeLevel -= this._scopeBias * num;
        this._scopeLevel += value * num;
        this._scopeBias = value;
      }
    }

    public abstract string Name { get; }

    protected internal override bool ContextIndependent => false;

    internal override bool ResultInTempContainer => false;

    protected internal override PredictedType ResultType => this._descriptor.lastPredictedType;

    protected internal override CodeNode[] GetChildrenImpl() => (CodeNode[]) null;

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
      this.ScopeBias = scopeBias;
      VariableDescriptor variableDescriptor = (VariableDescriptor) null;
      if (transferedVariables == null || !transferedVariables.TryGetValue(this.Name, out variableDescriptor))
        return;
      this._descriptor?.references.Remove(this);
      variableDescriptor.references.Add(this);
      this._descriptor = variableDescriptor;
    }
  }
}
