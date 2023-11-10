// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.NumberAddition
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;

namespace NiL.JS.Expressions
{
  public sealed class NumberAddition : Expression
  {
    protected internal override PredictedType ResultType => this._left.ResultType == PredictedType.Double ? PredictedType.Double : PredictedType.Number;

    internal override bool ResultInTempContainer => true;

    public NumberAddition(Expression first, Expression second)
      : base(first, second, true)
    {
    }

    public override JSValue Evaluate(Context context)
    {
      JSValue jsValue = this._left.Evaluate(context);
      if (jsValue._valueType == JSValueType.Integer)
      {
        int iValue = jsValue._iValue;
        JSValue second = this._right.Evaluate(context);
        if (second._valueType == JSValueType.Integer)
        {
          long num = (long) iValue + (long) second._iValue;
          if ((long) (int) num == num)
          {
            this._tempContainer._valueType = JSValueType.Integer;
            this._tempContainer._iValue = (int) num;
          }
          else
          {
            this._tempContainer._valueType = JSValueType.Double;
            this._tempContainer._dValue = (double) num;
          }
        }
        else if (second._valueType == JSValueType.Double)
        {
          this._tempContainer._valueType = JSValueType.Double;
          this._tempContainer._dValue = (double) iValue + second._dValue;
        }
        else
        {
          this._tempContainer._valueType = JSValueType.Integer;
          this._tempContainer._iValue = iValue;
          Addition.Impl(this._tempContainer, this._tempContainer, second);
        }
      }
      else if (jsValue._valueType == JSValueType.Double)
      {
        double dValue = jsValue._dValue;
        JSValue second = this._right.Evaluate(context);
        if (second._valueType == JSValueType.Integer)
        {
          this._tempContainer._valueType = JSValueType.Double;
          this._tempContainer._dValue = dValue + (double) second._iValue;
        }
        else if (second._valueType == JSValueType.Double)
        {
          this._tempContainer._valueType = JSValueType.Double;
          this._tempContainer._dValue = dValue + second._dValue;
        }
        else
        {
          this._tempContainer._valueType = JSValueType.Double;
          this._tempContainer._dValue = dValue;
          Addition.Impl(this._tempContainer, this._tempContainer, second);
        }
      }
      else
        Addition.Impl(this._tempContainer, jsValue.CloneImpl(false), this._right.Evaluate(context));
      return this._tempContainer;
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => "(" + this._left?.ToString() + " + " + this._right?.ToString() + ")";
  }
}
