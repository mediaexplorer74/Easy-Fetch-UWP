// Decompiled with JetBrains decompiler
// Type: NiL.JS.Statements.ForOf
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using NiL.JS.Expressions;
using NiL.JS.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NiL.JS.Statements
{
  public sealed class ForOf : CodeNode
  {
    private CodeNode _variable;
    private CodeNode _source;
    private CodeNode _body;
    private string[] _labels;

    public CodeNode Variable => this._variable;

    public CodeNode Source => this._source;

    public CodeNode Body => this._body;

    public ReadOnlyCollection<string> Labels => new ReadOnlyCollection<string>((IList<string>) this._labels);

    private ForOf()
    {
    }

    internal static CodeNode Parse(ParseInfo state, ref int index)
    {
      int num = index;
      Tools.SkipSpaces(state.Code, ref num);
      if (!Parser.Validate(state.Code, "for(", ref num) && !Parser.Validate(state.Code, "for (", ref num))
        return (CodeNode) null;
      Tools.SkipSpaces(state.Code, ref num);
      ForOf forOf = new ForOf()
      {
        _labels = state.Labels.GetRange(state.Labels.Count - state.LabelsCount, state.LabelsCount).ToArray()
      };
      VariableDescriptor[] variableDescriptorArray = (VariableDescriptor[]) null;
      int count = state.Variables.Count;
      ++state.LexicalScopeLevel;
      try
      {
        CodeNode source = VariableDefinition.Parse(state, ref num, true);
        string name;
        if (source == null)
        {
          if (state.Code[num] == ';')
            return (CodeNode) null;
          Tools.SkipSpaces(state.Code, ref num);
          int startIndex = num;
          if (!Parser.ValidateName(state.Code, ref num, state.Strict))
            return (CodeNode) null;
          name = Tools.Unescape(state.Code.Substring(startIndex, num - startIndex), state.Strict);
          NiL.JS.Expressions.Variable variable = new NiL.JS.Expressions.Variable(name, state.LexicalScopeLevel);
          variable.Position = startIndex;
          variable.Length = num - startIndex;
          variable.ScopeLevel = state.LexicalScopeLevel;
          source = (CodeNode) variable;
          Tools.SkipSpaces(state.Code, ref num);
          if (state.Code[num] == '=')
          {
            Tools.SkipSpaces(state.Code, ref num);
            Expression right = ExpressionTree.Parse(state, ref num, processComma: false, forForLoop: true);
            if (right == null)
              return (CodeNode) right;
            Expression left = (Expression) new AssignmentOperatorCache((Expression) (source as NiL.JS.Expressions.Variable));
            Assignment assignment = new Assignment(left, right);
            assignment.Position = left.Position;
            assignment.Length = right.EndPosition - left.Position;
            Expression expression = (Expression) assignment;
            if (source == expression._left._left)
              source = (CodeNode) expression;
            Tools.SkipSpaces(state.Code, ref num);
          }
        }
        else
          name = (source as VariableDefinition)._variables[0].name;
        if (!Parser.Validate(state.Code, "of", ref num))
        {
          if (count < state.Variables.Count)
            state.Variables.RemoveRange(count, state.Variables.Count - count);
          return (CodeNode) null;
        }
        if (state.Strict && (name == "arguments" || name == "eval"))
          ExceptionHelper.ThrowSyntaxError("Parameters name may not be \"arguments\" or \"eval\" in strict mode at ", state.Code, source.Position, source.Length);
        if (source is VariableDefinition && (source as VariableDefinition)._variables.Length > 1)
          ExceptionHelper.ThrowSyntaxError("Too many variables in for-of loop", state.Code, num);
        forOf._variable = source;
        state.LabelsCount = 0;
        Tools.SkipSpaces(state.Code, ref num);
        forOf._source = Parser.Parse(state, ref num, CodeFragmentType.Expression);
        Tools.SkipSpaces(state.Code, ref num);
        if (state.Code[num] != ')')
          ExceptionHelper.Throw((Error) new SyntaxError("Expected \")\" at + " + CodeCoordinates.FromTextPosition(state.Code, num, 0)?.ToString()));
        ++num;
        state.AllowBreak.Push(true);
        state.AllowContinue.Push(true);
        forOf._body = Parser.Parse(state, ref num, CodeFragmentType.Statement);
        state.AllowBreak.Pop();
        state.AllowContinue.Pop();
        forOf.Position = index;
        forOf.Length = num - index;
        index = num;
        variableDescriptorArray = CodeBlock.extractVariables(state, count);
      }
      finally
      {
        --state.LexicalScopeLevel;
      }
      CodeBlock codeBlock = new CodeBlock((CodeNode[]) new ForOf[1]
      {
        forOf
      });
      codeBlock._variables = variableDescriptorArray;
      codeBlock.Position = forOf.Position;
      codeBlock.Length = forOf.Length;
      return (CodeNode) codeBlock;
    }

    public override JSValue Evaluate(Context context)
    {
      ForOf.SuspendData suspendData = (ForOf.SuspendData) null;
      if (context._executionMode >= ExecutionMode.Resume)
        suspendData = context.SuspendData[(CodeNode) this] as ForOf.SuspendData;
      JSValue source;
      if (suspendData == null || suspendData.source == null)
      {
        if (context._debugging && !(this._source is CodeBlock))
          context.raiseDebugger(this._source);
        source = this._source.Evaluate(context);
        if (context._executionMode == ExecutionMode.Suspend)
        {
          context.SuspendData[(CodeNode) this] = (object) null;
          return (JSValue) null;
        }
      }
      else
        source = suspendData.source;
      JSValue jsValue;
      if (suspendData == null || suspendData.variable == null)
      {
        if (context._debugging && !(this._variable is CodeBlock))
          context.raiseDebugger(this._variable);
        if (this._variable is VariableDefinition variable)
        {
          this._variable.Evaluate(context);
          jsValue = (variable._initializers[0]._left ?? variable._initializers[0]).EvaluateForWrite(context);
        }
        else if (this._variable is Assignment)
        {
          this._variable.Evaluate(context);
          jsValue = (this._variable as Assignment)._left.Evaluate(context);
        }
        else
          jsValue = this._variable.EvaluateForWrite(context);
        if (context._executionMode == ExecutionMode.Suspend)
        {
          if (suspendData == null)
            suspendData = new ForOf.SuspendData();
          context.SuspendData[(CodeNode) this] = (object) suspendData;
          suspendData.source = source;
          return (JSValue) null;
        }
      }
      else
        jsValue = suspendData.variable;
      if (!source.Defined || source.IsNull || this._body == null)
        return (JSValue) null;
      IIterator iterator = suspendData?.iterator ?? IterationProtocolExtensions.AsIterable(source).iterator();
      if (iterator == null)
        return (JSValue) null;
      for (IIteratorResult iteratorResult = context._executionMode != ExecutionMode.Resume ? iterator.next() : (IIteratorResult) null; context._executionMode >= ExecutionMode.Resume || !iteratorResult.done; iteratorResult = iterator.next())
      {
        if (context._executionMode != ExecutionMode.Resume)
        {
          JSValueAttributesInternal attributes = jsValue._attributes;
          jsValue._attributes &= ~JSValueAttributesInternal.ReadOnly;
          jsValue.Assign(iteratorResult.value);
          jsValue._attributes = attributes;
        }
        this._body.Evaluate(context);
        if (context._executionMode != ExecutionMode.Regular)
        {
          if (context._executionMode < ExecutionMode.Return)
          {
            bool flag1 = context._executionInfo == null || System.Array.IndexOf<string>(this._labels, context._executionInfo._oValue as string) != -1;
            bool flag2 = context._executionMode > ExecutionMode.Continue || !flag1;
            if (flag1)
            {
              context._executionMode = ExecutionMode.Regular;
              context._executionInfo = JSValue.notExists;
            }
            if (flag2)
              return (JSValue) null;
          }
          else
          {
            if (context._executionMode != ExecutionMode.Suspend)
              return (JSValue) null;
            if (suspendData == null)
              suspendData = new ForOf.SuspendData();
            context.SuspendData[(CodeNode) this] = (object) suspendData;
            suspendData.source = source;
            suspendData.variable = jsValue;
            suspendData.iterator = iterator;
            return (JSValue) null;
          }
        }
      }
      return (JSValue) null;
    }

    protected internal override CodeNode[] GetChildrenImpl()
    {
      List<CodeNode> codeNodeList = new List<CodeNode>();
      codeNodeList.Add(this._body);
      codeNodeList.Add(this._variable);
      codeNodeList.Add(this._source);
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
      Parser.Build<CodeNode>(ref this._variable, 2, variables, codeContext | CodeContext.InExpression, message, stats, opts);
      Parser.Build<CodeNode>(ref this._source, 2, variables, codeContext | CodeContext.InExpression, message, stats, opts);
      Parser.Build<CodeNode>(ref this._body, System.Math.Max(1, expressionDepth), variables, codeContext | CodeContext.Conditional | CodeContext.InLoop, message, stats, opts);
      if (this._variable is Comma)
        this._variable = (this._variable as Comma).RightOperand == null ? (CodeNode) (this._variable as Comma).LeftOperand : throw new InvalidOperationException("Invalid left-hand side in for-of");
      if (message != null && (this._source is ObjectDefinition || this._source is ArrayDefinition || this._source is Constant))
        message(MessageLevel.Recomendation, this.Position, this.Length, "for-of with constant source. This solution has a performance penalty. Rewrite without using for..in.");
      return false;
    }

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      this._variable.Optimize(ref this._variable, owner, message, opts, stats);
      this._source.Optimize(ref this._source, owner, message, opts, stats);
      if (this._body == null)
        return;
      this._body.Optimize(ref this._body, owner, message, opts, stats);
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override void Decompose(ref CodeNode self)
    {
      this._variable.Decompose(ref this._variable);
      this._source.Decompose(ref this._source);
      this._body?.Decompose(ref this._body);
    }

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
      this._variable.RebuildScope(functionInfo, transferedVariables, scopeBias);
      this._source.RebuildScope(functionInfo, transferedVariables, scopeBias);
      this._body?.RebuildScope(functionInfo, transferedVariables, scopeBias);
    }

    public override string ToString() => "for (" + this._variable?.ToString() + " of " + this._source?.ToString() + ")" + (this._body is CodeBlock ? "" : Environment.NewLine + "  ") + this._body?.ToString();

    private sealed class SuspendData
    {
      public JSValue source;
      public JSValue variable;
      public IIterator iterator;
    }
  }
}
