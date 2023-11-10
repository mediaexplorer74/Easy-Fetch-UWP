// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.ConvertToBoolean
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;

namespace NiL.JS.Expressions
{
  public sealed class ConvertToBoolean : Expression
  {
    protected internal override PredictedType ResultType => PredictedType.Bool;

    internal override bool ResultInTempContainer => false;

    public ConvertToBoolean(Expression first)
      : base(first, (Expression) null, false)
    {
    }

    public override JSValue Evaluate(Context context) => (JSValue) (bool) this._left.Evaluate(context);

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => "!!" + this._left?.ToString();
  }
}
