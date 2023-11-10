// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.Equal
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using System;

namespace NiL.JS.Expressions
{
  public class Equal : Expression
  {
    protected internal override PredictedType ResultType => PredictedType.Bool;

    internal override bool ResultInTempContainer => false;

    public Equal(Expression first, Expression second)
      : base(first, second, false)
    {
    }

    public override JSValue Evaluate(Context context)
    {
      JSValue jsValue1 = this._left.Evaluate(context);
      int index = 0;
      switch (jsValue1._valueType)
      {
        case JSValueType.NotExists:
        case JSValueType.NotExistsInObject:
        case JSValueType.Undefined:
          JSValue jsValue2 = this._right.Evaluate(context);
          return jsValue2._valueType == JSValueType.Object ? (jsValue2._oValue != null ? (JSValue) NiL.JS.BaseLibrary.Boolean.False : (JSValue) NiL.JS.BaseLibrary.Boolean.True) : (jsValue2.Defined ? (JSValue) NiL.JS.BaseLibrary.Boolean.False : (JSValue) NiL.JS.BaseLibrary.Boolean.True);
        case JSValueType.Boolean:
        case JSValueType.Integer:
          int iValue = jsValue1._iValue;
          JSValue valueValueString1 = this._right.Evaluate(context);
          switch (valueValueString1._valueType)
          {
            case JSValueType.Boolean:
            case JSValueType.Integer:
              return iValue != valueValueString1._iValue ? (JSValue) NiL.JS.BaseLibrary.Boolean.False : (JSValue) NiL.JS.BaseLibrary.Boolean.True;
            case JSValueType.Double:
              return (double) iValue != valueValueString1._dValue ? (JSValue) NiL.JS.BaseLibrary.Boolean.False : (JSValue) NiL.JS.BaseLibrary.Boolean.True;
            case JSValueType.String:
              string code1 = valueValueString1._oValue.ToString();
              double num1;
              if (!Tools.ParseNumber(code1, ref index, out num1) || index != code1.Length)
                return (JSValue) false;
              return (double) iValue != num1 ? (JSValue) NiL.JS.BaseLibrary.Boolean.False : (JSValue) NiL.JS.BaseLibrary.Boolean.True;
            case JSValueType.Object:
            case JSValueType.Date:
              valueValueString1 = valueValueString1.ToPrimitiveValue_Value_String();
              if (valueValueString1._valueType != JSValueType.Integer && valueValueString1._valueType != JSValueType.Boolean)
              {
                if (valueValueString1._valueType != JSValueType.Double)
                {
                  if (valueValueString1._valueType != JSValueType.String)
                  {
                    if (valueValueString1._valueType >= JSValueType.Object)
                      return (JSValue) false;
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
              return (JSValue) NiL.JS.BaseLibrary.Boolean.False;
          }
        case JSValueType.Double:
          double dValue = jsValue1._dValue;
          JSValue valueValueString2 = this._right.Evaluate(context);
          switch (valueValueString2._valueType)
          {
            case JSValueType.Boolean:
            case JSValueType.Integer:
              return dValue != (double) valueValueString2._iValue ? (JSValue) NiL.JS.BaseLibrary.Boolean.False : (JSValue) NiL.JS.BaseLibrary.Boolean.True;
            case JSValueType.Double:
              return dValue != valueValueString2._dValue ? (JSValue) NiL.JS.BaseLibrary.Boolean.False : (JSValue) NiL.JS.BaseLibrary.Boolean.True;
            case JSValueType.String:
              string code2 = valueValueString2._oValue.ToString();
              if (!Tools.ParseNumber(code2, ref index, out valueValueString2._dValue) || index != code2.Length)
                return (JSValue) false;
              return dValue != valueValueString2._dValue ? (JSValue) NiL.JS.BaseLibrary.Boolean.False : (JSValue) NiL.JS.BaseLibrary.Boolean.True;
            case JSValueType.Object:
            case JSValueType.Date:
              valueValueString2 = valueValueString2.ToPrimitiveValue_Value_String();
              if (valueValueString2._valueType != JSValueType.Integer && valueValueString2._valueType != JSValueType.Boolean)
              {
                if (valueValueString2._valueType != JSValueType.Double)
                {
                  if (valueValueString2._valueType != JSValueType.String)
                  {
                    if (valueValueString2._valueType >= JSValueType.Object)
                      return (JSValue) (dValue == 0.0 && !Tools.IsNegativeZero(dValue));
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
              return (JSValue) false;
          }
        case JSValueType.String:
          string str = jsValue1._oValue.ToString();
          JSValue jsValue3 = this._right.Evaluate(context);
          switch (jsValue3._valueType)
          {
            case JSValueType.Boolean:
            case JSValueType.Integer:
              double num2;
              if (!Tools.ParseNumber(str, ref index, out num2) || index != str.Length)
                return (JSValue) false;
              return num2 != (double) jsValue3._iValue ? (JSValue) NiL.JS.BaseLibrary.Boolean.False : (JSValue) NiL.JS.BaseLibrary.Boolean.True;
            case JSValueType.Double:
              double num3;
              if (!Tools.ParseNumber(str, ref index, out num3) || index != str.Length)
                return (JSValue) false;
              return num3 != jsValue3._dValue ? (JSValue) NiL.JS.BaseLibrary.Boolean.False : (JSValue) NiL.JS.BaseLibrary.Boolean.True;
            case JSValueType.String:
              return string.CompareOrdinal(str, jsValue3._oValue.ToString()) != 0 ? (JSValue) NiL.JS.BaseLibrary.Boolean.False : (JSValue) NiL.JS.BaseLibrary.Boolean.True;
            case JSValueType.Object:
            case JSValueType.Function:
              JSValue valueValueString3 = jsValue3.ToPrimitiveValue_Value_String();
              switch (valueValueString3._valueType)
              {
                case JSValueType.Boolean:
                case JSValueType.Integer:
                  double num4;
                  if (Tools.ParseNumber(str, ref index, out num4) && index == str.Length)
                    return num4 != (double) valueValueString3._iValue ? (JSValue) NiL.JS.BaseLibrary.Boolean.False : (JSValue) NiL.JS.BaseLibrary.Boolean.True;
                  goto case JSValueType.String;
                case JSValueType.Double:
                  double num5;
                  if (Tools.ParseNumber(str, ref index, out num5) && index == str.Length)
                    return num5 != valueValueString3._dValue ? (JSValue) NiL.JS.BaseLibrary.Boolean.False : (JSValue) NiL.JS.BaseLibrary.Boolean.True;
                  goto case JSValueType.String;
                case JSValueType.String:
                  return string.CompareOrdinal(str, valueValueString3._oValue.ToString()) != 0 ? (JSValue) NiL.JS.BaseLibrary.Boolean.False : (JSValue) NiL.JS.BaseLibrary.Boolean.True;
              }
              break;
          }
          return (JSValue) false;
        case JSValueType.Symbol:
        case JSValueType.Object:
        case JSValueType.Function:
        case JSValueType.Date:
          if (this._tempContainer == null)
            this._tempContainer = new JSValue()
            {
              _attributes = JSValueAttributesInternal.Temporary
            };
          this._tempContainer.Assign(jsValue1);
          JSValue tempContainer = this._tempContainer;
          JSValue jsValue4 = this._right.Evaluate(context);
          switch (jsValue4._valueType)
          {
            case JSValueType.Boolean:
            case JSValueType.Integer:
            case JSValueType.Double:
              double num6 = jsValue4._valueType == JSValueType.Double ? jsValue4._dValue : (double) jsValue4._iValue;
              JSValue valueValueString4 = tempContainer.ToPrimitiveValue_Value_String();
              switch (valueValueString4._valueType)
              {
                case JSValueType.Boolean:
                case JSValueType.Integer:
                  return (double) valueValueString4._iValue != num6 ? (JSValue) NiL.JS.BaseLibrary.Boolean.False : (JSValue) NiL.JS.BaseLibrary.Boolean.True;
                case JSValueType.Double:
                  return valueValueString4._dValue != num6 ? (JSValue) NiL.JS.BaseLibrary.Boolean.False : (JSValue) NiL.JS.BaseLibrary.Boolean.True;
                case JSValueType.String:
                  string code3 = valueValueString4._oValue.ToString();
                  if (!Tools.ParseNumber(code3, ref index, out valueValueString4._dValue) || index != code3.Length)
                    return (JSValue) false;
                  return num6 != valueValueString4._dValue ? (JSValue) NiL.JS.BaseLibrary.Boolean.False : (JSValue) NiL.JS.BaseLibrary.Boolean.True;
                default:
                  return (JSValue) false;
              }
            case JSValueType.String:
              string code4 = jsValue4._oValue.ToString();
              JSValue valueValueString5 = tempContainer.ToPrimitiveValue_Value_String();
              switch (valueValueString5._valueType)
              {
                case JSValueType.Boolean:
                case JSValueType.Integer:
                case JSValueType.Double:
                  valueValueString5._dValue = valueValueString5._valueType == JSValueType.Double ? valueValueString5._dValue : (double) valueValueString5._iValue;
                  double num7;
                  if (!Tools.ParseNumber(code4, ref index, out num7) || index != code4.Length)
                    return (JSValue) false;
                  return num7 != valueValueString5._dValue ? (JSValue) NiL.JS.BaseLibrary.Boolean.False : (JSValue) NiL.JS.BaseLibrary.Boolean.True;
                case JSValueType.String:
                  return !(valueValueString5._oValue.ToString() == code4) ? (JSValue) NiL.JS.BaseLibrary.Boolean.False : (JSValue) NiL.JS.BaseLibrary.Boolean.True;
                default:
                  return (JSValue) false;
              }
            default:
              return tempContainer._oValue != jsValue4._oValue ? (JSValue) NiL.JS.BaseLibrary.Boolean.False : (JSValue) NiL.JS.BaseLibrary.Boolean.True;
          }
        default:
          throw new NotImplementedException();
      }
    }

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      base.Optimize(ref _this, owner, message, opts, stats);
      if (message == null)
        return;
      if (!(this._left is Constant constant1))
        constant1 = this._right as Constant;
      Constant constant2 = constant1;
      if (constant2 == null)
        return;
      switch (constant2.value._valueType)
      {
        case JSValueType.Undefined:
          message(MessageLevel.Warning, this.Position, this.Length, "To compare with undefined use '===' or '!==' instead of '==' or '!='.");
          break;
        case JSValueType.Object:
          if (constant2.value._oValue != null)
            break;
          message(MessageLevel.Warning, this.Position, this.Length, "To compare with null use '===' or '!==' instead of '==' or '!='.");
          break;
      }
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => "(" + this._left?.ToString() + " == " + this._right?.ToString() + ")";
  }
}
