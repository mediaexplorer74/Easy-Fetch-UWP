// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.Modulo
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;

namespace NiL.JS.Expressions
{
  public sealed class Modulo : Expression
  {
    protected internal override PredictedType ResultType
    {
      get
      {
        int resultType1 = (int) this._left.ResultType;
        PredictedType resultType2 = this._right.ResultType;
        int num = (int) resultType2;
        return resultType1 == num ? resultType2 : PredictedType.Number;
      }
    }

    internal override bool ResultInTempContainer => true;

    public Modulo(Expression first, Expression second)
      : base(first, second, true)
    {
    }

    public override JSValue Evaluate(Context context)
    {
      JSValue jsValue1 = this._left.Evaluate(context);
      if (jsValue1._valueType == JSValueType.Integer)
      {
        int iValue = jsValue1._iValue;
        JSValue jsValue2 = this._right.Evaluate(context);
        if (iValue >= 0 && jsValue2._valueType == JSValueType.Integer && jsValue2._iValue != 0)
        {
          this._tempContainer._valueType = JSValueType.Integer;
          this._tempContainer._iValue = iValue % jsValue2._iValue;
        }
        else
        {
          this._tempContainer._valueType = JSValueType.Double;
          this._tempContainer._dValue = (double) iValue % Tools.JSObjectToDouble(jsValue2);
        }
      }
      else
      {
        this._tempContainer._dValue = Tools.JSObjectToDouble(jsValue1) % Tools.JSObjectToDouble(this._right.Evaluate(context));
        this._tempContainer._valueType = JSValueType.Double;
      }
      return this._tempContainer;
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => "(" + this._left?.ToString() + " % " + this._right?.ToString() + ")";
  }
}
