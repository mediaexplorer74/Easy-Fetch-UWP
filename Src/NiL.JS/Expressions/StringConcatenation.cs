// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.StringConcatenation
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Statements;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiL.JS.Expressions
{
  public sealed class StringConcatenation : Expression
  {
    internal Expression[] _parts;

    protected internal override bool ContextIndependent
    {
      get
      {
        for (int index = 0; index < this._parts.Length; ++index)
        {
          if (!this._parts[index].ContextIndependent)
            return false;
        }
        return true;
      }
    }

    protected internal override bool NeedDecompose
    {
      get
      {
        for (int index = 0; index < this._parts.Length; ++index)
        {
          if (this._parts[index].NeedDecompose)
            return true;
        }
        return false;
      }
    }

    protected internal override PredictedType ResultType => PredictedType.String;

    internal override bool ResultInTempContainer => true;

    public StringConcatenation(Expression[] sources)
      : base((Expression) null, (Expression) null, true)
    {
      this._parts = sources.Length >= 2 ? sources : throw new ArgumentException("sources too short");
    }

    private static object prep(JSValue x)
    {
      if (x._valueType == JSValueType.String)
        return x._oValue;
      x = x._valueType != JSValueType.Date ? x.ToPrimitiveValue_Value_String() : x.ToPrimitiveValue_String_Value();
      return x._valueType == JSValueType.String ? x._oValue : (object) x.BaseToString();
    }

    public override JSValue Evaluate(Context context)
    {
      object firstSource = StringConcatenation.prep(this._parts[0].Evaluate(context));
      for (int index = 1; index < this._parts.Length; ++index)
        firstSource = (object) new RopeString(firstSource, StringConcatenation.prep(this._parts[index].Evaluate(context)));
      this._tempContainer._valueType = JSValueType.String;
      this._tempContainer._oValue = firstSource;
      return this._tempContainer;
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
      int num = base.Build(ref _this, expressionDepth, variables, codeContext, message, stats, opts) ? 1 : 0;
      if (num != 0)
        return num != 0;
      this._right = this._parts[this._parts.Length - 1];
      return num != 0;
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override void Decompose(ref Expression self, IList<CodeNode> result)
    {
      int num = -1;
      for (int index = 0; index < this._parts.Length; ++index)
      {
        Expression part = this._parts[index];
        this._parts[index].Decompose(ref part, result);
        this._parts[index] = part;
        if (this._parts[index].NeedDecompose)
          num = index;
      }
      for (int index = 0; index < num; ++index)
      {
        if (!(this._parts[index] is ExtractStoredValue))
        {
          result.Add((CodeNode) new StoreValue(this._parts[index], false));
          this._parts[index] = (Expression) new ExtractStoredValue(this._parts[index]);
        }
      }
    }

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
      base.RebuildScope(functionInfo, transferedVariables, scopeBias);
      for (int index = 0; index < this._parts.Length; ++index)
        this._parts[index].RebuildScope(functionInfo, transferedVariables, scopeBias);
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder("(", this._parts.Length * 10).Append((object) this._parts[0]);
      for (int index = 1; index < this._parts.Length; ++index)
        stringBuilder.Append(" + ").Append((object) this._parts[index]);
      return stringBuilder.Append(")").ToString();
    }
  }
}
