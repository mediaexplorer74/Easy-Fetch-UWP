// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.Negation
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;

namespace NiL.JS.Expressions
{
  public sealed class Negation : Expression
  {
    protected internal override PredictedType ResultType => PredictedType.Number;

    internal override bool ResultInTempContainer => true;

    public Negation(Expression first)
      : base(first, (Expression) null, true)
    {
    }

    public override JSValue Evaluate(Context context)
    {
      JSValue jsValue = this._left.Evaluate(context);
      if (jsValue._valueType == JSValueType.Integer || jsValue.ValueType == JSValueType.Boolean)
      {
        if (jsValue._iValue == 0)
        {
          this._tempContainer._valueType = JSValueType.Double;
          this._tempContainer._dValue = -0.0;
        }
        else if (jsValue._iValue == int.MinValue)
        {
          this._tempContainer._valueType = JSValueType.Double;
          this._tempContainer._dValue = (double) jsValue._iValue;
        }
        else
        {
          this._tempContainer._valueType = JSValueType.Integer;
          this._tempContainer._iValue = -jsValue._iValue;
        }
      }
      else
      {
        this._tempContainer._dValue = -Tools.JSObjectToDouble(jsValue);
        this._tempContainer._valueType = JSValueType.Double;
      }
      return this._tempContainer;
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => "-" + this._left?.ToString();
  }
}
