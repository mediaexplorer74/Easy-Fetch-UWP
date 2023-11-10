// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.Multiplication
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using System.Collections.Generic;

namespace NiL.JS.Expressions
{
  public sealed class Multiplication : Expression
  {
    protected internal override PredictedType ResultType => this._left.ResultType == PredictedType.Double ? PredictedType.Double : PredictedType.Number;

    internal override bool ResultInTempContainer => true;

    public Multiplication(Expression first, Expression second)
      : base(first, second, true)
    {
    }

    public override JSValue Evaluate(Context context)
    {
      JSValue jsValue1 = this._left.Evaluate(context);
      JSValue jsValue2;
      double num1;
      if ((jsValue1._valueType & (JSValueType) 15) > JSValueType.Undefined)
      {
        int iValue = jsValue1._iValue;
        jsValue2 = this._right.Evaluate(context);
        if ((jsValue2._valueType & (JSValueType) 15) > JSValueType.Undefined)
        {
          if (((long) (iValue | jsValue2._iValue) & 4294901760L) == 0L)
          {
            this._tempContainer._iValue = iValue * jsValue2._iValue;
            this._tempContainer._valueType = JSValueType.Integer;
          }
          else
          {
            long num2 = (long) iValue * (long) jsValue2._iValue;
            if (num2 > (long) int.MaxValue || num2 < (long) int.MinValue)
            {
              this._tempContainer._dValue = (double) num2;
              this._tempContainer._valueType = JSValueType.Double;
            }
            else
            {
              this._tempContainer._iValue = (int) num2;
              this._tempContainer._valueType = JSValueType.Integer;
            }
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
      this._tempContainer._dValue = num1 * Tools.JSObjectToDouble(jsValue2);
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
      if (!flag)
      {
        if (this._left is Constant left && Tools.JSObjectToDouble(left.Evaluate((Context) null)) == 1.0)
        {
          _this = (CodeNode) new ConvertToNumber(this._right);
          return true;
        }
        if (this._right is Constant right && Tools.JSObjectToDouble(right.Evaluate((Context) null)) == 1.0)
        {
          _this = (CodeNode) new ConvertToNumber(this._left);
          return true;
        }
      }
      return flag;
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString()
    {
      if (this._left is Constant && (this._left as Constant).value._valueType == JSValueType.Integer && (this._left as Constant).value._iValue == -1)
        return "-" + this._right?.ToString();
      return "(" + this._left?.ToString() + " * " + this._right?.ToString() + ")";
    }
  }
}
