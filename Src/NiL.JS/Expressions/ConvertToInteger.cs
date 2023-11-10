// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.ConvertToInteger
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace NiL.JS.Expressions
{
  public sealed class ConvertToInteger : Expression
  {
    protected internal override PredictedType ResultType => PredictedType.Int;

    internal override bool ResultInTempContainer => true;

    public ConvertToInteger(Expression first)
      : base(first, (Expression) null, true)
    {
    }

    public override JSValue Evaluate(Context context)
    {
      JSValue jsValue = this._left.Evaluate(context);
      if (jsValue._valueType == JSValueType.Integer)
        this._tempContainer._iValue = jsValue._iValue;
      else
        this._tempContainer._iValue = Tools.JSObjectToInt32(jsValue, 0, false);
      this._tempContainer._valueType = JSValueType.Integer;
      return this._tempContainer;
    }

    internal override System.Linq.Expressions.Expression TryCompile(
      bool selfCompile,
      bool forAssign,
      Type expectedType,
      List<CodeNode> dynamicValues)
    {
      System.Linq.Expressions.Expression expression = this._left.TryCompile(false, false, typeof (int), dynamicValues);
      if (expression == null)
        return (System.Linq.Expressions.Expression) null;
      if ((object) expression.Type == (object) typeof (int))
        return expression;
      if ((object) expression.Type == (object) typeof (bool))
        return (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Condition(expression, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) 1), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) 0));
      return (object) expression.Type == (object) typeof (double) ? (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Convert(expression, typeof (double)) : (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call(new Func<object, int>(Convert.ToInt32).GetMethodInfo(), expression);
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => "(" + this._left?.ToString() + " | 0)";
  }
}
