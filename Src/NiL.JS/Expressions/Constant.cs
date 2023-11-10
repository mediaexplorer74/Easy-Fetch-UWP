// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.Constant
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using System.Collections.Generic;

namespace NiL.JS.Expressions
{
  public sealed class Constant : Expression
  {
    internal JSValue value;

    public JSValue Value => this.value;

    protected internal override PredictedType ResultType
    {
      get
      {
        if (this.value == null)
          return PredictedType.Unknown;
        switch (this.value._valueType)
        {
          case JSValueType.NotExists:
          case JSValueType.NotExistsInObject:
          case JSValueType.Undefined:
            return PredictedType.Undefined;
          case JSValueType.Boolean:
            return PredictedType.Bool;
          case JSValueType.Integer:
            return PredictedType.Int;
          case JSValueType.Double:
            return PredictedType.Double;
          case JSValueType.String:
            return PredictedType.String;
          default:
            return PredictedType.Object;
        }
      }
    }

    internal override bool ResultInTempContainer => false;

    protected internal override bool ContextIndependent => true;

    public Constant(JSValue value)
      : base((Expression) null, (Expression) null, false)
    {
      this.value = value;
    }

    public override JSValue Evaluate(Context context) => this.value;

    protected internal override JSValue EvaluateForWrite(Context context) => this.value == JSValue.undefined ? this.value : base.EvaluateForWrite(context);

    protected internal override CodeNode[] GetChildrenImpl() => this.value != null && this.value._oValue is CodeNode[] ? this.value._oValue as CodeNode[] : (CodeNode[]) null;

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
      if ((opts & Options.SuppressUselessExpressionsElimination) == Options.None && expressionDepth <= 1)
      {
        _this = (CodeNode) null;
        this.Eliminated = true;
        if (message != null && (this.value._valueType != JSValueType.String || this.value._oValue.ToString() != "use strict"))
          message(MessageLevel.Warning, this.Position, this.Length, "Unused constant was removed. Maybe, something missing.");
      }
      return false;
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString()
    {
      if (this.value == null)
        return "";
      if (this.value._valueType == JSValueType.String)
        return "\"" + this.value._oValue?.ToString() + "\"";
      if (!(this.value._oValue is CodeNode[]))
        return this.value.ToString();
      string str = "";
      int length = (this.value._oValue as CodeNode[]).Length;
      while (length-- > 0)
        str = (length != 0 ? ", " : "") + (this.value._oValue as CodeNode[])[length]?.ToString() + str;
      return str;
    }

    public override void Decompose(ref Expression self, IList<CodeNode> result)
    {
    }
  }
}
