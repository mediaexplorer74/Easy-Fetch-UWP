// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.New
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using System;
using System.Collections.Generic;

namespace NiL.JS.Expressions
{
  public sealed class New : Expression
  {
    protected internal override bool ContextIndependent => false;

    internal override bool ResultInTempContainer => false;

    protected internal override PredictedType ResultType => PredictedType.Object;

    internal New(Call call)
      : base((Expression) call, (Expression) null, false)
    {
    }

    public static CodeNode Parse(ParseInfo state, ref int index)
    {
      int index1 = index;
      if (!Parser.Validate(state.Code, "new", ref index1) || !Parser.IsIdentifierTerminator(state.Code[index1]))
        return (CodeNode) null;
      while (Tools.IsWhiteSpace(state.Code[index1]))
        ++index1;
      Expression first = ExpressionTree.Parse(state, ref index1, true, false, true);
      if (first == null)
        ExceptionHelper.Throw((Error) new SyntaxError("Invalid prefix operation. " + CodeCoordinates.FromTextPosition(state.Code, index1, 0)?.ToString()));
      Expression expression;
      if (first is Call)
      {
        New @new = new New(first as Call);
        @new.Position = index;
        @new.Length = index1 - index;
        expression = (Expression) @new;
      }
      else
      {
        if (state.Message != null)
          state.Message(MessageLevel.Warning, index, 0, "Missed brackets in a constructor invocation.");
        Call call = new Call(first, new Expression[0]);
        call.Position = first.Position;
        call.Length = first.Length;
        New @new = new New(call);
        @new.Position = index;
        @new.Length = index1 - index;
        expression = (Expression) @new;
      }
      index = index1;
      return (CodeNode) expression;
    }

    public override JSValue Evaluate(Context context) => throw new InvalidOperationException();

    public override bool Build(
      ref CodeNode _this,
      int expressionDepth,
      Dictionary<string, VariableDescriptor> variables,
      CodeContext codeContext,
      InternalCompilerMessageCallback message,
      FunctionInfo stats,
      Options opts)
    {
      if (message != null && expressionDepth <= 1)
        message(MessageLevel.Warning, this.Position, 0, "Do not use NewOperator for side effect");
      (this._left as Call)._callMode = CallMode.Construct;
      _this = (CodeNode) this._left;
      return true;
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => "new " + this._left.ToString();
  }
}
