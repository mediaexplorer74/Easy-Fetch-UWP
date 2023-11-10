// Decompiled with JetBrains decompiler
// Type: NiL.JS.Statements.InfinityLoop
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Expressions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NiL.JS.Statements
{
  public sealed class InfinityLoop : CodeNode
  {
    private CodeNode body;
    private string[] labels;

    public CodeNode Body => this.body;

    public ReadOnlyCollection<string> Labels => new ReadOnlyCollection<string>((IList<string>) this.labels);

    internal InfinityLoop(CodeNode body, string[] labels)
    {
      this.body = body ?? (CodeNode) new Empty();
      this.labels = labels;
    }

    public override JSValue Evaluate(Context context)
    {
      bool flag1;
      do
      {
        do
        {
          if (context._debugging && !(this.body is CodeBlock))
            context.raiseDebugger(this.body);
          context._lastResult = this.body.Evaluate(context) ?? context._lastResult;
        }
        while (context._executionMode == ExecutionMode.Regular);
        if (context._executionMode < ExecutionMode.Return)
        {
          bool flag2 = context._executionInfo == null || Array.IndexOf<string>(this.labels, context._executionInfo._oValue as string) != -1;
          flag1 = context._executionMode > ExecutionMode.Continue || !flag2;
          if (flag2)
          {
            context._executionMode = ExecutionMode.Regular;
            context._executionInfo = JSValue.notExists;
          }
        }
        else
          goto label_8;
      }
      while (!flag1);
      return (JSValue) null;
label_8:
      return (JSValue) null;
    }

    protected internal override CodeNode[] GetChildrenImpl() => new CodeNode[1]
    {
      this.body
    };

    public override bool Build(
      ref CodeNode _this,
      int expressionDepth,
      Dictionary<string, VariableDescriptor> variables,
      CodeContext codeContext,
      InternalCompilerMessageCallback message,
      FunctionInfo stats,
      Options opts)
    {
      return false;
    }

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      this.body.Optimize(ref this.body, owner, message, opts, stats);
    }

    public override void Decompose(ref CodeNode self) => this.body.Decompose(ref this.body);

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
      this.body.RebuildScope(functionInfo, transferedVariables, scopeBias);
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => "for (;;)" + (this.body is CodeBlock ? "" : Environment.NewLine + "  ") + this.body?.ToString();
  }
}
