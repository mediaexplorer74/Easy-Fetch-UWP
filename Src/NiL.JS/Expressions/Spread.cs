// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.Spread
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace NiL.JS.Expressions
{
  public sealed class Spread : Expression
  {
    protected internal override PredictedType ResultType => PredictedType.Unknown;

    internal override bool ResultInTempContainer => false;

    public Spread(Expression source)
      : base(source, (Expression) null, false)
    {
    }

    public override JSValue Evaluate(Context context)
    {
      JSObject jsObject = new JSObject();
      jsObject._oValue = (object) IterationProtocolExtensions.AsIterable(this._left.Evaluate(context)).AsEnumerable().ToArray<JSValue>();
      jsObject._valueType = JSValueType.SpreadOperatorResult;
      return (JSValue) jsObject;
    }

    protected internal override CodeNode[] GetChildrenImpl() => new CodeNode[1]
    {
      (CodeNode) this._left
    };

    public override bool Build(
      ref CodeNode _this,
      int expressionDepth,
      Dictionary<string, VariableDescriptor> variables,
      CodeContext codeContext,
      InternalCompilerMessageCallback message,
      FunctionInfo stats,
      Options opts)
    {
      CodeNode left = (CodeNode) this._left;
      int num = this._left.Build(ref left, expressionDepth, variables, codeContext, message, stats, opts) ? 1 : 0;
      if (!(left is Expression expression))
        expression = this._left;
      this._left = expression;
      return num != 0;
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit((Expression) this);

    public override string ToString() => "..." + this._left?.ToString();
  }
}
