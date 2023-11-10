// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.NumberMoreOrEqual
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;

namespace NiL.JS.Expressions
{
  public sealed class NumberMoreOrEqual : Expression
  {
    protected internal override PredictedType ResultType => PredictedType.Bool;

    internal override bool ResultInTempContainer => false;

    public NumberMoreOrEqual(Expression first, Expression second)
      : base(first, second, false)
    {
    }

    public override JSValue Evaluate(Context context)
    {
      JSValue jsValue = this._left.Evaluate(context);
      if (jsValue._valueType == JSValueType.Integer || jsValue._valueType == JSValueType.Boolean)
      {
        int iValue = jsValue._iValue;
        JSValue second = this._right.Evaluate(context);
        if (second._valueType == JSValueType.Integer || second._valueType == JSValueType.Boolean)
          return (JSValue) (iValue >= second._iValue);
        if (second._valueType == JSValueType.Double)
          return (JSValue) ((double) iValue >= second._dValue);
        if (this._tempContainer == null)
          this._tempContainer = new JSValue()
          {
            _attributes = JSValueAttributesInternal.Temporary
          };
        this._tempContainer._valueType = JSValueType.Integer;
        this._tempContainer._iValue = iValue;
        return (JSValue) !Less.Check(this._tempContainer, second, true);
      }
      if (jsValue._valueType == JSValueType.Double)
      {
        double dValue = jsValue._dValue;
        JSValue second = this._right.Evaluate(context);
        if (second._valueType == JSValueType.Integer || second._valueType == JSValueType.Boolean)
          return (JSValue) (dValue >= (double) second._iValue);
        if (second._valueType == JSValueType.Double)
          return (JSValue) (dValue >= second._dValue);
        if (this._tempContainer == null)
          this._tempContainer = new JSValue()
          {
            _attributes = JSValueAttributesInternal.Temporary
          };
        this._tempContainer._valueType = JSValueType.Double;
        this._tempContainer._dValue = dValue;
        return (JSValue) !Less.Check(this._tempContainer, second, true);
      }
      if (this._tempContainer == null)
        this._tempContainer = new JSValue()
        {
          _attributes = JSValueAttributesInternal.Temporary
        };
      JSValue tempContainer = this._tempContainer;
      tempContainer.Assign(jsValue);
      this._tempContainer = (JSValue) null;
      int num = !Less.Check(tempContainer, this._right.Evaluate(context), true) ? 1 : 0;
      this._tempContainer = tempContainer;
      return (JSValue) (num != 0);
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => "(" + this._left?.ToString() + " >= " + this._right?.ToString() + ")";
  }
}
