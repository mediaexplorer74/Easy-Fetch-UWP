// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.LessOrEqual
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;

namespace NiL.JS.Expressions
{
  public sealed class LessOrEqual : More
  {
    protected internal override PredictedType ResultType => PredictedType.Bool;

    public LessOrEqual(Expression first, Expression second)
      : base(first, second)
    {
    }

    public override JSValue Evaluate(Context context) => (JSValue) (base.Evaluate(context)._iValue == 0);

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      this.baseOptimize(ref _this, owner, message, opts, stats);
      if (_this != this || this._left.ResultType != PredictedType.Number || this._right.ResultType != PredictedType.Number)
        return;
      _this = (CodeNode) new NumberLessOrEqual(this._left, this._right);
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => "(" + this._left?.ToString() + " <= " + this._right?.ToString() + ")";
  }
}
