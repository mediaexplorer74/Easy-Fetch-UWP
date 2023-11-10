// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.AwaitExpression
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using NiL.JS.Statements;
using System.Collections.Generic;

namespace NiL.JS.Expressions
{
  public sealed class AwaitExpression : Expression
  {
    protected internal override bool ContextIndependent => false;

    protected internal override bool NeedDecompose => true;

    public AwaitExpression(Expression source)
      : base(source, (Expression) null, false)
    {
    }

    public override JSValue Evaluate(Context context)
    {
      if (context._executionMode == ExecutionMode.ResumeThrow)
      {
        if ((bool) context.SuspendData[(CodeNode) this])
        {
          context._executionMode = ExecutionMode.Regular;
          throw new JSException(context._executionInfo);
        }
      }
      else if (context._executionMode == ExecutionMode.Resume && (bool) context.SuspendData[(CodeNode) this])
      {
        context._executionMode = ExecutionMode.Regular;
        return context._executionInfo;
      }
      JSValue jsValue = this._left.Evaluate(context);
      if (context._executionMode != ExecutionMode.Regular)
      {
        if (context._executionMode == ExecutionMode.Suspend)
          context.SuspendData[(CodeNode) this] = (object) false;
        return (JSValue) null;
      }
      if (jsValue != null && (jsValue._valueType < JSValueType.Object || !(jsValue.Value is Promise)))
        return jsValue;
      context._executionMode = ExecutionMode.Suspend;
      context._executionInfo = jsValue;
      context.SuspendData[(CodeNode) this] = (object) true;
      return (JSValue) null;
    }

    public static CodeNode Parse(ParseInfo state, ref int index)
    {
      int num = index;
      if (!Parser.Validate(state.Code, "await", ref num) || !Parser.IsIdentifierTerminator(state.Code[num]))
        return (CodeNode) null;
      if ((state.CodeContext & CodeContext.InAsync) == CodeContext.None)
        ExceptionHelper.ThrowSyntaxError("await is not allowed in this context", state.Code, index, "await".Length);
      Tools.SkipSpaces(state.Code, ref num);
      Expression source = ExpressionTree.Parse(state, ref num, processComma: false, forForLoop: true);
      if (source == null)
        ExceptionHelper.ThrowSyntaxError("Expression missed", state.Code, num);
      index = num;
      return (CodeNode) new AwaitExpression(source);
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

    public override void Decompose(ref Expression self, IList<CodeNode> result)
    {
      this._left.Decompose(ref this._left, result);
      if ((this._codeContext & CodeContext.InExpression) == CodeContext.None)
        return;
      result.Add((CodeNode) new StoreValue((Expression) this, false));
      self = (Expression) new ExtractStoredValue((Expression) this);
    }

    public override string ToString() => "await " + this._left?.ToString();
  }
}
