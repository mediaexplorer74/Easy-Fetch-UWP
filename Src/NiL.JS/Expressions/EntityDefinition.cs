// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.EntityDefinition
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using System;
using System.Collections.Generic;

namespace NiL.JS.Expressions
{
  public abstract class EntityDefinition : Expression
  {
    internal readonly VariableReference reference;
    internal readonly string _name;

    [CLSCompliant(false)]
    protected bool Built { get; set; }

    public string Name => this._name;

    public VariableReference Reference => this.reference;

    public abstract bool Hoist { get; }

    protected EntityDefinition(string name)
    {
      this._name = name;
      this.reference = (VariableReference) new EntityReference(this);
    }

    public override bool Build(
      ref CodeNode _this,
      int expressionDepth,
      Dictionary<string, VariableDescriptor> variables,
      CodeContext codeContext,
      InternalCompilerMessageCallback message,
      FunctionInfo stats,
      Options opts)
    {
      this._codeContext = codeContext;
      return false;
    }

    public abstract override void Decompose(ref Expression self, IList<CodeNode> result);

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
      this.reference.ScopeBias = scopeBias;
      if (this.reference._descriptor == null || this.reference._descriptor.definitionScopeLevel < 0)
        return;
      this.reference._descriptor.definitionScopeLevel = this.reference.ScopeLevel;
      this.reference._descriptor.scopeBias = scopeBias;
    }
  }
}
