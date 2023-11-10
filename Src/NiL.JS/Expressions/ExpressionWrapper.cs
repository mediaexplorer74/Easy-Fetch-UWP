// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.ExpressionWrapper
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using System.Collections.Generic;

namespace NiL.JS.Expressions
{
  public sealed class ExpressionWrapper : Expression
  {
    private CodeNode node;

    public CodeNode Node => this.node;

    protected internal override bool ContextIndependent => false;

    internal override bool ResultInTempContainer => false;

    public ExpressionWrapper(CodeNode node) => this.node = node;

    public override JSValue Evaluate(Context context) => this.node.Evaluate(context);

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit((Expression) this);

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
      return this.node.Build(ref this.node, expressionDepth, variables, codeContext | CodeContext.InExpression, message, stats, opts);
    }
  }
}
