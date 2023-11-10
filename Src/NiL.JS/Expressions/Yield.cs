// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.Yield
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using NiL.JS.Extensions;
using NiL.JS.Statements;
using System;
using System.Collections.Generic;

namespace NiL.JS.Expressions
{
  public sealed class Yield : Expression
  {
    private bool _reiterate;

    internal override bool ResultInTempContainer => false;

    protected internal override bool ContextIndependent => false;

    protected internal override bool NeedDecompose => true;

    public bool Reiterate => this._reiterate;

    public Yield(Expression first, bool reiterate)
      : base(first, (Expression) null, true)
    {
      this._reiterate = reiterate;
    }

    public static CodeNode Parse(ParseInfo state, ref int index)
    {
      if ((state.CodeContext & CodeContext.InGenerator) == CodeContext.None)
        ExceptionHelper.Throw((Error) new SyntaxError("Invalid use of yield operator"));
      int num = index;
      if (!Parser.Validate(state.Code, "yield", ref num))
        return (CodeNode) null;
      Tools.SkipSpaces(state.Code, ref num);
      bool reiterate = false;
      if (state.Code[num] == '*')
      {
        reiterate = true;
        do
        {
          ++num;
        }
        while (Tools.IsWhiteSpace(state.Code[num]));
      }
      Expression first = ExpressionTree.Parse(state, ref num, processComma: false, forForLoop: true);
      if (first == null)
        ExceptionHelper.ThrowSyntaxError("Invalid prefix operation", state.Code, num);
      index = num;
      Yield yield = new Yield(first, reiterate);
      yield.Position = index;
      yield.Length = num - index;
      return (CodeNode) yield;
    }

    public override JSValue Evaluate(Context context)
    {
      if (context._executionMode == ExecutionMode.ResumeThrow)
      {
        context.SuspendData.Clear();
        context._executionMode = ExecutionMode.Regular;
        ExceptionHelper.Throw(context._executionInfo);
      }
      if (this._reiterate)
      {
        if (context._executionMode == ExecutionMode.Regular)
        {
          IIterator iterator = IterationProtocolExtensions.AsIterable(this._left.Evaluate(context)).iterator();
          IIteratorResult iteratorResult = iterator.next();
          if (iteratorResult.done)
            return JSValue.undefined;
          context.SuspendData[(CodeNode) this] = (object) iterator;
          context._executionInfo = iteratorResult.value;
          context._executionMode = ExecutionMode.Suspend;
          return JSValue.notExists;
        }
        if (context._executionMode == ExecutionMode.Resume)
        {
          IIterator iterator1 = context.SuspendData[(CodeNode) this] as IIterator;
          IIterator iterator2 = iterator1;
          Arguments arguments;
          if (!context._executionInfo.Defined)
          {
            arguments = (Arguments) null;
          }
          else
          {
            arguments = new Arguments();
            arguments.Add(context._executionInfo);
          }
          IIteratorResult iteratorResult = iterator2.next(arguments);
          context._executionInfo = iteratorResult.value;
          if (iteratorResult.done)
          {
            context._executionMode = ExecutionMode.Regular;
            return iteratorResult.value;
          }
          context.SuspendData[(CodeNode) this] = (object) iterator1;
          context._executionMode = ExecutionMode.Suspend;
          return JSValue.notExists;
        }
      }
      else
      {
        if (context._executionMode == ExecutionMode.Regular)
        {
          context._executionInfo = this._left.Evaluate(context);
          context._executionMode = ExecutionMode.Suspend;
          return JSValue.notExists;
        }
        if (context._executionMode == ExecutionMode.Resume)
        {
          context._executionMode = ExecutionMode.Regular;
          JSValue executionInfo = context._executionInfo;
          context._executionInfo = (JSValue) null;
          return executionInfo;
        }
      }
      throw new InvalidOperationException();
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
      stats.NeedDecompose = true;
      return base.Build(ref _this, expressionDepth, variables, codeContext, message, stats, opts);
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit((Expression) this);

    public override void Decompose(ref Expression self, IList<CodeNode> result)
    {
      this._left.Decompose(ref this._left, result);
      if ((this._codeContext & CodeContext.InExpression) == CodeContext.None)
        return;
      result.Add((CodeNode) new StoreValue((Expression) this, false));
      self = (Expression) new ExtractStoredValue((Expression) this);
    }

    public override string ToString() => "yield" + (this._reiterate ? "* " : " ") + this._left?.ToString();
  }
}
