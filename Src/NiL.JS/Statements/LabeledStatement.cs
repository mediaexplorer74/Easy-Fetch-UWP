// Decompiled with JetBrains decompiler
// Type: NiL.JS.Statements.LabeledStatement
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Expressions;
using System.Collections.Generic;

namespace NiL.JS.Statements
{
  public sealed class LabeledStatement : CodeNode
  {
    private CodeNode statement;
    private string label;

    public CodeNode Statement => this.statement;

    public string Label => this.label;

    internal static CodeNode Parse(ParseInfo state, ref int index)
    {
      int index1 = index;
      if (!Parser.ValidateName(state.Code, ref index1, state.Strict))
        return (CodeNode) null;
      int num1 = index1;
      if (index1 >= state.Code.Length || !Parser.Validate(state.Code, " :", ref index1) && state.Code[index1++] != ':')
        return (CodeNode) null;
      string str = state.Code.Substring(index, num1 - index);
      state.Labels.Add(str);
      int labelsCount = state.LabelsCount;
      ++state.LabelsCount;
      CodeNode codeNode = Parser.Parse(state, ref index1, CodeFragmentType.Statement);
      state.Labels.Remove(str);
      state.LabelsCount = labelsCount;
      if (codeNode is FunctionDefinition && state.Message != null)
        state.Message(MessageLevel.CriticalWarning, codeNode.Position, codeNode.Length, "Labeled function. Are you sure?");
      int num2 = index;
      index = index1;
      LabeledStatement labeledStatement = new LabeledStatement();
      labeledStatement.statement = codeNode;
      labeledStatement.label = str;
      labeledStatement.Position = num2;
      labeledStatement.Length = index - num2;
      return (CodeNode) labeledStatement;
    }

    public override JSValue Evaluate(Context context)
    {
      JSValue jsValue = this.statement.Evaluate(context);
      if (context._executionMode != ExecutionMode.Break || context._executionInfo == null || !(context._executionInfo._oValue as string == this.label))
        return jsValue;
      context._executionMode = ExecutionMode.Regular;
      context._executionInfo = JSValue.notExists;
      return jsValue;
    }

    protected internal override CodeNode[] GetChildrenImpl() => new CodeNode[1]
    {
      this.statement
    };

    public override bool Build(
      ref CodeNode _this,
      int expressionDepth,
      Dictionary<string, VariableDescriptor> variables,
      CodeContext codeContext,
      InternalCompilerMessageCallback message,
      FunctionInfo stats,
      Options opts)
    {
      Parser.Build<CodeNode>(ref this.statement, expressionDepth, variables, codeContext, message, stats, opts);
      if (this.statement == null)
        _this = (CodeNode) null;
      return false;
    }

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      this.statement.Optimize(ref this.statement, owner, message, opts, stats);
      if (this.statement != null)
        return;
      _this = (CodeNode) null;
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => this.label + ": " + this.statement?.ToString();

    public override void Decompose(ref CodeNode self) => this.statement.Decompose(ref this.statement);

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
      this.statement.RebuildScope(functionInfo, transferedVariables, scopeBias);
    }
  }
}
