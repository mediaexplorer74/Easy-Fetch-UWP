// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.Decrement
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using System;
using System.Collections.Generic;

namespace NiL.JS.Expressions
{
  public sealed class Decrement : Expression
  {
    private DecrimentType _type;

    public DecrimentType Type => this._type;

    protected internal override bool ContextIndependent => false;

    internal override bool ResultInTempContainer => this._tempContainer != null;

    protected internal override PredictedType ResultType
    {
      get
      {
        PredictedType resultType = this._left.ResultType;
        if (this._tempContainer != null)
          return resultType;
        return resultType == PredictedType.Double ? PredictedType.Double : PredictedType.Number;
      }
    }

    protected internal override bool LValueModifier => true;

    public Decrement(Expression op, DecrimentType type)
      : base(op, (Expression) null, type == DecrimentType.Postdecriment)
    {
      if (op == null)
        throw new ArgumentNullException(nameof (op));
      this._type = type;
    }

    public override JSValue Evaluate(Context context)
    {
      bool flag = this._type == DecrimentType.Postdecriment;
      Function function = (Function) null;
      JSValue result = this._left.EvaluateForWrite(context);
      if (result._valueType == JSValueType.Property)
      {
        NiL.JS.Core.PropertyPair oValue = result._oValue as NiL.JS.Core.PropertyPair;
        function = oValue.setter;
        if (context._strict && function == null)
          this.raiseErrorProp();
        result = oValue.getter != null ? oValue.getter.Call(context._objectSource, (Arguments) null).CloneImpl(~JSValueAttributesInternal.None) : JSValue.undefined.CloneImpl(~JSValueAttributesInternal.None);
      }
      else if ((result._attributes & JSValueAttributesInternal.ReadOnly) != JSValueAttributesInternal.None)
      {
        if (context._strict)
          this.raiseErrorValue();
        result = result.CloneImpl(false);
      }
      switch (result._valueType)
      {
        case JSValueType.NotExists:
          ExceptionHelper.ThrowIfNotExists<JSValue>(result, (object) this._left);
          break;
        case JSValueType.Boolean:
          result._valueType = JSValueType.Integer;
          break;
        case JSValueType.String:
          Tools.JSObjectToNumber(result, result);
          break;
        case JSValueType.Object:
        case JSValueType.Function:
        case JSValueType.Date:
          result.Assign(result.ToPrimitiveValue_Value_String());
          switch (result._valueType)
          {
            case JSValueType.Boolean:
              result._valueType = JSValueType.Integer;
              break;
            case JSValueType.String:
              Tools.JSObjectToNumber(result, result);
              break;
            case JSValueType.Object:
            case JSValueType.Function:
            case JSValueType.Date:
              result._valueType = JSValueType.Integer;
              result._iValue = 0;
              break;
          }
          break;
      }
      JSValue jsValue;
      if (flag && result.Defined)
      {
        jsValue = this._tempContainer;
        jsValue.Assign(result);
      }
      else
        jsValue = result;
      switch (result._valueType)
      {
        case JSValueType.NotExists:
        case JSValueType.NotExistsInObject:
        case JSValueType.Undefined:
          result._valueType = JSValueType.Double;
          result._dValue = double.NaN;
          break;
        case JSValueType.Integer:
          if (result._iValue == int.MinValue)
          {
            result._valueType = JSValueType.Double;
            result._dValue = (double) result._iValue - 1.0;
            break;
          }
          --result._iValue;
          break;
        case JSValueType.Double:
          --result._dValue;
          break;
      }
      if (function != null)
      {
        Arguments arguments = new Arguments(context);
        arguments._iValue = 1;
        arguments[0] = result;
        function.Call(context._objectSource, arguments);
      }
      else if ((result._attributes & JSValueAttributesInternal.Reassign) != JSValueAttributesInternal.None)
        result.Assign(result);
      return jsValue;
    }

    private void raiseErrorValue() => ExceptionHelper.Throw((Error) new TypeError("Can not decrement readonly \"" + this._left?.ToString() + "\""));

    private void raiseErrorProp() => ExceptionHelper.Throw((Error) new TypeError("Can not decrement property \"" + this._left?.ToString() + "\" without setter."));

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
      Parser.Build(ref this._left, expressionDepth + 1, variables, codeContext | CodeContext.InExpression, message, stats, opts);
      if (expressionDepth <= 1 && this._type == DecrimentType.Postdecriment)
        this._type = DecrimentType.Predecriment;
      if (!(this._left is VariableReference variableReference1))
        variableReference1 = this._left is AssignmentOperatorCache ? (this._left as AssignmentOperatorCache).Source as VariableReference : (VariableReference) null;
      VariableReference variableReference2 = variableReference1;
      if (variableReference2 != null)
        (variableReference2.Descriptor.assignments ?? (variableReference2.Descriptor.assignments = new List<Expression>())).Add((Expression) this);
      return false;
    }

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      if (!(this._left is VariableReference left) || !left._descriptor.IsDefined)
        return;
      switch (left._descriptor.lastPredictedType)
      {
        case PredictedType.Unknown:
        case PredictedType.Int:
          left._descriptor.lastPredictedType = PredictedType.Number;
          break;
        case PredictedType.Double:
          break;
        default:
          left._descriptor.lastPredictedType = PredictedType.Ambiguous;
          break;
      }
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => this._type != DecrimentType.Predecriment ? this._left?.ToString() + "--" : "--" + this._left?.ToString();
  }
}
