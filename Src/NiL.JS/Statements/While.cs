// Decompiled with JetBrains decompiler
// Type: NiL.JS.Statements.While
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
  public sealed class While : CodeNode
  {
    private bool allowRemove;
    private CodeNode condition;
    private CodeNode body;
    private string[] labels;

    public CodeNode Condition => this.condition;

    public CodeNode Body => this.body;

    public ICollection<string> Labels => (ICollection<string>) new ReadOnlyCollection<string>((IList<string>) this.labels);

    internal static CodeNode Parse(ParseInfo state, ref int index)
    {
      int index1 = index;
      if (!Parser.Validate(state.Code, "while (", ref index1) && !Parser.Validate(state.Code, "while(", ref index1))
        return (CodeNode) null;
      int labelsCount = state.LabelsCount;
      state.LabelsCount = 0;
      while (Tools.IsWhiteSpace(state.Code[index1]))
        ++index1;
      CodeNode codeNode1 = Parser.Parse(state, ref index1, CodeFragmentType.Expression);
      while (index1 < state.Code.Length && Tools.IsWhiteSpace(state.Code[index1]))
        ++index1;
      if (index1 >= state.Code.Length)
        ExceptionHelper.Throw((Error) new SyntaxError(Strings.UnexpectedEndOfSource));
      if (state.Code[index1] != ')')
        throw new ArgumentException("code (" + index1.ToString() + ")");
      do
      {
        ++index1;
      }
      while (index1 < state.Code.Length && Tools.IsWhiteSpace(state.Code[index1]));
      if (index1 >= state.Code.Length)
        ExceptionHelper.ThrowSyntaxError(Strings.UnexpectedEndOfSource);
      state.AllowBreak.Push(true);
      state.AllowContinue.Push(true);
      int continiesCount = state.ContiniesCount;
      int breaksCount = state.BreaksCount;
      CodeNode codeNode2 = Parser.Parse(state, ref index1, CodeFragmentType.Statement);
      if (codeNode2 is FunctionDefinition)
      {
        if (state.Message != null)
          state.Message(MessageLevel.CriticalWarning, codeNode2.Position, codeNode2.Length, Strings.DoNotDeclareFunctionInNestedBlocks);
        codeNode2 = (CodeNode) new CodeBlock(new CodeNode[1]
        {
          codeNode2
        });
      }
      state.AllowBreak.Pop();
      state.AllowContinue.Pop();
      int num = index;
      index = index1;
      While @while = new While();
      @while.allowRemove = continiesCount == state.ContiniesCount && breaksCount == state.BreaksCount;
      @while.body = codeNode2;
      @while.condition = codeNode1;
      @while.labels = state.Labels.GetRange(state.Labels.Count - labelsCount, labelsCount).ToArray();
      @while.Position = num;
      @while.Length = index - num;
      return (CodeNode) @while;
    }

    public override JSValue Evaluate(Context context)
    {
      bool flag1 = this.body != null;
      if (context._executionMode != ExecutionMode.Resume || context.SuspendData[(CodeNode) this] == this.condition)
      {
        if (context._executionMode != ExecutionMode.Resume && context._debugging)
          context.raiseDebugger(this.condition);
        JSValue jsValue = this.condition.Evaluate(context);
        if (context._executionMode == ExecutionMode.Suspend)
        {
          context.SuspendData[(CodeNode) this] = (object) this.condition;
          return (JSValue) null;
        }
        if (!(bool) jsValue)
          return (JSValue) null;
      }
      JSValue jsValue1;
      do
      {
        if (flag1 && (context._executionMode != ExecutionMode.Resume || context.SuspendData[(CodeNode) this] == this.body))
        {
          if (context._executionMode != ExecutionMode.Resume && context._debugging && !(this.body is CodeBlock))
            context.raiseDebugger(this.body);
          JSValue jsValue2 = this.body.Evaluate(context);
          if (jsValue2 != null)
            context._lastResult = jsValue2;
          if (context._executionMode != ExecutionMode.Regular)
          {
            if (context._executionMode < ExecutionMode.Return)
            {
              bool flag2 = context._executionInfo == null || System.Array.IndexOf<string>(this.labels, context._executionInfo._oValue as string) != -1;
              bool flag3 = context._executionMode > ExecutionMode.Continue || !flag2;
              if (flag2)
              {
                context._executionMode = ExecutionMode.Regular;
                context._executionInfo = (JSValue) null;
              }
              if (flag3)
                return (JSValue) null;
            }
            else
            {
              if (context._executionMode != ExecutionMode.Suspend)
                return (JSValue) null;
              context.SuspendData[(CodeNode) this] = (object) this.body;
              return (JSValue) null;
            }
          }
        }
        if (context._executionMode != ExecutionMode.Resume && context._debugging)
          context.raiseDebugger(this.condition);
        jsValue1 = this.condition.Evaluate(context);
        if (context._executionMode == ExecutionMode.Suspend)
        {
          context.SuspendData[(CodeNode) this] = (object) this.condition;
          return (JSValue) null;
        }
      }
      while ((bool) jsValue1);
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
      Parser.Build<CodeNode>(ref this.body, expressionDepth, variables, codeContext | CodeContext.Conditional | CodeContext.InLoop, message, stats, opts);
      Parser.Build<CodeNode>(ref this.condition, 2, variables, codeContext | CodeContext.InLoop | CodeContext.InExpression, message, stats, opts);
      if ((opts & Options.SuppressUselessStatementsElimination) == Options.None && this.condition is ConvertToBoolean)
      {
        if (message != null)
          message(MessageLevel.Warning, this.condition.Position, 2, "Useless conversion. Remove double negation in condition");
        this.condition = (CodeNode) (this.condition as Expression)._left;
      }
      try
      {
        if (this.allowRemove && (this.condition is Constant || this.condition is Expression && (this.condition as Expression).ContextIndependent))
        {
          this.Eliminated = true;
          if ((bool) this.condition.Evaluate((Context) null))
          {
            if ((opts & Options.SuppressUselessStatementsElimination) == Options.None && this.body != null)
              _this = (CodeNode) new InfinityLoop(this.body, this.labels);
          }
          else if ((opts & Options.SuppressUselessStatementsElimination) == Options.None)
          {
            _this = (CodeNode) null;
            if (this.body != null)
              this.body.Eliminated = true;
          }
          this.condition.Eliminated = true;
        }
        else if ((opts & Options.SuppressUselessStatementsElimination) == Options.None)
        {
          if (!(this.condition is ObjectDefinition) || (this.condition as ObjectDefinition).FieldNames.Length != 0)
          {
            if (this.condition is ArrayDefinition)
            {
              if ((this.condition as ArrayDefinition).Elements.Length != 0)
                goto label_19;
            }
            else
              goto label_19;
          }
          _this = (CodeNode) new InfinityLoop(this.body, this.labels);
          this.condition.Eliminated = true;
        }
      }
      catch
      {
      }
label_19:
      return false;
    }

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      if (this.condition != null)
        this.condition.Optimize(ref this.condition, owner, message, opts, stats);
      if (this.body == null)
        return;
      this.body.Optimize(ref this.body, owner, message, opts, stats);
    }

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

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => "while (" + this.condition?.ToString() + ")" + (this.body is CodeBlock ? "" : Environment.NewLine + "  ") + this.body?.ToString();
  }
}
