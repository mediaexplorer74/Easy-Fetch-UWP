// Decompiled with JetBrains decompiler
// Type: NiL.JS.Statements.DoWhile
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using NiL.JS.Expressions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NiL.JS.Statements
{
  public sealed class DoWhile : CodeNode
  {
    private bool allowRemove;
    private CodeNode condition;
    private CodeNode body;
    private string[] labels;

    public CodeNode Condition => this.condition;

    public CodeNode Body => this.body;

    public ICollection<string> Labels => (ICollection<string>) new ReadOnlyCollection<string>((IList<string>) this.labels);

    private DoWhile()
    {
    }

    internal static CodeNode Parse(ParseInfo state, ref int index)
    {
      int index1 = index;
      while (Tools.IsWhiteSpace(state.Code[index1]))
        ++index1;
      if (!Parser.Validate(state.Code, "do", ref index1) || !Parser.IsIdentifierTerminator(state.Code[index1]))
        return (CodeNode) null;
      int labelsCount = state.LabelsCount;
      state.LabelsCount = 0;
      while (Tools.IsWhiteSpace(state.Code[index1]))
        ++index1;
      state.AllowBreak.Push(true);
      state.AllowContinue.Push(true);
      int continiesCount = state.ContiniesCount;
      int breaksCount = state.BreaksCount;
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
      state.AllowBreak.Pop();
      state.AllowContinue.Pop();
      if (!(codeNode1 is CodeBlock) && state.Code[index1] == ';')
        ++index1;
      while (index1 < state.Code.Length && Tools.IsWhiteSpace(state.Code[index1]))
        ++index1;
      if (index1 >= state.Code.Length)
        ExceptionHelper.ThrowSyntaxError(Strings.UnexpectedEndOfSource);
      if (!Parser.Validate(state.Code, "while", ref index1))
        ExceptionHelper.Throw((Error) new SyntaxError("Expected \"while\" at + " + CodeCoordinates.FromTextPosition(state.Code, index1, 0)?.ToString()));
      while (Tools.IsWhiteSpace(state.Code[index1]))
        ++index1;
      if (state.Code[index1] != '(')
        ExceptionHelper.Throw((Error) new SyntaxError("Expected \"(\" at + " + CodeCoordinates.FromTextPosition(state.Code, index1, 0)?.ToString()));
      do
      {
        ++index1;
      }
      while (Tools.IsWhiteSpace(state.Code[index1]));
      CodeNode codeNode2 = Parser.Parse(state, ref index1, CodeFragmentType.Expression);
      while (Tools.IsWhiteSpace(state.Code[index1]))
        ++index1;
      if (state.Code[index1] != ')')
        ExceptionHelper.Throw((Error) new SyntaxError("Expected \")\" at + " + CodeCoordinates.FromTextPosition(state.Code, index1, 0)?.ToString()));
      ++index1;
      int num = index;
      index = index1;
      DoWhile doWhile = new DoWhile();
      doWhile.allowRemove = continiesCount == state.ContiniesCount && breaksCount == state.BreaksCount;
      doWhile.body = codeNode1;
      doWhile.condition = codeNode2;
      doWhile.labels = state.Labels.GetRange(state.Labels.Count - labelsCount, labelsCount).ToArray();
      doWhile.Position = num;
      doWhile.Length = index - num;
      return (CodeNode) doWhile;
    }

    public override JSValue Evaluate(Context context)
    {
      JSValue jsValue;
      do
      {
        if (context._executionMode != ExecutionMode.Resume || !context.SuspendData.ContainsKey((CodeNode) this))
        {
          if (context._debugging && !(this.body is CodeBlock))
            context.raiseDebugger(this.body);
          context._lastResult = this.body.Evaluate(context) ?? context._lastResult;
          if (context._executionMode != ExecutionMode.Regular)
          {
            if (context._executionMode >= ExecutionMode.Return)
              return (JSValue) null;
            bool flag1 = context._executionInfo == null || System.Array.IndexOf<string>(this.labels, context._executionInfo._oValue as string) != -1;
            bool flag2 = context._executionMode > ExecutionMode.Continue || !flag1;
            if (flag1)
            {
              context._executionMode = ExecutionMode.Regular;
              context._executionInfo = JSValue.notExists;
            }
            if (flag2)
              return (JSValue) null;
          }
        }
        if (context._debugging)
          context.raiseDebugger(this.condition);
        jsValue = this.condition.Evaluate(context);
        if (context._executionMode == ExecutionMode.Suspend)
        {
          context.SuspendData[(CodeNode) this] = (object) null;
          return (JSValue) null;
        }
      }
      while ((bool) jsValue);
      return (JSValue) null;
    }

    protected internal override CodeNode[] GetChildrenImpl()
    {
      List<CodeNode> codeNodeList = new List<CodeNode>();
      codeNodeList.Add(this.body);
      codeNodeList.Add(this.condition);
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
      expressionDepth = System.Math.Max(1, expressionDepth);
      Parser.Build<CodeNode>(ref this.body, expressionDepth, variables, codeContext | CodeContext.InLoop, message, stats, opts);
      Parser.Build<CodeNode>(ref this.condition, 2, variables, codeContext | CodeContext.InLoop | CodeContext.InExpression, message, stats, opts);
      try
      {
        if (this.allowRemove)
        {
          if ((opts & Options.SuppressUselessStatementsElimination) == Options.None)
          {
            if (!(this.condition is Constant))
            {
              if (!(this.condition as Expression).ContextIndependent)
                goto label_11;
            }
            if ((bool) this.condition.Evaluate((Context) null))
              _this = (CodeNode) new InfinityLoop(this.body, this.labels);
            else if (this.labels.Length == 0)
              _this = this.body;
            this.condition.Eliminated = true;
          }
        }
      }
      catch
      {
      }
label_11:
      if (_this == this && this.body == null)
        this.body = (CodeNode) new Empty();
      return false;
    }

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      this.condition.Optimize(ref this.condition, owner, message, opts, stats);
      this.body.Optimize(ref this.body, owner, message, opts, stats);
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override void Decompose(ref CodeNode self)
    {
      if (this.condition != null)
        this.condition.Decompose(ref this.condition);
      if (this.body == null)
        return;
      this.body.Decompose(ref this.body);
    }

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
      this.condition?.RebuildScope(functionInfo, transferedVariables, scopeBias);
      this.body?.RebuildScope(functionInfo, transferedVariables, scopeBias);
    }

    public override string ToString()
    {
      string[] strArray = new string[5]
      {
        "do",
        null,
        null,
        null,
        null
      };
      string str;
      if (!(this.body is CodeBlock))
        str = Environment.NewLine + "  " + this.body?.ToString() + ";" + Environment.NewLine;
      else
        str = this.body?.ToString() + " ";
      strArray[1] = str;
      strArray[2] = "while (";
      strArray[3] = this.condition?.ToString();
      strArray[4] = ")";
      return string.Concat(strArray);
    }
  }
}
