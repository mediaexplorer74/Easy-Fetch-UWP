// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.Substract
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using System.Collections.Generic;

namespace NiL.JS.Expressions
{
  public sealed class Substract : Expression
  {
    protected internal override PredictedType ResultType => PredictedType.Number;

    internal override bool ResultInTempContainer => true;

    public Substract(Expression first, Expression second)
      : base(first, second, true)
    {
    }

    public override JSValue Evaluate(Context context)
    {
      JSValue jsValue1 = this._left.Evaluate(context);
      JSValue jsValue2;
      double num1;
      if (jsValue1._valueType == JSValueType.Integer || jsValue1._valueType == JSValueType.Boolean)
      {
        int iValue = jsValue1._iValue;
        jsValue2 = this._right.Evaluate(context);
        if (jsValue2._valueType == JSValueType.Integer || jsValue2._valueType == JSValueType.Boolean)
        {
          long num2 = (long) iValue - (long) jsValue2._iValue;
          if (num2 != (long) (int) num2)
          {
            this._tempContainer._dValue = (double) num2;
            this._tempContainer._valueType = JSValueType.Double;
          }
          else
          {
            this._tempContainer._iValue = (int) num2;
            this._tempContainer._valueType = JSValueType.Integer;
          }
          return this._tempContainer;
        }
        num1 = (double) iValue;
      }
      else
      {
        num1 = Tools.JSObjectToDouble(jsValue1);
        jsValue2 = this._right.Evaluate(context);
      }
      this._tempContainer._dValue = num1 - Tools.JSObjectToDouble(jsValue2);
      this._tempContainer._valueType = JSValueType.Double;
      return this._tempContainer;
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
      bool flag = base.Build(ref _this, expressionDepth, variables, codeContext, message, stats, opts);
      if (flag || !(this._left is Constant) || Tools.JSObjectToDouble(this._left.Evaluate((Context) null)) != 0.0)
        return flag;
      _this = (CodeNode) new Negation(this._right);
      return true;
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => "(" + this._left?.ToString() + " - " + this._right?.ToString() + ")";
  }
}
