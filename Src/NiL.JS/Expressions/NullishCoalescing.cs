// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.NullishCoalescing
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using System.Collections.Generic;

namespace NiL.JS.Expressions
{
  public sealed class NullishCoalescing : Expression
  {
    protected internal override PredictedType ResultType => PredictedType.Bool;

    internal override bool ResultInTempContainer => false;

    public NullishCoalescing(Expression first, Expression second)
      : base(first, second, false)
    {
    }

    public override JSValue Evaluate(Context context)
    {
      JSValue jsValue = this._left.Evaluate(context);
      return jsValue.Defined && !jsValue.IsNull ? jsValue : this._right.Evaluate(context);
    }

    public override bool Build(
      ref CodeNode _this,
      int expressionDepth,
      Dictionary<string, VariableDescriptor> variables,
      CodeContext codeContext,
      InternalCompilerMessageCallback message,
      FunctionInfo stats,
      Options opts)
    {
      if (message != null && expressionDepth <= 1)
        message(MessageLevel.Warning, this.Position, 0, "Do not use logical operator as a conditional statement");
      return base.Build(ref _this, expressionDepth, variables, codeContext | CodeContext.Conditional, message, stats, opts);
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit((Expression) this);

    public override string ToString() => "(" + this._left?.ToString() + " ?? " + this._right?.ToString() + ")";
  }
}
