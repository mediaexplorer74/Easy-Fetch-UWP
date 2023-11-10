// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.Power
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using System;
using System.Collections.Generic;

namespace NiL.JS.Expressions
{
  public sealed class Power : Expression
  {
    protected internal override PredictedType ResultType => PredictedType.Int;

    internal override bool ResultInTempContainer => true;

    public Power(Expression first, Expression second)
      : base(first, second, true)
    {
    }

    public override JSValue Evaluate(Context context)
    {
      this._tempContainer._dValue = Math.Pow(Tools.JSObjectToDouble(this._left.Evaluate(context)), Tools.JSObjectToDouble(this._right.Evaluate(context)));
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
        if (_this == this)
        {
          try
          {
            if (this._left.ContextIndependent)
              this._left = (Expression) new Constant((JSValue) Tools.JSObjectToDouble(this._left.Evaluate((Context) null)));
            if (this._right.ContextIndependent)
            {
              if (this._left.ContextIndependent)
              {
                _this = (CodeNode) new Constant((JSValue) Math.Pow(Tools.JSObjectToDouble(this._left.Evaluate((Context) null)), Tools.JSObjectToDouble(this._right.Evaluate((Context) null))));
              }
              else
              {
                int int32 = Tools.JSObjectToInt32(this._right.Evaluate((Context) null));
                switch (int32)
                {
                  case 0:
                    _this = (CodeNode) new Constant((JSValue) 1);
                    break;
                  case 1:
                    _this = (CodeNode) this._left;
                    break;
                  case 2:
                    _this = (CodeNode) new Multiplication(this._left, this._left);
                    break;
                  default:
                    this._right = (Expression) new Constant((JSValue) int32);
                    break;
                }
              }
            }
          }
          catch
          {
          }
        }
      }
      return flag;
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit((Expression) this);

    public override string ToString() => "(" + this._left?.ToString() + " ** " + this._right?.ToString() + ")";
  }
}
