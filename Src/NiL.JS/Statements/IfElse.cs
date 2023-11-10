// Decompiled with JetBrains decompiler
// Type: NiL.JS.Statements.IfElse
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Expressions;
using System;
using System.Collections.Generic;

namespace NiL.JS.Statements
{
  public sealed class IfElse : CodeNode
  {
    private Expression condition;
    private CodeNode then;
    private CodeNode @else;

    public CodeNode Then => this.then;

    public CodeNode Else => this.@else;

    public Expression Condition => this.condition;

    private IfElse()
    {
    }

    public IfElse(Expression condition, CodeNode body, CodeNode elseBody)
    {
      this.condition = condition;
      this.then = body;
      this.@else = elseBody;
    }

    internal static CodeNode Parse(ParseInfo state, ref int index)
    {
      int index1 = index;
      if (!Parser.Validate(state.Code, "if (", ref index1) && !Parser.Validate(state.Code, "if(", ref index1))
        return (CodeNode) null;
      while (Tools.IsWhiteSpace(state.Code[index1]))
        ++index1;
      Expression expression = (Expression) ExpressionTree.Parse(state, ref index1);
      while (Tools.IsWhiteSpace(state.Code[index1]))
        ++index1;
      if (state.Code[index1] != ')')
        throw new ArgumentException("code (" + index1.ToString() + ")");
      do
      {
        ++index1;
      }
      while (Tools.IsWhiteSpace(state.Code[index1]));
      bool flag = state.Code[index1] == '{';
      CodeNode codeNode1 = Parser.Parse(state, ref index1, CodeFragmentType.Statement);
      if (codeNode1 is FunctionDefinition)
      {
        if (state.Message != null)
          state.Message(MessageLevel.CriticalWarning, codeNode1.Position, codeNode1.Length, Strings.DoNotDeclareFunctionInNestedBlocks);
        codeNode1 = (CodeNode) new CodeBlock(new CodeNode[1]
        {
          codeNode1
        });
      }
      CodeNode codeNode2 = (CodeNode) null;
      int num1 = index1;
      while (index1 < state.Code.Length && Tools.IsWhiteSpace(state.Code[index1]))
        ++index1;
      if (index1 < state.Code.Length && !flag && state.Code[index1] == ';')
      {
        do
        {
          ++index1;
        }
        while (index1 < state.Code.Length && Tools.IsWhiteSpace(state.Code[index1]));
      }
      if (Parser.Validate(state.Code, "else", ref index1))
      {
        while (Tools.IsWhiteSpace(state.Code[index1]))
          ++index1;
        codeNode2 = Parser.Parse(state, ref index1, CodeFragmentType.Statement);
        if (codeNode2 is FunctionDefinition)
        {
          if (state.Message != null)
            state.Message(MessageLevel.CriticalWarning, codeNode2.Position, codeNode2.Length, Strings.DoNotDeclareFunctionInNestedBlocks);
          codeNode2 = (CodeNode) new CodeBlock(new CodeNode[1]
          {
            codeNode2
          });
        }
      }
      else
        index1 = num1;
      int num2 = index;
      index = index1;
      IfElse ifElse = new IfElse();
      ifElse.then = codeNode1;
      ifElse.condition = expression;
      ifElse.@else = codeNode2;
      ifElse.Position = num2;
      ifElse.Length = index - num2;
      return (CodeNode) ifElse;
    }

    public override JSValue Evaluate(Context context)
    {
      bool flag;
      if (context._executionMode != ExecutionMode.Resume || !context.SuspendData.ContainsKey((CodeNode) this))
      {
        if (context._debugging)
          context.raiseDebugger((CodeNode) this.condition);
        flag = (bool) this.condition.Evaluate(context);
        if (context._executionMode == ExecutionMode.Suspend)
          return (JSValue) null;
      }
      else
        flag = (bool) context.SuspendData[(CodeNode) this];
      if (flag)
      {
        if (this.then != null)
        {
          if (context._debugging && !(this.then is CodeBlock))
            context.raiseDebugger(this.then);
          JSValue jsValue = this.then.Evaluate(context);
          if (jsValue != null)
            context._lastResult = jsValue;
        }
      }
      else if (this.@else != null)
      {
        if (context._debugging && !(this.@else is CodeBlock))
          context.raiseDebugger(this.@else);
        JSValue jsValue = this.@else.Evaluate(context);
        if (jsValue != null)
          context._lastResult = jsValue;
      }
      if (context._executionMode == ExecutionMode.Suspend)
        context.SuspendData[(CodeNode) this] = (object) flag;
      return (JSValue) null;
    }

    protected internal override CodeNode[] GetChildrenImpl()
    {
      List<CodeNode> codeNodeList = new List<CodeNode>();
      codeNodeList.Add(this.then);
      codeNodeList.Add((CodeNode) this.condition);
      codeNodeList.Add(this.@else);
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
      Parser.Build(ref this.condition, 2, variables, codeContext | CodeContext.InExpression, message, stats, opts);
      Parser.Build<CodeNode>(ref this.then, expressionDepth, variables, codeContext | CodeContext.Conditional, message, stats, opts);
      Parser.Build<CodeNode>(ref this.@else, expressionDepth, variables, codeContext | CodeContext.Conditional, message, stats, opts);
      if ((opts & Options.SuppressUselessStatementsElimination) == Options.None && this.condition is ConvertToBoolean)
      {
        if (message != null)
          message(MessageLevel.Warning, this.condition.Position, 2, "Useless conversion. Remove double negation in condition");
        this.condition = this.condition._left;
      }
      try
      {
        if ((opts & Options.SuppressUselessStatementsElimination) == Options.None)
        {
          if (!(this.condition is Constant))
          {
            if (!this.condition.ContextIndependent)
              goto label_10;
          }
          _this = !(bool) this.condition.Evaluate((Context) null) ? this.@else : this.then;
          this.condition.Eliminated = true;
        }
      }
      catch
      {
      }
label_10:
      return false;
    }

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      CodeNode condition = (CodeNode) this.condition;
      this.condition.Optimize(ref condition, owner, message, opts, stats);
      this.condition = (Expression) condition;
      if ((opts & Options.SuppressUselessStatementsElimination) != Options.None)
        return;
      if (this.then != null)
        this.then.Optimize(ref this.then, owner, message, opts, stats);
      if (this.@else != null)
        this.@else.Optimize(ref this.@else, owner, message, opts, stats);
      if (this.then != null || this.@else != null)
        return;
      _this = (CodeNode) new Comma(this.condition, (Expression) new Constant(JSValue.undefined));
    }

    public override void Decompose(ref CodeNode self)
    {
      this.condition.Decompose(ref this.condition);
      if (this.then != null)
        this.then.Decompose(ref this.then);
      if (this.@else == null)
        return;
      this.@else.Decompose(ref this.@else);
    }

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
      this.condition?.RebuildScope(functionInfo, transferedVariables, scopeBias);
      this.then?.RebuildScope(functionInfo, transferedVariables, scopeBias);
      this.@else?.RebuildScope(functionInfo, transferedVariables, scopeBias);
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString()
    {
      string newLine = Environment.NewLine;
      string newValue = Environment.NewLine + "  ";
      string str1 = this.then.ToString();
      string str2 = this.@else == null ? "" : this.@else.ToString();
      return "if (" + this.condition?.ToString() + ")" + (this.then is CodeBlock ? str1 : Environment.NewLine + "  " + str1.Replace(newLine, newValue)) + (this.@else != null ? Environment.NewLine + "else" + Environment.NewLine + (this.@else is CodeBlock ? str2.Replace(newLine, newValue) : "  " + str2) : "");
    }
  }
}
