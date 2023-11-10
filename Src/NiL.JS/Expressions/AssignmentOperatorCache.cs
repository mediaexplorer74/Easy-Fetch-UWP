// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.AssignmentOperatorCache
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using System.Collections.Generic;

namespace NiL.JS.Expressions
{
  public sealed class AssignmentOperatorCache : Expression
  {
    private JSValue secondResult;

    public CodeNode Source => (CodeNode) this._left;

    protected internal override bool ContextIndependent => false;

    protected internal override PredictedType ResultType => this._left.ResultType;

    internal override bool ResultInTempContainer => false;

    internal AssignmentOperatorCache(Expression source)
      : base(source, (Expression) null, false)
    {
    }

    protected internal override JSValue EvaluateForWrite(Context context)
    {
      JSValue forWrite = this._left.EvaluateForWrite(context);
      this.secondResult = Tools.InvokeGetter(forWrite, context._objectSource);
      return forWrite;
    }

    public override JSValue Evaluate(Context context)
    {
      JSValue secondResult = this.secondResult;
      this.secondResult = (JSValue) null;
      return secondResult;
    }

    public override string ToString() => this._left.ToString();

    public override int Length
    {
      get => this._left.Length;
      internal set => this._left.Length = value;
    }

    public override int Position
    {
      get => this._left.Position;
      internal set => this._left.Position = value;
    }

    protected internal override CodeNode[] GetChildrenImpl() => this._left.Children;

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit((Expression) this);

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      base.Optimize(ref _this, owner, message, opts, stats);
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
      if (this._right != null)
        return false;
      this._right = this._left;
      this._codeContext = codeContext;
      int num = this._left.Build(ref _this, expressionDepth, variables, codeContext | CodeContext.InExpression, message, stats, opts) ? 1 : 0;
      if (num != 0)
        return num != 0;
      if (!(this._left is Variable))
        return num != 0;
      (this._left as Variable)._forceThrow = true;
      return num != 0;
    }
  }
}
