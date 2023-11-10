// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.ConvertToString
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;

namespace NiL.JS.Expressions
{
  public sealed class ConvertToString : Expression
  {
    protected internal override PredictedType ResultType => PredictedType.String;

    internal override bool ResultInTempContainer => true;

    public ConvertToString(Expression first)
      : base(first, (Expression) null, true)
    {
    }

    public override JSValue Evaluate(Context context)
    {
      JSValue jsValue = this._left.Evaluate(context);
      if (jsValue._valueType == JSValueType.String)
        return jsValue;
      this._tempContainer._valueType = JSValueType.String;
      this._tempContainer._oValue = (object) jsValue.ToPrimitiveValue_Value_String().ToString();
      return this._tempContainer;
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => "( \"\" + " + this._left?.ToString() + ")";
  }
}
