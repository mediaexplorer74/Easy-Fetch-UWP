// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.Call
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using NiL.JS.Core.Interop;
using NiL.JS.Statements;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NiL.JS.Expressions
{
  public sealed class Call : Expression
  {
    private Expression[] _arguments;
    internal bool withSpread;
    internal bool allowTCO;
    internal CallMode _callMode;

    public CallMode CallMode => this._callMode;

    protected internal override bool ContextIndependent => false;

    internal override bool ResultInTempContainer => false;

    protected internal override PredictedType ResultType => this._left is VariableReference && (this._left as VariableReference)._descriptor.initializer is FunctionDefinition initializer ? initializer._functionInfo.ResultType : PredictedType.Unknown;

    public Expression[] Arguments => this._arguments;

    public bool AllowTCO => this.allowTCO && this._callMode == CallMode.Regular;

    protected internal override bool NeedDecompose
    {
      get
      {
        if (this._left.NeedDecompose)
          return true;
        for (int index = 0; index < this._arguments.Length; ++index)
        {
          if (this._arguments[index].NeedDecompose)
            return true;
        }
        return false;
      }
    }

    internal Call(Expression first, Expression[] arguments)
      : base(first, (Expression) null, false)
    {
      this._arguments = arguments;
    }

    public override JSValue Evaluate(Context context)
    {
      JSValue jsValue = this._left.Evaluate(context);
      JSValue targetObject = context._objectSource;
      callable = (ICallable) null;
      Function func = (Function) null;
      if (jsValue._valueType >= JSValueType.Object)
      {
        if (jsValue._valueType == JSValueType.Function)
        {
          func = jsValue._oValue as Function;
          callable = (ICallable) func;
        }
        if (func == null)
        {
          if (!(jsValue._oValue is ICallable callable))
            callable = jsValue.Value as ICallable;
          if (callable == null && jsValue.Value is Proxy proxy)
            callable = proxy.PrototypeInstance as ICallable;
        }
      }
      if (callable == null)
      {
        for (int index = 0; index < this._arguments.Length; ++index)
        {
          context._objectSource = (JSValue) null;
          this._arguments[index].Evaluate(context);
        }
        context._objectSource = (JSValue) null;
        ExceptionHelper.ThrowTypeError(this._left.ToString() + " is not a function");
        return (JSValue) null;
      }
      if (func == null)
      {
        Call.checkStack(context);
        switch (this._callMode)
        {
          case CallMode.Construct:
            return callable.Construct(Tools.CreateArguments(this._arguments, context));
          case CallMode.Super:
            return callable.Construct(targetObject, Tools.CreateArguments(this._arguments, context));
          default:
            return callable.Call(targetObject, Tools.CreateArguments(this._arguments, context));
        }
      }
      else
      {
        if (this.allowTCO && this._callMode == CallMode.Regular && func._functionDefinition.kind != FunctionKind.Generator && func._functionDefinition.kind != FunctionKind.MethodGenerator && func._functionDefinition.kind != FunctionKind.AnonymousGenerator && context._owner != null && func == context._owner._oValue)
        {
          this.tailCall(context, func);
          context._objectSource = targetObject;
          return JSValue.undefined;
        }
        context._objectSource = (JSValue) null;
        Call.checkStack(context);
        if (this._callMode == CallMode.Construct)
          targetObject = (JSValue) null;
        return (jsValue._attributes & JSValueAttributesInternal.Eval) != JSValueAttributesInternal.None ? this.callEval(context) : func.InternalInvoke(targetObject, this._arguments, context, this.withSpread, this._callMode != 0);
      }
    }

    private JSValue callEval(Context context)
    {
      if (this._callMode != CallMode.Regular)
        ExceptionHelper.ThrowTypeError("function eval(...) cannot be called as a constructor");
      if (this._arguments == null || this._arguments.Length == 0)
        return JSValue.NotExists;
      JSValue jsValue = this._arguments[0].Evaluate(context);
      for (int index = 1; index < this._arguments.Length; ++index)
      {
        context._objectSource = (JSValue) null;
        this._arguments[index].Evaluate(context);
      }
      return jsValue._valueType != JSValueType.String ? jsValue : context.Eval(jsValue.ToString());
    }

    private void tailCall(Context context, Function func)
    {
      context._executionMode = ExecutionMode.TailRecursion;
      NiL.JS.Core.Arguments arguments = new NiL.JS.Core.Arguments(context);
      for (int index = 0; index < this._arguments.Length; ++index)
        arguments.Add(Tools.EvalExpressionSafe(context, this._arguments[index]));
      context._objectSource = (JSValue) null;
      arguments._callee = (JSValue) func;
      context._executionInfo = (JSValue) arguments;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void checkStack(Context context)
    {
      if (context._callDepth >= 1000)
        throw new JSException((Error) new RangeError("Stack overflow."));
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
      if (stats != null)
        stats.UseCall = true;
      this._codeContext = codeContext;
      if (this._left is Super left)
      {
        left.ctorMode = true;
        this._callMode = CallMode.Super;
      }
      for (int index = 0; index < this._arguments.Length; ++index)
        Parser.Build(ref this._arguments[index], expressionDepth + 1, variables, codeContext | CodeContext.InExpression, message, stats, opts);
      base.Build(ref _this, expressionDepth, variables, codeContext, message, stats, opts);
      if (this._left is Variable)
      {
        string key = this._left.ToString();
        if (key == "eval" && stats != null)
        {
          stats.ContainsEval = true;
          foreach (KeyValuePair<string, VariableDescriptor> variable in variables)
            variable.Value.captured = true;
        }
        VariableDescriptor variableDescriptor = (VariableDescriptor) null;
        if (variables.TryGetValue(key, out variableDescriptor) && variableDescriptor.initializer is FunctionDefinition initializer)
        {
          for (int index = 0; index < initializer.parameters.Length && index < this._arguments.Length; ++index)
          {
            if (initializer.parameters[index].lastPredictedType == PredictedType.Unknown)
              initializer.parameters[index].lastPredictedType = this._arguments[index].ResultType;
            else if (Tools.CompareWithMask((Enum) initializer.parameters[index].lastPredictedType, (Enum) this._arguments[index].ResultType, (Enum) PredictedType.Group) != 0)
              initializer.parameters[index].lastPredictedType = PredictedType.Ambiguous;
          }
        }
      }
      return false;
    }

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      base.Optimize(ref _this, owner, message, opts, stats);
      int length = this._arguments.Length;
      while (length-- > 0)
      {
        CodeNode _this1 = (CodeNode) this._arguments[length];
        _this1.Optimize(ref _this1, owner, message, opts, stats);
        this._arguments[length] = _this1 as Expression;
      }
    }

    protected internal override CodeNode[] GetChildrenImpl()
    {
      CodeNode[] childrenImpl = new CodeNode[this._arguments.Length + 1];
      childrenImpl[0] = (CodeNode) this._left;
      this._arguments.CopyTo((System.Array) childrenImpl, 1);
      return childrenImpl;
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override void Decompose(ref Expression self, IList<CodeNode> result)
    {
      this._left.Decompose(ref this._left, result);
      int num = -1;
      for (int index = 0; index < this._arguments.Length; ++index)
      {
        this._arguments[index].Decompose(ref this._arguments[index], result);
        if (this._arguments[index].NeedDecompose)
          num = index;
      }
      for (int index = 0; index < num; ++index)
      {
        if (!(this._arguments[index] is ExtractStoredValue))
        {
          result.Add((CodeNode) new StoreValue(this._arguments[index], false));
          this._arguments[index] = (Expression) new ExtractStoredValue(this._arguments[index]);
        }
      }
    }

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
      base.RebuildScope(functionInfo, transferedVariables, scopeBias);
      for (int index = 0; index < this._arguments.Length; ++index)
        this._arguments[index].RebuildScope(functionInfo, transferedVariables, scopeBias);
    }

    public override string ToString()
    {
      string str1 = this._left?.ToString() + "(";
      for (int index = 0; index < this._arguments.Length; ++index)
      {
        str1 += this._arguments[index]?.ToString();
        if (index + 1 < this._arguments.Length)
          str1 += ", ";
      }
      string str2 = str1 + ")";
      return this._callMode == CallMode.Construct ? "new " + str2 : str2;
    }
  }
}
