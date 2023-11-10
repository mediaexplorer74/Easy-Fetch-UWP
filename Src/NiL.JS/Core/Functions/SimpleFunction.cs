// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Functions.SimpleFunction
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core.Interop;
using NiL.JS.Expressions;
using NiL.JS.Statements;
using System;

namespace NiL.JS.Core.Functions
{
  [Prototype(typeof (Function), true)]
  internal sealed class SimpleFunction : Function
  {
    internal SimpleFunction(Context context, FunctionDefinition creator)
      : base(context, creator)
    {
    }

    internal override JSValue InternalInvoke(
      JSValue targetObject,
      Expression[] arguments,
      Context initiator,
      bool withSpread,
      bool construct)
    {
      if (construct | withSpread)
        return base.InternalInvoke(targetObject, arguments, initiator, withSpread, construct);
      CodeBlock body = this._functionDefinition._body;
      JSValue notExists = JSValue.notExists;
      JSValue.notExists._valueType = JSValueType.NotExists;
      return this._functionDefinition.parameters.Length == arguments.Length && arguments.Length < 9 ? this.fastInvoke(targetObject, arguments, initiator) : base.InternalInvoke(targetObject, arguments, initiator, withSpread, construct);
    }

    private JSValue fastInvoke(JSValue targetObject, Expression[] arguments, Context initiator)
    {
      CodeBlock body = this._functionDefinition._body;
      targetObject = this.correctTargetObject(targetObject, body._strict);
      if (this._functionDefinition.recursionDepth > this._functionDefinition.parametersStored)
      {
        this.storeParameters();
        ++this._functionDefinition.parametersStored;
      }
      JSValue jsValue = (JSValue) null;
      Arguments args = (Arguments) null;
      bool flag = false;
      while (true)
      {
        Context internalContext = new Context(this._initialContext, false, (Function) this);
        Context context = internalContext;
        Context currentContext = Context.CurrentContext;
        int num = (currentContext != null ? currentContext._callDepth : 0) + 1;
        context._callDepth = num;
        internalContext._thisBind = this._functionDefinition.kind != FunctionKind.Arrow ? targetObject : this._initialContext._thisBind;
        if (flag)
          this.initParameters(args, this._functionDefinition._functionInfo.ContainsEval || this._functionDefinition._functionInfo.ContainsWith || this._functionDefinition._functionInfo.ContainsDebugger || this._functionDefinition._functionInfo.NeedDecompose || internalContext != null && internalContext._debugging, internalContext);
        else
          this.initParametersFast(arguments, initiator, internalContext);
        ++this._functionDefinition.recursionDepth;
        this.initCachedReference(internalContext);
        internalContext._strict |= body._strict;
        internalContext.Activate();
        try
        {
          jsValue = this.evaluateBody(internalContext);
          if (internalContext._executionMode == ExecutionMode.TailRecursion)
          {
            flag = true;
            args = internalContext._executionInfo as Arguments;
          }
          else
            flag = false;
        }
        finally
        {
          --this._functionDefinition.recursionDepth;
          if (this._functionDefinition.parametersStored > this._functionDefinition.recursionDepth)
            --this._functionDefinition.parametersStored;
          this.exit(internalContext);
        }
        if (flag)
          targetObject = this.correctTargetObject(internalContext._objectSource, body._strict);
        else
          break;
      }
      return jsValue;
    }

    private void initParametersFast(
      Expression[] arguments,
      Context initiator,
      Context internalContext)
    {
      JSValue jsValue1 = (JSValue) null;
      JSValue jsValue2 = (JSValue) null;
      JSValue jsValue3 = (JSValue) null;
      JSValue jsValue4 = (JSValue) null;
      JSValue jsValue5 = (JSValue) null;
      JSValue jsValue6 = (JSValue) null;
      JSValue jsValue7 = (JSValue) null;
      int length = arguments.Length;
      if (this._functionDefinition.parameters.Length != length)
        throw new ArgumentException("Invalid arguments count");
      if (length > 8)
        throw new ArgumentException("To many arguments");
      if (length == 0)
        return;
      JSValue jsValue8 = Tools.EvalExpressionSafe(initiator, arguments[0]);
      if (length > 1)
      {
        jsValue1 = Tools.EvalExpressionSafe(initiator, arguments[1]);
        if (length > 2)
        {
          jsValue2 = Tools.EvalExpressionSafe(initiator, arguments[2]);
          if (length > 3)
          {
            jsValue3 = Tools.EvalExpressionSafe(initiator, arguments[3]);
            if (length > 4)
            {
              jsValue4 = Tools.EvalExpressionSafe(initiator, arguments[4]);
              if (length > 5)
              {
                jsValue5 = Tools.EvalExpressionSafe(initiator, arguments[5]);
                if (length > 6)
                {
                  jsValue6 = Tools.EvalExpressionSafe(initiator, arguments[6]);
                  if (length > 7)
                    jsValue7 = Tools.EvalExpressionSafe(initiator, arguments[7]);
                }
              }
            }
          }
        }
      }
      this.setParamValue(0, jsValue8, internalContext);
      if (length <= 1)
        return;
      this.setParamValue(1, jsValue1, internalContext);
      if (length <= 2)
        return;
      this.setParamValue(2, jsValue2, internalContext);
      if (length <= 3)
        return;
      this.setParamValue(3, jsValue3, internalContext);
      if (length <= 4)
        return;
      this.setParamValue(4, jsValue4, internalContext);
      if (length <= 5)
        return;
      this.setParamValue(5, jsValue5, internalContext);
      if (length <= 6)
        return;
      this.setParamValue(6, jsValue6, internalContext);
      if (length <= 7)
        return;
      this.setParamValue(7, jsValue7, internalContext);
    }

    private void setParamValue(int index, JSValue value, Context context)
    {
      if (this._functionDefinition.parameters[index].assignments != null)
      {
        value = value.CloneImpl(false);
        value._attributes |= JSValueAttributesInternal.Argument;
      }
      else
        value._attributes &= ~JSValueAttributesInternal.Cloned;
      if (!value.Defined && this._functionDefinition.parameters.Length > index && this._functionDefinition.parameters[index].initializer != null)
        value.Assign(this._functionDefinition.parameters[index].initializer.Evaluate(context));
      this._functionDefinition.parameters[index].cacheRes = value;
      this._functionDefinition.parameters[index].cacheContext = context;
      if (!this._functionDefinition.parameters[index].captured)
        return;
      if (context._variables == null)
        context._variables = JSObject.getFieldsContainer();
      context._variables[this._functionDefinition.parameters[index].name] = value;
    }
  }
}
