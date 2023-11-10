// Decompiled with JetBrains decompiler
// Type: NiL.JS.Statements.For
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using NiL.JS.Expressions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NiL.JS.Statements
{
  public sealed class For : CodeNode
  {
    private CodeNode _initializer;
    private CodeNode _condition;
    private CodeNode _post;
    private CodeNode _body;
    private string[] labels;

    public CodeNode Initializer => this._initializer;

    public CodeNode Condition => this._condition;

    public CodeNode Post => this._post;

    public CodeNode Body => this._body;

    public ICollection<string> Labels => (ICollection<string>) new ReadOnlyCollection<string>((IList<string>) this.labels);

    private For()
    {
    }

    internal static CodeNode Parse(ParseInfo state, ref int index)
    {
      int num1 = index;
      while (Tools.IsWhiteSpace(state.Code[num1]))
        ++num1;
      if (!Parser.Validate(state.Code, "for(", ref num1) && !Parser.Validate(state.Code, "for (", ref num1))
        return (CodeNode) null;
      while (Tools.IsWhiteSpace(state.Code[num1]))
        ++num1;
      CodeNode codeNode1 = (CodeNode) null;
      int labelsCount = state.LabelsCount;
      int count = state.Variables.Count;
      state.LabelsCount = 0;
      ++state.LexicalScopeLevel;
      try
      {
        CodeNode codeNode2 = VariableDefinition.Parse(state, ref num1, true) ?? (CodeNode) ExpressionTree.Parse(state, ref num1, false, true, false, true, true);
        if (codeNode2 is ExpressionTree && (codeNode2 as ExpressionTree).Type == OperationType.None && (codeNode2 as ExpressionTree)._right == null)
          codeNode2 = (CodeNode) (codeNode2 as ExpressionTree)._left;
        if (state.Code[num1] != ';')
          ExceptionHelper.Throw((Error) new SyntaxError("Expected \";\" at + " + CodeCoordinates.FromTextPosition(state.Code, num1, 0)?.ToString()));
        do
        {
          ++num1;
        }
        while (Tools.IsWhiteSpace(state.Code[num1]));
        CodeNode codeNode3 = state.Code[num1] == ';' ? (CodeNode) null : (CodeNode) ExpressionTree.Parse(state, ref num1, false, true, false, true, true);
        if (state.Code[num1] != ';')
          ExceptionHelper.Throw((Error) new SyntaxError("Expected \";\" at + " + CodeCoordinates.FromTextPosition(state.Code, num1, 0)?.ToString()));
        do
        {
          ++num1;
        }
        while (Tools.IsWhiteSpace(state.Code[num1]));
        CodeNode codeNode4 = state.Code[num1] == ')' ? (CodeNode) null : (CodeNode) ExpressionTree.Parse(state, ref num1, false, true, false, true, true);
        while (Tools.IsWhiteSpace(state.Code[num1]))
          ++num1;
        if (state.Code[num1] != ')')
          ExceptionHelper.Throw((Error) new SyntaxError("Expected \";\" at + " + CodeCoordinates.FromTextPosition(state.Code, num1, 0)?.ToString()));
        ++num1;
        Tools.SkipSpaces(state.Code, ref num1);
        state.AllowBreak.Push(true);
        state.AllowContinue.Push(true);
        try
        {
          codeNode1 = Parser.Parse(state, ref num1, CodeFragmentType.Statement);
          if (codeNode1 is VariableDefinition variableDefinition)
          {
            if (variableDefinition.Kind >= VariableKind.ConstantInLexicalScope)
              ExceptionHelper.ThrowSyntaxError("Block scope variables can not be declared in for-loop directly", state.Code, codeNode1.Position);
            if (state.Message != null)
              state.Message(MessageLevel.Warning, codeNode1.Position, codeNode1.Length, "Do not declare variables in for-loop directly");
          }
        }
        finally
        {
          state.AllowBreak.Pop();
          state.AllowContinue.Pop();
        }
        int num2 = index;
        index = num1;
        For @for = new For();
        @for._body = codeNode1;
        @for._condition = codeNode3;
        @for._initializer = codeNode2;
        @for._post = codeNode4;
        @for.labels = state.Labels.GetRange(state.Labels.Count - labelsCount, labelsCount).ToArray();
        @for.Position = num2;
        @for.Length = index - num2;
        CodeNode codeNode5 = (CodeNode) @for;
        VariableDescriptor[] variables = CodeBlock.extractVariables(state, count);
        CodeBlock codeBlock = new CodeBlock(new CodeNode[1]
        {
          codeNode5
        });
        codeBlock._variables = variables;
        codeBlock.Position = codeNode5.Position;
        codeBlock.Length = codeNode5.Length;
        return (CodeNode) codeBlock;
      }
      finally
      {
        --state.LexicalScopeLevel;
      }
    }

    public override JSValue Evaluate(Context context)
    {
      if (this._initializer != null && (context._executionMode != ExecutionMode.Resume || context.SuspendData[(CodeNode) this] == this._initializer))
      {
        if (context._executionMode != ExecutionMode.Resume && context._debugging)
          context.raiseDebugger(this._initializer);
        this._initializer.Evaluate(context);
        if (context._executionMode == ExecutionMode.Suspend)
        {
          context.SuspendData[(CodeNode) this] = (object) this._initializer;
          return (JSValue) null;
        }
      }
      bool flag1 = this._body != null;
      bool flag2 = this._post != null;
      if (context._executionMode != ExecutionMode.Resume || context.SuspendData[(CodeNode) this] == this._condition)
      {
        if (context._executionMode != ExecutionMode.Resume && context._debugging)
          context.raiseDebugger(this._condition);
        bool flag3 = (bool) this._condition.Evaluate(context);
        if (context._executionMode == ExecutionMode.Suspend)
        {
          context.SuspendData[(CodeNode) this] = (object) this._condition;
          return (JSValue) null;
        }
        if (!flag3)
          return (JSValue) null;
      }
      bool flag4;
      do
      {
        if (flag1 && (context._executionMode != ExecutionMode.Resume || context.SuspendData[(CodeNode) this] == this._body))
        {
          if (context._executionMode != ExecutionMode.Resume && context._debugging && !(this._body is CodeBlock))
            context.raiseDebugger(this._body);
          JSValue jsValue = this._body.Evaluate(context);
          if (jsValue != null)
            context._lastResult = jsValue;
          if (context._executionMode != ExecutionMode.Regular)
          {
            if (context._executionMode < ExecutionMode.Return)
            {
              bool flag5 = context._executionInfo == null || System.Array.IndexOf<string>(this.labels, context._executionInfo._oValue as string) != -1;
              bool flag6 = context._executionMode > ExecutionMode.Continue || !flag5;
              if (flag5)
              {
                context._executionMode = ExecutionMode.Regular;
                context._executionInfo = (JSValue) null;
              }
              if (flag6)
                return (JSValue) null;
            }
            else
            {
              if (context._executionMode != ExecutionMode.Suspend)
                return (JSValue) null;
              context.SuspendData[(CodeNode) this] = (object) this._body;
              return (JSValue) null;
            }
          }
        }
        if (flag2 && (context._executionMode != ExecutionMode.Resume || context.SuspendData[(CodeNode) this] == this._post))
        {
          if (context._executionMode != ExecutionMode.Resume && context._debugging)
            context.raiseDebugger(this._post);
          this._post.Evaluate(context);
        }
        if (context._executionMode != ExecutionMode.Resume && context._debugging)
          context.raiseDebugger(this._condition);
        flag4 = (bool) this._condition.Evaluate(context);
        if (context._executionMode == ExecutionMode.Suspend)
        {
          context.SuspendData[(CodeNode) this] = (object) this._condition;
          return (JSValue) null;
        }
      }
      while (flag4);
      return (JSValue) null;
    }

    protected internal override CodeNode[] GetChildrenImpl()
    {
      List<CodeNode> codeNodeList = new List<CodeNode>();
      codeNodeList.Add(this._initializer);
      codeNodeList.Add(this._condition);
      codeNodeList.Add(this._post);
      codeNodeList.Add(this._body);
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
      Parser.Build<CodeNode>(ref this._initializer, 1, variables, codeContext, message, stats, opts);
      VariableDefinition initializer = this._initializer as VariableDefinition;
      if ((opts & Options.SuppressUselessStatementsElimination) == Options.None && initializer != null && initializer._initializers.Length == 1 && initializer.Kind == VariableKind.FunctionScope)
        this._initializer = (CodeNode) initializer._initializers[0];
      Parser.Build<CodeNode>(ref this._condition, 2, variables, codeContext | CodeContext.InLoop | CodeContext.InExpression, message, stats, opts);
      if (this._post != null)
      {
        Parser.Build<CodeNode>(ref this._post, 1, variables, codeContext | CodeContext.Conditional | CodeContext.InLoop | CodeContext.InExpression, message, stats, opts);
        if (this._post == null && message != null)
          message(MessageLevel.Warning, this.Position, this.Length, "Last expression of for-loop was removed. Maybe, it's a mistake.");
      }
      Parser.Build<CodeNode>(ref this._body, System.Math.Max(1, expressionDepth), variables, codeContext | CodeContext.Conditional | CodeContext.InLoop, message, stats, opts);
      if (initializer != null && initializer.Kind != VariableKind.FunctionScope && ((IEnumerable<VariableDescriptor>) initializer._variables).Any<VariableDescriptor>((Func<VariableDescriptor, bool>) (x => x.captured)))
      {
        if (this._body is CodeBlock codeBlock)
        {
          CodeNode[] destinationArray = new CodeNode[codeBlock._lines.Length + 1];
          System.Array.Copy((System.Array) codeBlock._lines, (System.Array) destinationArray, codeBlock._lines.Length);
          destinationArray[destinationArray.Length - 1] = (CodeNode) new For.PerIterationScopeInitializer(initializer._variables);
          codeBlock._lines = destinationArray;
        }
        else
        {
          CodeNode[] body = new CodeNode[2]
          {
            this._body,
            (CodeNode) new For.PerIterationScopeInitializer(initializer._variables)
          };
          this._body = (CodeNode) (codeBlock = new CodeBlock(body));
        }
        codeBlock._suppressScopeIsolation = SuppressScopeIsolationMode.DoNotSuppress;
        for (int index = 0; index < initializer._variables.Length; ++index)
        {
          if (initializer._variables[index].captured)
            initializer._variables[index].definitionScopeLevel = -1;
        }
      }
      if (this._condition == null)
      {
        this._condition = (CodeNode) new Constant((JSValue) NiL.JS.BaseLibrary.Boolean.True);
      }
      else
      {
        if ((opts & Options.SuppressUselessStatementsElimination) == Options.None && this._condition is Expression && (this._condition as Expression).ContextIndependent && !(bool) this._condition.Evaluate((Context) null))
        {
          _this = this._initializer;
          return false;
        }
        if (this._body == null || this._body is Empty)
        {
          VariableReference left = (VariableReference) null;
          Constant right = (Constant) null;
          if (this._condition is Less)
          {
            left = (this._condition as Less).LeftOperand as VariableReference;
            right = (this._condition as Less).RightOperand as Constant;
          }
          else if (this._condition is More)
          {
            left = (this._condition as More).RightOperand as VariableReference;
            right = (this._condition as More).LeftOperand as Constant;
          }
          else if (this._condition is NotEqual)
          {
            left = (this._condition as Less).RightOperand as VariableReference;
            right = (this._condition as Less).LeftOperand as Constant;
            if (left == null && right == null)
            {
              left = (this._condition as Less).LeftOperand as VariableReference;
              right = (this._condition as Less).RightOperand as Constant;
            }
          }
          if (left != null && right != null && this._post is Increment && ((this._post as Increment).LeftOperand as VariableReference)._descriptor == left._descriptor && left.ScopeLevel >= 0 && left._descriptor.definitionScopeLevel >= 0 && this._initializer is Assignment && (this._initializer as Assignment).LeftOperand is Variable && ((this._initializer as Assignment).LeftOperand as Variable)._descriptor == left._descriptor)
          {
            Expression rightOperand = (this._initializer as Assignment).RightOperand;
            if (rightOperand is Constant)
            {
              JSValue first = rightOperand.Evaluate((Context) null);
              JSValue second = right.Evaluate((Context) null);
              if ((first._valueType == JSValueType.Integer || first._valueType == JSValueType.Boolean || first._valueType == JSValueType.Double) && (second._valueType == JSValueType.Integer || second._valueType == JSValueType.Boolean || second._valueType == JSValueType.Double))
              {
                this._post.Eliminated = true;
                this._condition.Eliminated = true;
                if ((opts & Options.SuppressUselessStatementsElimination) == Options.None)
                {
                  if (!Less.Check(first, second))
                  {
                    _this = this._initializer;
                    return false;
                  }
                  _this = (CodeNode) new CodeBlock(new CodeNode[2]
                  {
                    this._initializer,
                    (CodeNode) new Assignment((Expression) left, (Expression) right)
                  });
                  return true;
                }
              }
            }
          }
        }
      }
      return false;
    }

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      if (this._initializer != null)
        this._initializer.Optimize(ref this._initializer, owner, message, opts, stats);
      if (this._condition != null)
        this._condition.Optimize(ref this._condition, owner, message, opts, stats);
      if (this._post != null)
        this._post.Optimize(ref this._post, owner, message, opts, stats);
      if (this._body == null)
        return;
      this._body.Optimize(ref this._body, owner, message, opts, stats);
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override void Decompose(ref CodeNode self)
    {
      this._initializer?.Decompose(ref this._initializer);
      this._condition?.Decompose(ref this._condition);
      this._body?.Decompose(ref this._body);
      this._post?.Decompose(ref this._post);
    }

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
      this._initializer?.RebuildScope(functionInfo, transferedVariables, scopeBias);
      this._condition?.RebuildScope(functionInfo, transferedVariables, scopeBias);
      this._body?.RebuildScope(functionInfo, transferedVariables, scopeBias);
      this._post?.RebuildScope(functionInfo, transferedVariables, scopeBias);
    }

    public override string ToString() => "for (" + ((object) this._initializer ?? (object) "").ToString() + "; " + this._condition?.ToString() + "; " + this._post?.ToString() + ")" + (this._body is CodeBlock ? "" : Environment.NewLine + "  ") + this._body?.ToString();

    private sealed class PerIterationScopeInitializer : CodeNode
    {
      private VariableDescriptor[] _variables;

      public PerIterationScopeInitializer(VariableDescriptor[] variables) => this._variables = variables;

      public override void Decompose(ref CodeNode self)
      {
      }

      public override JSValue Evaluate(Context context)
      {
        if (this._variables != null)
        {
          for (int index = 0; index < this._variables.Length; ++index)
          {
            if (this._variables[index].captured)
              context.DefineVariable(this._variables[index].name).Assign(this._variables[index].cacheRes.CloneImpl());
          }
        }
        return (JSValue) null;
      }

      public override void RebuildScope(
        FunctionInfo functionInfo,
        Dictionary<string, VariableDescriptor> transferedVariables,
        int scopeBias)
      {
      }
    }
  }
}
