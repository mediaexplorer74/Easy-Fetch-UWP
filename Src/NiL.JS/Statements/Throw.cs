// Decompiled with JetBrains decompiler
// Type: NiL.JS.Statements.Throw
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using NiL.JS.Expressions;
using System;
using System.Collections.Generic;

namespace NiL.JS.Statements
{
  public sealed class Throw : CodeNode
  {
    private Expression _body;
    private Exception _exception;

    public Throw(Exception e) => this._exception = e;

    internal Throw(Expression statement) => this._body = statement;

    public CodeNode Body => (CodeNode) this._body;

    internal static CodeNode Parse(ParseInfo state, ref int index)
    {
      int index1 = index;
      if (!Parser.Validate(state.Code, "throw", ref index1) || !Parser.IsIdentifierTerminator(state.Code[index1]))
        return (CodeNode) null;
      while (index1 < state.Code.Length && Tools.IsWhiteSpace(state.Code[index1]) && !Tools.IsLineTerminator(state.Code[index1]))
        ++index1;
      Expression statement = state.Code[index1] == ';' || Tools.IsLineTerminator(state.Code[index1]) ? (Expression) null : (Expression) Parser.Parse(state, ref index1, CodeFragmentType.Expression);
      if (statement is Empty)
        ExceptionHelper.Throw((Error) new SyntaxError("Can't throw result of EmptyStatement " + CodeCoordinates.FromTextPosition(state.Code, index1 - 1, 0)?.ToString()));
      int num = index;
      index = index1;
      Throw @throw = new Throw(statement);
      @throw.Position = num;
      @throw.Length = index - num;
      return (CodeNode) @throw;
    }

    public override JSValue Evaluate(Context context)
    {
      JSValue error = this._body == null ? (this._exception == null ? JSValue.undefined : context.GlobalContext.ProxyValue((object) this._exception)) : this._body.Evaluate(context);
      if (context._executionMode == ExecutionMode.Suspend)
        return (JSValue) null;
      ExceptionHelper.Throw(error);
      return (JSValue) null;
    }

    protected internal override CodeNode[] GetChildrenImpl()
    {
      List<CodeNode> codeNodeList = new List<CodeNode>();
      codeNodeList.Add((CodeNode) this._body);
      codeNodeList.RemoveAll((Predicate<CodeNode>) (x => x == null));
      return codeNodeList.ToArray();
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
      Parser.Build(ref this._body, 2, variables, codeContext | CodeContext.InExpression, message, stats, opts);
      return false;
    }

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      if (this._body == null)
        return;
      this._body.Optimize(ref this._body, owner, message, opts, stats);
    }

    public override void Decompose(ref CodeNode self)
    {
      if (this._body == null)
        return;
      this._body.Decompose(ref this._body);
    }

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
      this._body?.RebuildScope(functionInfo, transferedVariables, scopeBias);
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => "throw" + (this._body != null ? " " + this._body?.ToString() : (this._exception != null ? "\"<native exception>\"" : ""));
  }
}
