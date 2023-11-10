// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.Conditional
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NiL.JS.Expressions
{
  public sealed class Conditional : Expression
  {
    private Expression[] threads;

    protected internal override bool ContextIndependent
    {
      get
      {
        if (!base.ContextIndependent || this.threads[0] != null && !this.threads[0].ContextIndependent)
          return false;
        return this.threads[1] == null || this.threads[1].ContextIndependent;
      }
    }

    protected internal override PredictedType ResultType
    {
      get
      {
        PredictedType resultType1 = this.threads[0].ResultType;
        PredictedType resultType2 = this.threads[1].ResultType;
        if (resultType1 == resultType2)
          return resultType1;
        return Tools.IsEqual((Enum) resultType1, (Enum) resultType2, (Enum) PredictedType.Group) ? resultType1 & PredictedType.Group : PredictedType.Ambiguous;
      }
    }

    internal override bool ResultInTempContainer => false;

    public IList<Expression> Threads => (IList<Expression>) new ReadOnlyCollection<Expression>((IList<Expression>) this.threads);

    public Conditional(Expression first, Expression[] threads)
      : base(first, (Expression) null, false)
    {
      this.threads = threads;
    }

    public override JSValue Evaluate(Context context) => !(bool) this._left.Evaluate(context) ? this.threads[1].Evaluate(context) : this.threads[0].Evaluate(context);

    public override bool Build(
      ref CodeNode _this,
      int expressionDepth,
      Dictionary<string, VariableDescriptor> variables,
      CodeContext codeContext,
      InternalCompilerMessageCallback message,
      FunctionInfo stats,
      Options opts)
    {
      Parser.Build(ref this._left, expressionDepth + 1, variables, codeContext | CodeContext.Conditional | CodeContext.InExpression, message, stats, opts);
      Parser.Build(ref this.threads[0], expressionDepth, variables, codeContext | CodeContext.Conditional | CodeContext.InExpression, message, stats, opts);
      Parser.Build(ref this.threads[1], expressionDepth, variables, codeContext | CodeContext.Conditional | CodeContext.InExpression, message, stats, opts);
      if ((opts & Options.SuppressUselessExpressionsElimination) == Options.None && expressionDepth <= 1)
      {
        if (this.threads[0] == null && this.threads[1] == null)
        {
          if (this._left.ContextIndependent)
          {
            _this = (CodeNode) null;
            return false;
          }
          _this = (CodeNode) new Comma(this._left, (Expression) new Constant(JSValue.undefined));
        }
        else
        {
          if (this.threads[0] == null)
          {
            ref CodeNode local = ref _this;
            LogicalDisjunction logicalDisjunction = new LogicalDisjunction(this._left, this.threads[1]);
            logicalDisjunction.Position = this.Position;
            logicalDisjunction.Length = this.Length;
            local = (CodeNode) logicalDisjunction;
            return true;
          }
          if (this.threads[1] == null)
          {
            ref CodeNode local = ref _this;
            LogicalConjunction logicalConjunction = new LogicalConjunction(this._left, this.threads[0]);
            logicalConjunction.Position = this.Position;
            logicalConjunction.Length = this.Length;
            local = (CodeNode) logicalConjunction;
            return true;
          }
          if (this._left.ContextIndependent)
          {
            _this = (bool) this._left.Evaluate((Context) null) ? (CodeNode) this.threads[0] : (CodeNode) this.threads[1];
            return false;
          }
        }
      }
      base.Build(ref _this, expressionDepth + 1, variables, codeContext, message, stats, opts);
      return false;
    }

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      base.Optimize(ref _this, owner, message, opts, stats);
      int length = this.threads.Length;
      while (length-- > 0)
      {
        CodeNode thread = (CodeNode) this.threads[length];
        thread.Optimize(ref thread, owner, message, opts, stats);
        this.threads[length] = thread as Expression;
      }
      if (message == null || !(this.threads[0] is Variable) && !(this.threads[0] is Constant) || !(this.threads[1] is Variable) && !(this.threads[1] is Constant) || this.ResultType != PredictedType.Ambiguous)
        return;
      message(MessageLevel.Warning, this.Position, this.Length, "Type of an expression is ambiguous");
    }

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
      base.RebuildScope(functionInfo, transferedVariables, scopeBias);
      this.threads[0]?.RebuildScope(functionInfo, transferedVariables, scopeBias);
      this.threads[1]?.RebuildScope(functionInfo, transferedVariables, scopeBias);
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => "(" + this._left?.ToString() + " ? " + this.threads[0]?.ToString() + " : " + this.threads[1]?.ToString() + ")";
  }
}
