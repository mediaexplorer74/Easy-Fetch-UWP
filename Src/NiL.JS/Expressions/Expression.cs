// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.Expression
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Statements;
using System;
using System.Collections.Generic;

namespace NiL.JS.Expressions
{
  public abstract class Expression : CodeNode
  {
    internal Expression _left;
    internal Expression _right;
    internal JSValue _tempContainer;
    internal CodeContext _codeContext;

    protected internal virtual PredictedType ResultType => PredictedType.Unknown;

    internal virtual bool ResultInTempContainer => false;

    public Expression LeftOperand => this._left;

    public Expression RightOperand => this._right;

    public override bool Eliminated
    {
      get => base.Eliminated;
      internal set
      {
        if (this._left != null)
          this._left.Eliminated = true;
        if (this._right != null)
          this._right.Eliminated = true;
        base.Eliminated = value;
      }
    }

    protected internal virtual bool LValueModifier => false;

    protected internal virtual bool ContextIndependent
    {
      get
      {
        if (this._left != null && !this._left.ContextIndependent)
          return false;
        return this._right == null || this._right.ContextIndependent;
      }
    }

    protected internal virtual bool NeedDecompose
    {
      get
      {
        if (this._left != null && this._left.NeedDecompose)
          return true;
        return this._right != null && this._right.NeedDecompose;
      }
    }

    protected Expression()
    {
    }

    protected Expression(Expression first, Expression second, bool createTempContainer)
    {
      this._left = first;
      this._right = second;
      if (!createTempContainer)
        return;
      this._tempContainer = new JSValue()
      {
        _attributes = JSValueAttributesInternal.Temporary
      };
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
      codeContext |= CodeContext.InExpression;
      Parser.Build(ref this._left, expressionDepth + 1, variables, codeContext, message, stats, opts);
      Parser.Build(ref this._right, expressionDepth + 1, variables, codeContext, message, stats, opts);
      if (!this.ContextIndependent)
        return false;
      if (message != null && !(this is RegExpExpression))
        message(MessageLevel.Warning, this.Position, this.Length, "Constant expression. Maybe, it's a mistake.");
      try
      {
        JSValue jsValue = this.Evaluate((Context) null);
        if (jsValue._valueType == JSValueType.Double && !Tools.IsNegativeZero(jsValue._dValue) && jsValue._dValue == (double) (int) jsValue._dValue)
        {
          jsValue._iValue = (int) jsValue._dValue;
          jsValue._valueType = JSValueType.Integer;
        }
        _this = (CodeNode) new Constant(jsValue);
        return true;
      }
      catch (JSException ex)
      {
        _this = (CodeNode) new ExpressionWrapper((CodeNode) new Throw((Expression) new Constant(ex.Error)));
        this.expressionWillThrow(message);
        return true;
      }
      catch (Exception ex)
      {
        _this = (CodeNode) new ExpressionWrapper((CodeNode) new Throw(ex));
        this.expressionWillThrow(message);
        return true;
      }
    }

    internal void Optimize(
      ref Expression self,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      CodeNode _this = (CodeNode) self;
      this.Optimize(ref _this, owner, message, opts, stats);
      self = (Expression) _this;
    }

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      this.baseOptimize(ref _this, owner, message, opts, stats);
    }

    internal void baseOptimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      CodeNode left = (CodeNode) this._left;
      CodeNode right = (CodeNode) this._right;
      if (left != null)
      {
        left.Optimize(ref left, owner, message, opts, stats);
        this._left = left as Expression;
      }
      if (right != null)
      {
        right.Optimize(ref right, owner, message, opts, stats);
        this._right = right as Expression;
      }
      if (!this.ContextIndependent)
        return;
      if (this is Constant)
        return;
      try
      {
        _this = (CodeNode) new Constant(this.Evaluate((Context) null));
      }
      catch (JSException ex)
      {
        _this = (CodeNode) new ExpressionWrapper((CodeNode) new Throw((Expression) new Constant(ex.Error)));
        this.expressionWillThrow(message);
      }
      catch (Exception ex)
      {
        _this = (CodeNode) new ExpressionWrapper((CodeNode) new Throw(ex));
        this.expressionWillThrow(message);
      }
    }

    private void expressionWillThrow(InternalCompilerMessageCallback message)
    {
      if (message == null || this is RegExpExpression)
        return;
      message(MessageLevel.Warning, this.Position, this.Length, "Expression will throw an exception");
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    protected internal override CodeNode[] GetChildrenImpl()
    {
      if (this._left != null && this._right != null)
        return new CodeNode[2]
        {
          (CodeNode) this._left,
          (CodeNode) this._right
        };
      if (this._left != null)
        return new CodeNode[1]{ (CodeNode) this._left };
      if (this._right == null)
        return (CodeNode[]) null;
      return new CodeNode[1]{ (CodeNode) this._right };
    }

    protected internal void Decompose(ref Expression self)
    {
      CodeNode self1 = (CodeNode) self;
      self1.Decompose(ref self1);
      self = (Expression) self1;
    }

    public override sealed void Decompose(ref CodeNode self)
    {
      if (!this.NeedDecompose)
        return;
      List<CodeNode> result = new List<CodeNode>();
      Expression self1 = this;
      self1.Decompose(ref self1, (IList<CodeNode>) result);
      if (result.Count <= 0)
        return;
      self = (CodeNode) new SuspendableExpression(this, result.ToArray());
    }

    public virtual void Decompose(ref Expression self, IList<CodeNode> result)
    {
      if (this._left != null)
        this._left.Decompose(ref this._left, result);
      if (this._right == null)
        return;
      if (this._right.NeedDecompose && !(this._left is ExtractStoredValue))
      {
        result.Add((CodeNode) new StoreValue(this._left, this.LValueModifier));
        this._left = (Expression) new ExtractStoredValue(this._left);
      }
      this._right.Decompose(ref this._right, result);
    }

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
      this._left?.RebuildScope(functionInfo, transferedVariables, scopeBias);
      this._right?.RebuildScope(functionInfo, transferedVariables, scopeBias);
    }
  }
}
