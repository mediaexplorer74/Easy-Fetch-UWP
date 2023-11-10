// Decompiled with JetBrains decompiler
// Type: NiL.JS.Statements.With
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
  public sealed class With : CodeNode
  {
    private CodeNode _scope;
    private CodeNode _body;

    public CodeNode Body => this._body;

    public CodeNode Scope => this._scope;

    internal static CodeNode Parse(ParseInfo state, ref int index)
    {
      int index1 = index;
      if (!Parser.Validate(state.Code, "with (", ref index1) && !Parser.Validate(state.Code, "with(", ref index1))
        return (CodeNode) null;
      if (state.Strict)
        ExceptionHelper.Throw((Error) new SyntaxError("WithStatement is not allowed in strict mode."));
      if (state.Message != null)
        state.Message(MessageLevel.CriticalWarning, index, 4, "Do not use \"with\".");
      CodeNode codeNode1 = Parser.Parse(state, ref index1, CodeFragmentType.Expression);
      while (Tools.IsWhiteSpace(state.Code[index1]))
        ++index1;
      if (state.Code[index1] != ')')
        ExceptionHelper.Throw((Error) new SyntaxError("Invalid syntax WithStatement."));
      do
      {
        ++index1;
      }
      while (Tools.IsWhiteSpace(state.Code[index1]));
      CodeNode codeNode2 = (CodeNode) null;
      int count = state.Variables.Count;
      ++state.LexicalScopeLevel;
      using (state.WithCodeContext(CodeContext.InWith))
      {
        try
        {
          codeNode2 = Parser.Parse(state, ref index1, CodeFragmentType.Statement);
          VariableDescriptor[] variables = CodeBlock.extractVariables(state, count);
          CodeBlock codeBlock = new CodeBlock(new CodeNode[1]
          {
            codeNode2
          });
          codeBlock._variables = variables;
          codeBlock.Position = codeNode2.Position;
          codeBlock.Length = codeNode2.Length;
          codeNode2 = (CodeNode) codeBlock;
        }
        finally
        {
          --state.LexicalScopeLevel;
        }
      }
      int num = index;
      index = index1;
      With with = new With();
      with._scope = codeNode1;
      with._body = codeNode2;
      with.Position = num;
      with.Length = index - num;
      return (CodeNode) with;
    }

    public override JSValue Evaluate(Context context)
    {
      WithContext intcontext = (WithContext) null;
      Action<Context> action = (Action<Context>) null;
      if (context._executionMode >= ExecutionMode.Resume)
      {
        action = context.SuspendData[(CodeNode) this] as Action<Context>;
        if (action != null)
        {
          action(context);
          return (JSValue) null;
        }
      }
      if (context._executionMode != ExecutionMode.Resume && context._debugging)
        context.raiseDebugger(this._scope);
      JSValue jsValue = this._scope.Evaluate(context);
      if (context._executionMode == ExecutionMode.Suspend)
      {
        context.SuspendData[(CodeNode) this] = (object) null;
        return (JSValue) null;
      }
      intcontext = new WithContext(jsValue, context);
      action = (Action<Context>) (c =>
      {
        try
        {
          intcontext._executionMode = c._executionMode;
          intcontext._executionInfo = c._executionInfo;
          intcontext.Activate();
          c._lastResult = this._body.Evaluate((Context) intcontext) ?? intcontext._lastResult;
          c._executionMode = intcontext._executionMode;
          c._executionInfo = intcontext._executionInfo;
          if (c._executionMode != ExecutionMode.Suspend)
            return;
          c.SuspendData[(CodeNode) this] = (object) action;
        }
        finally
        {
          intcontext.Deactivate();
        }
      });
      if (context._debugging && !(this._body is CodeBlock))
        context.raiseDebugger(this._body);
      action(context);
      return (JSValue) null;
    }

    protected internal override CodeNode[] GetChildrenImpl()
    {
      List<CodeNode> codeNodeList = new List<CodeNode>();
      codeNodeList.Add(this._body);
      codeNodeList.Add(this._scope);
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
      if (stats != null)
        stats.ContainsWith = true;
      Parser.Build<CodeNode>(ref this._scope, expressionDepth + 1, variables, codeContext | CodeContext.InExpression, message, stats, opts);
      Parser.Build<CodeNode>(ref this._body, expressionDepth, new Dictionary<string, VariableDescriptor>(), codeContext | CodeContext.InWith, message, stats, opts);
      return false;
    }

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      if (this._scope != null)
        this._scope.Optimize(ref this._scope, owner, message, opts, stats);
      if (this._body != null)
        this._body.Optimize(ref this._body, owner, message, opts, stats);
      if (this._body != null)
        return;
      _this = this._scope;
    }

    public override void Decompose(ref CodeNode self)
    {
      if (this._scope != null)
        this._scope.Decompose(ref this._scope);
      if (this._body == null)
        return;
      this._body.Decompose(ref this._body);
    }

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
      this._scope?.RebuildScope(functionInfo, transferedVariables, scopeBias);
      Dictionary<string, VariableDescriptor> transferedVariables1 = new Dictionary<string, VariableDescriptor>();
      this._body?.RebuildScope(functionInfo, transferedVariables1, scopeBias + 1);
      if (transferedVariables1 == null || !(this._body is CodeBlock body))
        return;
      List<VariableDescriptor> variableDescriptorList = new List<VariableDescriptor>();
      foreach (KeyValuePair<string, VariableDescriptor> keyValuePair in transferedVariables1)
      {
        if (keyValuePair.Value is ParameterDescriptor || !(keyValuePair.Value.initializer is FunctionDefinition))
          transferedVariables.Add(keyValuePair.Key, keyValuePair.Value);
        else
          variableDescriptorList.Add(keyValuePair.Value);
      }
      body._variables = variableDescriptorList.ToArray();
      body._suppressScopeIsolation = body._variables.Length == 0 ? SuppressScopeIsolationMode.Suppress : SuppressScopeIsolationMode.DoNotSuppress;
    }

    public override string ToString() => "with (" + this._scope?.ToString() + ")" + (this._body is CodeBlock ? "" : Environment.NewLine + "  ") + this._body?.ToString();

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);
  }
}
