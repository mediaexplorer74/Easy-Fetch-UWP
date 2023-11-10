// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.Comma
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using System.Collections.Generic;

namespace NiL.JS.Expressions
{
  public sealed class Comma : Expression
  {
    protected internal override PredictedType ResultType => (this._right ?? this._left).ResultType;

    internal override bool ResultInTempContainer => false;

    public Comma(Expression first, Expression second)
      : base(first, second, false)
    {
    }

    public override JSValue Evaluate(Context context)
    {
      JSValue jsValue = this._left.Evaluate(context);
      if (this._right != null)
      {
        if (context != null)
          context._objectSource = (JSValue) null;
        jsValue = this._right.Evaluate(context);
      }
      if (context != null)
        context._objectSource = (JSValue) null;
      return jsValue._valueType >= JSValueType.Object && jsValue._oValue is JSValue oValue ? oValue : jsValue;
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
      this._codeContext = codeContext;
      if (message != null && expressionDepth <= 1 && this._left != null && this._right != null)
        message(MessageLevel.Warning, this.Position, 0, "Do not use comma as a statements delimiter");
      if (this._right == null)
      {
        _this = (CodeNode) this._left;
        return true;
      }
      Parser.Build(ref this._left, expressionDepth + 1, variables, codeContext | CodeContext.InExpression, message, stats, opts);
      Parser.Build(ref this._right, expressionDepth + 1, variables, codeContext | CodeContext.InExpression, message, stats, opts);
      return false;
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => "(" + this._left?.ToString() + (this._right != null ? ", " + this._right?.ToString() : "") + ")";
  }
}
