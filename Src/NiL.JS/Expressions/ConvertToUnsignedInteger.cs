// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.ConvertToUnsignedInteger
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;

namespace NiL.JS.Expressions
{
  public sealed class ConvertToUnsignedInteger : Expression
  {
    protected internal override PredictedType ResultType => PredictedType.Number;

    internal override bool ResultInTempContainer => true;

    public ConvertToUnsignedInteger(Expression first)
      : base(first, (Expression) null, true)
    {
    }

    public override JSValue Evaluate(Context context)
    {
      uint int32 = (uint) Tools.JSObjectToInt32(this._left.Evaluate(context));
      if (int32 <= (uint) int.MaxValue)
      {
        this._tempContainer._iValue = (int) int32;
        this._tempContainer._valueType = JSValueType.Integer;
      }
      else
      {
        this._tempContainer._dValue = (double) int32;
        this._tempContainer._valueType = JSValueType.Double;
      }
      return this._tempContainer;
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => "(" + this._left?.ToString() + " | 0)";
  }
}
