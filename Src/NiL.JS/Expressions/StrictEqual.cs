// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.StrictEqual
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using System;

namespace NiL.JS.Expressions
{
  public class StrictEqual : Expression
  {
    protected internal override PredictedType ResultType => PredictedType.Bool;

    internal override bool ResultInTempContainer => false;

    public StrictEqual(Expression first, Expression second)
      : base(first, second, true)
    {
    }

    internal static bool Check(JSValue first, JSValue second)
    {
      switch (first._valueType)
      {
        case JSValueType.NotExists:
        case JSValueType.NotExistsInObject:
        case JSValueType.Undefined:
          return second._valueType <= JSValueType.Undefined;
        case JSValueType.Boolean:
          return first._valueType == second._valueType && first._iValue == second._iValue;
        case JSValueType.Integer:
          if (second._valueType == JSValueType.Double)
            return (double) first._iValue == second._dValue;
          return second._valueType == JSValueType.Integer && first._iValue == second._iValue;
        case JSValueType.Double:
          if (second._valueType == JSValueType.Integer)
            return first._dValue == (double) second._iValue;
          return second._valueType == JSValueType.Double && first._dValue == second._dValue;
        case JSValueType.String:
          return second._valueType == JSValueType.String && string.CompareOrdinal(first._oValue.ToString(), second._oValue.ToString()) == 0;
        case JSValueType.Symbol:
        case JSValueType.Object:
        case JSValueType.Function:
        case JSValueType.Date:
          if (first._valueType != second._valueType)
            return false;
          return first._oValue == null ? second._oValue == null : second._oValue == first._oValue;
        default:
          throw new NotImplementedException();
      }
    }

    public override JSValue Evaluate(Context context)
    {
      this._tempContainer.Assign(this._left.Evaluate(context));
      return StrictEqual.Check(this._tempContainer, this._right.Evaluate(context)) ? (JSValue) NiL.JS.BaseLibrary.Boolean.True : (JSValue) NiL.JS.BaseLibrary.Boolean.False;
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => "(" + this._left?.ToString() + " === " + this._right?.ToString() + ")";
  }
}
