// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.ArrayDefinition
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Statements;
using System;
using System.Collections.Generic;

namespace NiL.JS.Expressions
{
  public sealed class ArrayDefinition : Expression
  {
    private static JSValue writableNotExists;
    private Expression[] elements;

    public Expression[] Elements => this.elements;

    protected internal override bool ContextIndependent => false;

    internal override bool ResultInTempContainer => false;

    protected internal override bool NeedDecompose
    {
      get
      {
        for (int index = 0; index < this.elements.Length; ++index)
        {
          if (this.elements[index] != null && this.elements[index].NeedDecompose)
            return true;
        }
        return false;
      }
    }

    private ArrayDefinition()
    {
    }

    internal static CodeNode Parse(ParseInfo state, ref int index)
    {
      int index1 = index;
      if (state.Code[index] != '[')
        throw new ArgumentException("Syntax error. Expected '['");
      do
      {
        ++index1;
      }
      while (Tools.IsWhiteSpace(state.Code[index1]));
      List<Expression> expressionList1 = new List<Expression>();
      while (state.Code[index1] != ']')
      {
        int num = index1;
        bool flag = Parser.Validate(state.Code, "...", ref index1);
        if (state.Code[index1] == ',')
        {
          if (flag)
            ExceptionHelper.ThrowSyntaxError("Expected expression", state.Code, index1);
          expressionList1.Add((Expression) null);
        }
        else
          expressionList1.Add(ExpressionTree.Parse(state, ref index1, false, false, false, true, false));
        if (flag)
        {
          List<Expression> expressionList2 = expressionList1;
          int index2 = expressionList1.Count - 1;
          Spread spread = new Spread(expressionList1[expressionList1.Count - 1]);
          spread.Position = num;
          spread.Length = index1 - num;
          expressionList2[index2] = (Expression) spread;
        }
        while (Tools.IsWhiteSpace(state.Code[index1]))
          ++index1;
        if (state.Code[index1] == ',')
        {
          do
          {
            ++index1;
          }
          while (Tools.IsWhiteSpace(state.Code[index1]));
        }
        else if (state.Code[index1] != ']')
          ExceptionHelper.ThrowSyntaxError("Expected ']'", state.Code, index1);
      }
      ++index1;
      int num1 = index;
      index = index1;
      ArrayDefinition arrayDefinition = new ArrayDefinition();
      arrayDefinition.elements = expressionList1.ToArray();
      arrayDefinition.Position = num1;
      arrayDefinition.Length = index - num1;
      return (CodeNode) arrayDefinition;
    }

    public override JSValue Evaluate(Context context)
    {
      int length = this.elements.Length;
      NiL.JS.BaseLibrary.Array array = new NiL.JS.BaseLibrary.Array(length);
      if (length > 0)
      {
        int index1 = 0;
        int index2 = 0;
        while (index1 < length)
        {
          if (this.elements[index1] != null)
          {
            JSValue jsValue1 = this.elements[index1].Evaluate(context);
            if (jsValue1._valueType == JSValueType.SpreadOperatorResult)
            {
              IList<JSValue> oValue = jsValue1._oValue as IList<JSValue>;
              int index3 = 0;
              while (index3 < oValue.Count)
              {
                array._data[index2] = oValue[index3].CloneImpl(false);
                ++index3;
                ++index2;
              }
              --index2;
            }
            else
            {
              JSValue jsValue2 = jsValue1.CloneImpl(true);
              jsValue2._attributes = JSValueAttributesInternal.None;
              array._data[index2] = jsValue2;
            }
          }
          else
          {
            if (ArrayDefinition.writableNotExists == null)
              ArrayDefinition.writableNotExists = new JSValue()
              {
                _valueType = JSValueType.NotExistsInObject,
                _attributes = JSValueAttributesInternal.SystemObject
              };
            array._data[index2] = ArrayDefinition.writableNotExists;
          }
          ++index1;
          ++index2;
        }
      }
      return (JSValue) array;
    }

    protected internal override CodeNode[] GetChildrenImpl() => (CodeNode[]) this.elements;

    public override bool Build(
      ref CodeNode _this,
      int expressionDepth,
      Dictionary<string, VariableDescriptor> variables,
      CodeContext codeContext,
      InternalCompilerMessageCallback message,
      FunctionInfo stats,
      Options opts)
    {
      this._codeContext = codeContext;
      for (int index = 0; index < this.elements.Length; ++index)
        Parser.Build(ref this.elements[index], 2, variables, codeContext | CodeContext.InExpression, message, stats, opts);
      return false;
    }

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      int length = this.elements.Length;
      while (length-- > 0)
      {
        CodeNode element = (CodeNode) this.elements[length];
        if (element != null)
        {
          element.Optimize(ref element, owner, message, opts, stats);
          this.elements[length] = element as Expression;
        }
      }
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override void Decompose(ref Expression self, IList<CodeNode> result)
    {
      int num = -1;
      for (int index = 0; index < this.elements.Length; ++index)
      {
        this.elements[index].Decompose(ref this.elements[index], result);
        if (this.elements[index].NeedDecompose)
          num = index;
      }
      for (int index = 0; index < num; ++index)
      {
        if (!(this.elements[index] is ExtractStoredValue))
        {
          result.Add((CodeNode) new StoreValue(this.elements[index], false));
          this.elements[index] = (Expression) new ExtractStoredValue(this.elements[index]);
        }
      }
    }

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
      base.RebuildScope(functionInfo, transferedVariables, scopeBias);
      for (int index = 0; index < this.elements.Length; ++index)
        this.elements[index]?.RebuildScope(functionInfo, transferedVariables, scopeBias);
    }

    public override string ToString()
    {
      string str = "[";
      for (int index = 0; index < this.elements.Length; ++index)
      {
        str += this.elements[index]?.ToString();
        if (index + 1 < this.elements.Length)
          str += ", ";
      }
      return str + "]";
    }
  }
}
