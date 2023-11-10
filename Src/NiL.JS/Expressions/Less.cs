// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.Less
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using System;

namespace NiL.JS.Expressions
{
  public class Less : Expression
  {
    private bool trueLess;

    protected internal override PredictedType ResultType => PredictedType.Bool;

    internal override bool ResultInTempContainer => false;

    internal Less(Expression first, Expression second)
      : base(first, second, true)
    {
      this.trueLess = (object) this.GetType() == (object) typeof (Less);
    }

    internal static bool Check(JSValue first, JSValue second) => Less.Check(first, second, false);

    internal static bool Check(JSValue first, JSValue second, bool moreOrEqual)
    {
      switch (first._valueType)
      {
        case JSValueType.Boolean:
        case JSValueType.Integer:
          switch (second._valueType)
          {
            case JSValueType.Boolean:
            case JSValueType.Integer:
              return first._iValue < second._iValue;
            case JSValueType.Double:
              return double.IsNaN(second._dValue) ? moreOrEqual : (double) first._iValue < second._dValue;
            case JSValueType.String:
              int index1 = 0;
              double num1 = 0.0;
              return Tools.ParseNumber(second._oValue.ToString(), ref index1, out num1) && index1 == second._oValue.ToString().Length ? (double) first._iValue < num1 : moreOrEqual;
            case JSValueType.Object:
            case JSValueType.Date:
              second = second.ToPrimitiveValue_Value_String();
              if (second._valueType != JSValueType.Integer && second._valueType != JSValueType.Boolean)
              {
                if (second._valueType != JSValueType.Double)
                {
                  if (second._valueType != JSValueType.String)
                  {
                    if (second._valueType >= JSValueType.Object)
                      return first._iValue < 0;
                    throw new NotImplementedException();
                  }
                  goto case JSValueType.String;
                }
                else
                  goto case JSValueType.Double;
              }
              else
                goto case JSValueType.Boolean;
            default:
              return moreOrEqual;
          }
        case JSValueType.Double:
          if (double.IsNaN(first._dValue))
            return moreOrEqual;
          switch (second._valueType)
          {
            case JSValueType.Boolean:
            case JSValueType.Integer:
              return first._dValue < (double) second._iValue;
            case JSValueType.Double:
              return double.IsNaN(first._dValue) || double.IsNaN(second._dValue) ? moreOrEqual : first._dValue < second._dValue;
            case JSValueType.String:
              int index2 = 0;
              double num2 = 0.0;
              return Tools.ParseNumber(second._oValue.ToString(), ref index2, out num2) && index2 == second._oValue.ToString().Length ? first._dValue < num2 : moreOrEqual;
            case JSValueType.Object:
            case JSValueType.Date:
              second = second.ToPrimitiveValue_Value_String();
              if (second._valueType != JSValueType.Integer && second._valueType != JSValueType.Boolean)
              {
                if (second._valueType != JSValueType.Double)
                {
                  if (second._valueType != JSValueType.String)
                  {
                    if (second._valueType >= JSValueType.Object)
                      return first._dValue < 0.0;
                    throw new NotImplementedException();
                  }
                  goto case JSValueType.String;
                }
                else
                  goto case JSValueType.Double;
              }
              else
                goto case JSValueType.Boolean;
            default:
              return moreOrEqual;
          }
        case JSValueType.String:
          string str = first._oValue.ToString();
          switch (second._valueType)
          {
            case JSValueType.Boolean:
            case JSValueType.Integer:
              double num3 = 0.0;
              int index3 = 0;
              return Tools.ParseNumber(str, ref index3, out num3) && index3 == str.Length ? num3 < (double) second._iValue : moreOrEqual;
            case JSValueType.Double:
              double num4 = 0.0;
              int index4 = 0;
              return Tools.ParseNumber(str, ref index4, out num4) && index4 == str.Length ? num4 < second._dValue : moreOrEqual;
            case JSValueType.String:
              return string.CompareOrdinal(str, second._oValue.ToString()) < 0;
            case JSValueType.Object:
            case JSValueType.Function:
              second = second.ToPrimitiveValue_Value_String();
              switch (second._valueType)
              {
                case JSValueType.Boolean:
                case JSValueType.Integer:
                  double num5 = 0.0;
                  int index5 = 0;
                  if (Tools.ParseNumber(str, ref index5, out num5) && index5 == str.Length)
                    return num5 < (double) second._iValue;
                  goto case JSValueType.String;
                case JSValueType.Double:
                  double num6 = 0.0;
                  int index6 = 0;
                  if (Tools.ParseNumber(str, ref index6, out num6) && index6 == str.Length)
                    return num6 < second._dValue;
                  goto case JSValueType.String;
                case JSValueType.String:
                  return string.CompareOrdinal(str, second._oValue.ToString()) < 0;
                case JSValueType.Object:
                  double num7 = 0.0;
                  int index7 = 0;
                  return Tools.ParseNumber(str, ref index7, out num7) && index7 == str.Length ? num7 < 0.0 : moreOrEqual;
                default:
                  throw new NotImplementedException();
              }
            default:
              return moreOrEqual;
          }
        case JSValueType.Object:
        case JSValueType.Function:
        case JSValueType.Date:
          first = first.ToPrimitiveValue_Value_String();
          if (first._valueType != JSValueType.Integer && first._valueType != JSValueType.Boolean)
          {
            if (first._valueType != JSValueType.Double)
            {
              if (first._valueType != JSValueType.String)
              {
                if (first._valueType < JSValueType.Object)
                  throw new NotImplementedException();
                first._iValue = 0;
                goto case JSValueType.Boolean;
              }
              else
                goto case JSValueType.String;
            }
            else
              goto case JSValueType.Double;
          }
          else
            goto case JSValueType.Boolean;
        default:
          return moreOrEqual;
      }
    }

    public override JSValue Evaluate(Context context)
    {
      JSValue jsValue1 = this._left.Evaluate(context);
      JSValue jsValue2 = this._tempContainer;
      this._tempContainer = (JSValue) null;
      if (jsValue2 == null)
        jsValue2 = new JSValue()
        {
          _attributes = JSValueAttributesInternal.Temporary
        };
      jsValue2._valueType = jsValue1._valueType;
      jsValue2._iValue = jsValue1._iValue;
      jsValue2._dValue = jsValue1._dValue;
      jsValue2._oValue = jsValue1._oValue;
      JSValue second = this._right.Evaluate(context);
      this._tempContainer = jsValue2;
      if (jsValue2._valueType == JSValueType.Integer && second._valueType == JSValueType.Integer)
      {
        jsValue2._valueType = JSValueType.Boolean;
        jsValue2._iValue = jsValue2._iValue < second._iValue ? 1 : 0;
        return this._tempContainer;
      }
      if (this._tempContainer._valueType != JSValueType.Double || second._valueType != JSValueType.Double)
        return (JSValue) Less.Check(this._tempContainer, second, !this.trueLess);
      jsValue2._valueType = JSValueType.Boolean;
      jsValue2._iValue = double.IsNaN(jsValue2._dValue) || double.IsNaN(second._dValue) ? (this.trueLess ? 0 : 1) : (jsValue2._dValue < second._dValue ? 1 : 0);
      return this._tempContainer;
    }

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
      _this = (CodeNode) new NumberLess(this._left, this._right);
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => "(" + this._left?.ToString() + " < " + this._right?.ToString() + ")";
  }
}
