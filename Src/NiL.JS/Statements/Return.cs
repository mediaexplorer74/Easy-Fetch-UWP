// Decompiled with JetBrains decompiler
// Type: NiL.JS.Statements.Return
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using NiL.JS.Expressions;
using System.Collections.Generic;

namespace NiL.JS.Statements
{
  public sealed class Return : CodeNode
  {
    private Expression value;

    public Expression Value => this.value;

    internal Return()
    {
    }

    internal Return(Expression value) => this.value = value;

    internal static CodeNode Parse(ParseInfo state, ref int index)
    {
      int index1 = index;
      if (!Parser.Validate(state.Code, "return", ref index1) || !Parser.IsIdentifierTerminator(state.Code[index1]))
        return (CodeNode) null;
      if (state.AllowReturn == 0)
        ExceptionHelper.Throw((Error) new SyntaxError("Invalid use of return statement."));
      while (index1 < state.Code.Length && Tools.IsWhiteSpace(state.Code[index1]) && !Tools.IsLineTerminator(state.Code[index1]))
        ++index1;
      using (state.WithCodeContext(CodeContext.InExpression))
      {
        CodeNode codeNode = state.Code[index1] == ';' || Tools.IsLineTerminator(state.Code[index1]) ? (CodeNode) null : Parser.Parse(state, ref index1, CodeFragmentType.Expression);
        int num = index;
        index = index1;
        Return @return = new Return();
        @return.value = (Expression) codeNode;
        @return.Position = num;
        @return.Length = index - num;
        return (CodeNode) @return;
      }
    }

    public override JSValue Evaluate(Context context)
    {
      JSValue jsValue = this.value != null ? this.value.Evaluate(context) : (JSValue) null;
      if (context._executionMode == ExecutionMode.Regular)
      {
        context._executionInfo = jsValue;
        if (context._executionMode < ExecutionMode.Return)
          context._executionMode = ExecutionMode.Return;
      }
      return JSValue.notExists;
    }

    protected internal override CodeNode[] GetChildrenImpl()
    {
      if (this.value == null)
        return new CodeNode[0];
      return new CodeNode[1]{ (CodeNode) this.value };
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
      Parser.Build(ref this.value, expressionDepth + 1, variables, codeContext | CodeContext.InExpression, message, stats, opts);
      if (message == null && this.value is Conditional)
      {
        Conditional conditional = this.value as Conditional;
        IList<Expression> threads = conditional.Threads;
        ref CodeNode local = ref _this;
        IfElse ifElse = new IfElse(conditional.LeftOperand, (CodeNode) new Return(threads[0]), (CodeNode) new Return(threads[1]));
        ifElse.Position = conditional.Position;
        ifElse.Length = conditional.Length;
        local = (CodeNode) ifElse;
        return true;
      }
      if (this.value is Call)
        (this.value as Call).allowTCO = true;
      stats.Returns.Add(this.value ?? (Expression) Empty.Instance);
      return false;
    }

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      if (this.value == null)
        return;
      CodeNode _this1 = (CodeNode) this.value;
      this.value.Optimize(ref _this1, owner, message, opts, stats);
      this.value = (Expression) _this1;
      if (!(this.value is Empty) && (!(this.value is Constant) || this.value.Evaluate((Context) null) != JSValue.undefined))
        return;
      this.value = (Expression) null;
    }

    public override void Decompose(ref CodeNode self)
    {
      if (this.value == null)
        return;
      this.value.Decompose(ref this.value);
    }

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
      this.value?.RebuildScope(functionInfo, transferedVariables, scopeBias);
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => "return" + (this.value != null ? " " + this.value?.ToString() : "");
  }
}
