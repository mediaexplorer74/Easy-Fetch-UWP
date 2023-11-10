// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.Addition
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace NiL.JS.Expressions
{
  public sealed class Addition : Expression
  {
    protected internal override PredictedType ResultType
    {
      get
      {
        PredictedType resultType1 = this._left.ResultType;
        PredictedType resultType2 = this._right.ResultType;
        if (resultType1 == PredictedType.String || resultType2 == PredictedType.String)
          return PredictedType.String;
        if (resultType1 == resultType2)
        {
          if (resultType1 == PredictedType.Bool || resultType1 == PredictedType.Int)
            return PredictedType.Number;
          if (resultType1 == PredictedType.Double)
            return PredictedType.Double;
        }
        if (resultType1 == PredictedType.Bool)
        {
          if (resultType2 == PredictedType.Double)
            return PredictedType.Double;
          if (Tools.IsEqual((Enum) resultType2, (Enum) PredictedType.Number, (Enum) PredictedType.Group))
            return PredictedType.Number;
        }
        if (resultType2 == PredictedType.Bool)
        {
          if (resultType1 == PredictedType.Double)
            return PredictedType.Double;
          if (Tools.IsEqual((Enum) resultType1, (Enum) PredictedType.Number, (Enum) PredictedType.Group))
            return PredictedType.Number;
        }
        return PredictedType.Unknown;
      }
    }

    internal override bool ResultInTempContainer => false;

    public Addition(Expression first, Expression second)
      : base(first, second, true)
    {
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
      Addition.Impl(jsValue2, jsValue2, this._right.Evaluate(context));
      this._tempContainer = jsValue2;
      return jsValue2;
    }

    internal static void Impl(JSValue resultContainer, JSValue first, JSValue second)
    {
      switch (first._valueType)
      {
        case JSValueType.NotExists:
        case JSValueType.NotExistsInObject:
        case JSValueType.Undefined:
          if (second._valueType >= JSValueType.Object)
            second = second.ToPrimitiveValue_Value_String();
          switch (second._valueType)
          {
            case JSValueType.NotExists:
            case JSValueType.NotExistsInObject:
            case JSValueType.Undefined:
            case JSValueType.Object:
              resultContainer._valueType = JSValueType.Double;
              resultContainer._dValue = double.NaN;
              return;
            case JSValueType.Boolean:
            case JSValueType.Integer:
            case JSValueType.Double:
              resultContainer._valueType = JSValueType.Double;
              resultContainer._dValue = double.NaN;
              return;
            case JSValueType.String:
              resultContainer._valueType = JSValueType.String;
              resultContainer._oValue = (object) new RopeString((object) "undefined", second._oValue);
              return;
            default:
              return;
          }
        case JSValueType.Boolean:
        case JSValueType.Integer:
          if (second._valueType >= JSValueType.Object)
            second = second.ToPrimitiveValue_Value_String();
          switch (second._valueType)
          {
            case JSValueType.NotExists:
            case JSValueType.NotExistsInObject:
            case JSValueType.Undefined:
              resultContainer._dValue = double.NaN;
              resultContainer._valueType = JSValueType.Double;
              return;
            case JSValueType.Boolean:
            case JSValueType.Integer:
              long num = (long) first._iValue + (long) second._iValue;
              if ((long) (int) num == num)
              {
                resultContainer._valueType = JSValueType.Integer;
                resultContainer._iValue = (int) num;
                return;
              }
              resultContainer._valueType = JSValueType.Double;
              resultContainer._dValue = (double) num;
              return;
            case JSValueType.Double:
              resultContainer._valueType = JSValueType.Double;
              resultContainer._dValue = (double) first._iValue + second._dValue;
              return;
            case JSValueType.String:
              resultContainer._oValue = (object) new RopeString(first._valueType == JSValueType.Boolean ? (first._iValue != 0 ? (object) "true" : (object) "false") : (object) first._iValue.ToString((IFormatProvider) CultureInfo.InvariantCulture), second._oValue);
              resultContainer._valueType = JSValueType.String;
              return;
            case JSValueType.Object:
              resultContainer._iValue = first._iValue;
              resultContainer._valueType = JSValueType.Integer;
              return;
            default:
              return;
          }
        case JSValueType.Double:
          if (second._valueType >= JSValueType.Object)
            second = second.ToPrimitiveValue_Value_String();
          switch (second._valueType)
          {
            case JSValueType.NotExists:
            case JSValueType.NotExistsInObject:
            case JSValueType.Undefined:
              resultContainer._dValue = double.NaN;
              resultContainer._valueType = JSValueType.Double;
              return;
            case JSValueType.Boolean:
            case JSValueType.Integer:
              resultContainer._valueType = JSValueType.Double;
              resultContainer._dValue = first._dValue + (double) second._iValue;
              return;
            case JSValueType.Double:
              resultContainer._valueType = JSValueType.Double;
              resultContainer._dValue = first._dValue + second._dValue;
              return;
            case JSValueType.String:
              resultContainer._oValue = (object) new RopeString((object) Tools.DoubleToString(first._dValue), second._oValue);
              resultContainer._valueType = JSValueType.String;
              return;
            case JSValueType.Object:
              resultContainer._dValue = first._dValue;
              resultContainer._valueType = JSValueType.Double;
              return;
            default:
              return;
          }
        case JSValueType.String:
          object firstSource = first._oValue;
          switch (second._valueType)
          {
            case JSValueType.NotExists:
            case JSValueType.NotExistsInObject:
            case JSValueType.Undefined:
              firstSource = (object) new RopeString(firstSource, (object) "undefined");
              break;
            case JSValueType.Boolean:
              firstSource = (object) new RopeString(firstSource, second._iValue != 0 ? (object) "true" : (object) "false");
              break;
            case JSValueType.Integer:
              firstSource = (object) new RopeString(firstSource, (object) second._iValue.ToString((IFormatProvider) CultureInfo.InvariantCulture));
              break;
            case JSValueType.Double:
              firstSource = (object) new RopeString(firstSource, (object) Tools.DoubleToString(second._dValue));
              break;
            case JSValueType.String:
              firstSource = (object) new RopeString(firstSource, second._oValue);
              break;
            case JSValueType.Object:
            case JSValueType.Function:
              firstSource = (object) new RopeString(firstSource, (object) second.ToPrimitiveValue_Value_String().BaseToString());
              break;
            case JSValueType.Date:
              firstSource = (object) new RopeString(firstSource, (object) second.ToPrimitiveValue_String_Value().BaseToString());
              break;
          }
          resultContainer._oValue = firstSource;
          resultContainer._valueType = JSValueType.String;
          break;
        case JSValueType.Object:
        case JSValueType.Function:
          first = first.ToPrimitiveValue_Value_String();
          if (first._valueType != JSValueType.Integer && first._valueType != JSValueType.Boolean)
          {
            if (first._valueType == JSValueType.Object)
            {
              if (second._valueType >= JSValueType.String)
                second = second.ToPrimitiveValue_Value_String();
              if (second._valueType == JSValueType.String)
              {
                resultContainer._oValue = (object) new RopeString((object) "null", second._oValue);
                resultContainer._valueType = JSValueType.String;
                break;
              }
              first._iValue = 0;
              goto case JSValueType.Boolean;
            }
            else if (first._valueType != JSValueType.Double)
            {
              if (first._valueType != JSValueType.String)
                break;
              goto case JSValueType.String;
            }
            else
              goto case JSValueType.Double;
          }
          else
            goto case JSValueType.Boolean;
        case JSValueType.Date:
          first = first.ToPrimitiveValue_String_Value();
          Addition.Impl(resultContainer, first, second);
          break;
      }
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
      if (!flag && _this == this)
      {
        if (this._left is StringConcatenation)
        {
          _this = (CodeNode) this._left;
          Expression[] parts = (this._left as StringConcatenation)._parts;
          Array.Resize<Expression>(ref parts, parts.Length + 1);
          parts[parts.Length - 1] = this._right;
          (this._left as StringConcatenation)._parts = parts;
        }
        else if (this._right is StringConcatenation)
        {
          _this = (CodeNode) this._right;
          Expression[] parts = (this._right as StringConcatenation)._parts;
          Array.Resize<Expression>(ref parts, parts.Length + 1);
          Array.Copy((Array) parts, 0, (Array) parts, 1, parts.Length - 1);
          parts[0] = this._left;
          (this._right as StringConcatenation)._parts = parts;
        }
        else if (this._left.ContextIndependent && this._left.ResultType == PredictedType.String)
        {
          if (this._left.Evaluate((Context) null).ToString().Length == 0)
            _this = (CodeNode) new ConvertToString(this._right);
          else
            _this = (CodeNode) new StringConcatenation(new Expression[2]
            {
              this._left,
              this._right
            });
        }
        else if (this._right.ContextIndependent && this._right.ResultType == PredictedType.String)
        {
          if (this._right.Evaluate((Context) null).ToString().Length == 0)
            _this = (CodeNode) new ConvertToString(this._left);
          else
            _this = (CodeNode) new StringConcatenation(new Expression[2]
            {
              this._left,
              this._right
            });
        }
        else if (this._left.ResultType == PredictedType.String || this._right.ResultType == PredictedType.String)
          _this = (CodeNode) new StringConcatenation(new Expression[2]
          {
            this._left,
            this._right
          });
      }
      return flag;
    }

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      base.Optimize(ref _this, owner, message, opts, stats);
      if (!Tools.IsEqual((Enum) this._left.ResultType, (Enum) PredictedType.Number, (Enum) PredictedType.Group) || !Tools.IsEqual((Enum) this._right.ResultType, (Enum) PredictedType.Number, (Enum) PredictedType.Group))
        return;
      _this = (CodeNode) new NumberAddition(this._left, this._right);
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => "(" + this._left?.ToString() + " + " + this._right?.ToString() + ")";
  }
}
