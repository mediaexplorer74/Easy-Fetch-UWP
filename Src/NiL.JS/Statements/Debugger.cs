// Decompiled with JetBrains decompiler
// Type: NiL.JS.Statements.Debugger
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using System.Collections.Generic;

namespace NiL.JS.Statements
{
  public sealed class Debugger : CodeNode
  {
    internal static CodeNode Parse(ParseInfo state, ref int index)
    {
      int index1 = index;
      if (!Parser.Validate(state.Code, "debugger", ref index1))
        return (CodeNode) null;
      int num1 = index1 ^ index;
      index ^= num1;
      int num2 = num1 ^ index;
      Debugger debugger = new Debugger();
      debugger.Position = num2;
      debugger.Length = index - num2;
      return (CodeNode) debugger;
    }

    public override JSValue Evaluate(Context context)
    {
      if (!context._debugging)
        context.raiseDebugger((CodeNode) this);
      return JSValue.undefined;
    }

    public override string ToString() => "debugger";

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override bool Build(
      ref CodeNode _this,
      int expressionDepth,
      Dictionary<string, VariableDescriptor> variables,
      CodeContext codeContext,
      InternalCompilerMessageCallback message,
      FunctionInfo stats,
      Options opts)
    {
      if (stats != null)
        stats.ContainsDebugger = true;
      return base.Build(ref _this, expressionDepth, variables, codeContext, message, stats, opts);
    }

    protected internal override CodeNode[] GetChildrenImpl() => (CodeNode[]) null;

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
    }

    public override void Decompose(ref CodeNode self)
    {
    }
  }
}
