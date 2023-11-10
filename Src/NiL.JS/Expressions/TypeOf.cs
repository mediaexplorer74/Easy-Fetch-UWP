// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.TypeOf
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using System;
using System.Collections.Generic;

namespace NiL.JS.Expressions
{
  public sealed class TypeOf : Expression
  {
    protected internal override PredictedType ResultType => PredictedType.String;

    internal override bool ResultInTempContainer => false;

    public TypeOf(Expression first)
      : base(first, (Expression) null, false)
    {
      if (this._right != null)
        throw new InvalidOperationException("Second operand not allowed for typeof operator/");
    }

    public override JSValue Evaluate(Context context)
    {
      switch (this._left.Evaluate(context)._valueType)
      {
        case JSValueType.NotExists:
        case JSValueType.NotExistsInObject:
        case JSValueType.Undefined:
          return JSValue.undefinedString;
        case JSValueType.Boolean:
          return JSValue.booleanString;
        case JSValueType.Integer:
        case JSValueType.Double:
          return JSValue.numberString;
        case JSValueType.String:
          return JSValue.stringString;
        case JSValueType.Symbol:
          return JSValue.symbolString;
        case JSValueType.Function:
          return JSValue.functionString;
        default:
          return JSValue.objectString;
      }
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
      base.Build(ref _this, expressionDepth, variables, codeContext, message, stats, opts);
      if (this._left is Variable)
        (this._left as Variable)._suspendThrow = true;
      return false;
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => "typeof " + this._left?.ToString();
  }
}
