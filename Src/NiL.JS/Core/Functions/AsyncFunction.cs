// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Functions.AsyncFunction
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core.Interop;
using NiL.JS.Expressions;
using System;

namespace NiL.JS.Core.Functions
{
  [Prototype(typeof (Function), true)]
  internal sealed class AsyncFunction : Function
  {
    public override JSValue prototype
    {
      get => (JSValue) null;
      set
      {
      }
    }

    public AsyncFunction(Context context, FunctionDefinition implementation)
      : base(context, implementation)
    {
      this.RequireNewKeywordLevel = RequireNewKeywordLevel.WithoutNewOnly;
    }

    protected internal override JSValue Invoke(
      bool construct,
      JSValue targetObject,
      Arguments arguments)
    {
      if (construct)
        ExceptionHelper.ThrowTypeError("Async function cannot be invoked as a constructor");
      if (this._functionDefinition._body._lines.Length == 0)
      {
        JSValue.notExists._valueType = JSValueType.NotExists;
        return JSValue.notExists;
      }
      if (arguments == null)
        arguments = new Arguments(Context.CurrentContext);
      Context internalContext = new Context(this._initialContext, true, (Function) this);
      Context context = internalContext;
      Context currentContext = Context.CurrentContext;
      int num = (currentContext != null ? currentContext._callDepth : 0) + 1;
      context._callDepth = num;
      internalContext._definedVariables = this.Body._variables;
      this.initContext(targetObject, arguments, this._functionDefinition._functionInfo.ContainsArguments, internalContext);
      this.initParameters(arguments, this._functionDefinition._functionInfo.ContainsEval || this._functionDefinition._functionInfo.ContainsWith || this._functionDefinition._functionInfo.ContainsDebugger || this._functionDefinition._functionInfo.NeedDecompose || internalContext != null && internalContext._debugging, internalContext);
      JSValue result = this.run(internalContext);
      return this.processSuspend(internalContext, result);
    }

    private JSValue processSuspend(Context internalContext, JSValue result)
    {
      if (internalContext._executionMode == ExecutionMode.Suspend)
      {
        JSValue executionInfo = internalContext._executionInfo;
        AsyncFunction.Сontinuator сontinuator = new AsyncFunction.Сontinuator(this, internalContext);
        сontinuator.Build(executionInfo);
        result = сontinuator.ResultPromise;
      }
      else
        result = JSValue.Marshal((object) Promise.resolve(result));
      return result;
    }

    private JSValue run(Context internalContext)
    {
      internalContext.Activate();
      try
      {
        return this.evaluateBody(internalContext);
      }
      catch (JSException ex)
      {
        throw;
      }
      finally
      {
        internalContext.Deactivate();
      }
    }

    private sealed class Сontinuator
    {
      private readonly AsyncFunction _asyncFunction;
      private readonly Context _context;

      public JSValue ResultPromise { get; private set; }

      public Сontinuator(AsyncFunction asyncFunction, Context context)
      {
        this._asyncFunction = asyncFunction;
        this._context = context;
      }

      public void Build(JSValue promise) => this.ResultPromise = this.subscribeOrReturnValue(promise);

      private JSValue subscribeOrReturnValue(JSValue promiseOrValue) => !(promiseOrValue?.Value is Promise promise) ? promiseOrValue : JSValue.Marshal((object) promise.then(new Func<JSValue, JSValue>(this.then), new Func<JSValue, JSValue>(this.fail)));

      private JSValue fail(JSValue arg) => this.@continue(arg, ExecutionMode.ResumeThrow);

      private JSValue then(JSValue arg) => this.@continue(arg, ExecutionMode.Resume);

      private JSValue @continue(JSValue arg, ExecutionMode mode)
      {
        this._context._executionInfo = arg;
        this._context._executionMode = mode;
        return this.subscribeOrReturnValue(this._asyncFunction.run(this._context));
      }
    }
  }
}
