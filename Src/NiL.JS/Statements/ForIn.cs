// Decompiled with JetBrains decompiler
// Type: NiL.JS.Statements.ForIn
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
  public sealed class ForIn : CodeNode
  {
    private CodeNode _variable;
    private CodeNode _source;
    private CodeNode _body;
    private string[] _labels;

    public CodeNode Variable => this._variable;

    public CodeNode Source => this._source;

    public CodeNode Body => this._body;

    public ReadOnlyCollection<string> Labels => new ReadOnlyCollection<string>((IList<string>) this._labels);

    private ForIn()
    {
    }

    internal static CodeNode Parse(ParseInfo state, ref int index)
    {
      int num = index;
      Tools.SkipSpaces(state.Code, ref num);
      if (!Parser.Validate(state.Code, "for(", ref num) && !Parser.Validate(state.Code, "for (", ref num))
        return (CodeNode) null;
      Tools.SkipSpaces(state.Code, ref num);
      ForIn forIn = new ForIn()
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
        if (!Parser.Validate(state.Code, "in", ref num))
        {
          if (count < state.Variables.Count)
            state.Variables.RemoveRange(count, state.Variables.Count - count);
          return (CodeNode) null;
        }
        if (state.Strict && (name == "arguments" || name == "eval"))
          ExceptionHelper.ThrowSyntaxError("Parameters name may not be \"arguments\" or \"eval\" in strict mode at ", state.Code, source.Position, source.Length);
        if (source is VariableDefinition && (source as VariableDefinition)._variables.Length > 1)
          ExceptionHelper.ThrowSyntaxError("Too many variables in for-in loop", state.Code, num);
        forIn._variable = source;
        state.LabelsCount = 0;
        Tools.SkipSpaces(state.Code, ref num);
        forIn._source = Parser.Parse(state, ref num, CodeFragmentType.Expression);
        Tools.SkipSpaces(state.Code, ref num);
        if (state.Code[num] != ')')
          ExceptionHelper.Throw((Error) new SyntaxError("Expected \")\" at + " + CodeCoordinates.FromTextPosition(state.Code, num, 0)?.ToString()));
        ++num;
        state.AllowBreak.Push(true);
        state.AllowContinue.Push(true);
        forIn._body = Parser.Parse(state, ref num, CodeFragmentType.Statement);
        state.AllowBreak.Pop();
        state.AllowContinue.Pop();
        forIn.Position = index;
        forIn.Length = num - index;
        index = num;
        variableDescriptorArray = CodeBlock.extractVariables(state, count);
      }
      finally
      {
        --state.LexicalScopeLevel;
      }
      CodeBlock codeBlock = new CodeBlock((CodeNode[]) new ForIn[1]
      {
        forIn
      });
      codeBlock._variables = variableDescriptorArray;
      codeBlock.Position = forIn.Position;
      codeBlock.Length = forIn.Length;
      return (CodeNode) codeBlock;
    }

    public override JSValue Evaluate(Context context)
    {
      ForIn.SuspendData suspendData = (ForIn.SuspendData) null;
      if (context._executionMode >= ExecutionMode.Resume)
        suspendData = context.SuspendData[(CodeNode) this] as ForIn.SuspendData;
      JSValue jsValue1;
      if (suspendData == null || suspendData.source == null)
      {
        if (context._debugging && !(this._source is CodeBlock))
          context.raiseDebugger(this._source);
        jsValue1 = this._source.Evaluate(context);
        if (context._executionMode == ExecutionMode.Suspend)
        {
          context.SuspendData[(CodeNode) this] = (object) null;
          return (JSValue) null;
        }
      }
      else
        jsValue1 = suspendData.source;
      JSValue jsValue2;
      if (suspendData == null || suspendData.variable == null)
      {
        if (context._debugging && !(this._variable is CodeBlock))
          context.raiseDebugger(this._variable);
        if (this._variable is VariableDefinition variable)
        {
          this._variable.Evaluate(context);
          jsValue2 = (variable._initializers[0]._left ?? variable._initializers[0]).EvaluateForWrite(context);
        }
        else if (this._variable is Assignment)
        {
          this._variable.Evaluate(context);
          jsValue2 = (this._variable as Assignment)._left.Evaluate(context);
        }
        else
          jsValue2 = this._variable.EvaluateForWrite(context);
        if (context._executionMode == ExecutionMode.Suspend)
        {
          if (suspendData == null)
            suspendData = new ForIn.SuspendData();
          context.SuspendData[(CodeNode) this] = (object) suspendData;
          suspendData.source = jsValue1;
          return (JSValue) null;
        }
      }
      else
        jsValue2 = suspendData.variable;
      if (!jsValue1.Defined || jsValue1._valueType >= JSValueType.Object && jsValue1._oValue == null || this._body == null)
        return JSValue.undefined;
      int num = 0;
      HashSet<string> stringSet = suspendData?.processedKeys ?? new HashSet<string>((IEqualityComparer<string>) StringComparer.Ordinal);
      while (jsValue1 != null)
      {
        IEnumerator<KeyValuePair<string, JSValue>> enumerator = suspendData?.keys ?? jsValue1.GetEnumerator(false, EnumerationMode.RequireValues);
        while (context._executionMode >= ExecutionMode.Resume || enumerator.MoveNext())
        {
          if (context._executionMode != ExecutionMode.Resume)
          {
            KeyValuePair<string, JSValue> current = enumerator.Current;
            string key = current.Key;
            if (!stringSet.Contains(key))
            {
              stringSet.Add(key);
              jsValue2._valueType = JSValueType.String;
              jsValue2._oValue = (object) key;
              current = enumerator.Current;
              if ((current.Value._attributes & JSValueAttributesInternal.DoNotEnumerate) == JSValueAttributesInternal.None)
              {
                if (context._debugging && !(this._body is CodeBlock))
                  context.raiseDebugger(this._body);
              }
              else
                continue;
            }
            else
              continue;
          }
          context._lastResult = this._body.Evaluate(context) ?? context._lastResult;
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
                suspendData = new ForIn.SuspendData();
              context.SuspendData[(CodeNode) this] = (object) suspendData;
              suspendData.source = jsValue1;
              suspendData.variable = jsValue2;
              suspendData.processedKeys = stringSet;
              suspendData.keys = enumerator;
              return (JSValue) null;
            }
          }
          ++num;
        }
        jsValue1 = (JSValue) jsValue1.__proto__;
        if (jsValue1 == JSValue.@null || !jsValue1.Defined || jsValue1._valueType >= JSValueType.Object && jsValue1._oValue == null)
          break;
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
        this._variable = (this._variable as Comma).RightOperand == null ? (CodeNode) (this._variable as Comma).LeftOperand : throw new InvalidOperationException("Invalid left-hand side in for-in");
      if (message != null && (this._source is ObjectDefinition || this._source is ArrayDefinition || this._source is Constant))
        message(MessageLevel.Recomendation, this.Position, this.Length, "for..in with constant source. This reduce performance. Rewrite without using for..in.");
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

    public override string ToString() => "for (" + this._variable?.ToString() + " in " + this._source?.ToString() + ")" + (this._body is CodeBlock ? "" : Environment.NewLine + "  ") + this._body?.ToString();

    private sealed class SuspendData
    {
      public JSValue source;
      public JSValue variable;
      public HashSet<string> processedKeys;
      public IEnumerator<KeyValuePair<string, JSValue>> keys;
    }
  }
}
