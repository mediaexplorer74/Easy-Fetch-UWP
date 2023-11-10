// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Functions.EvalFunction
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core.Interop;
using NiL.JS.Expressions;
using System.Collections.Generic;

namespace NiL.JS.Core.Functions
{
  internal sealed class EvalFunction : Function
  {
    [Hidden]
    public override string name
    {
      [Hidden] get => "eval";
    }

    [Field]
    [DoNotDelete]
    [DoNotEnumerate]
    [NotConfigurable]
    public override JSValue prototype
    {
      [Hidden] get => (JSValue) null;
      [Hidden] set
      {
      }
    }

    [Hidden]
    public EvalFunction()
    {
      this._length = new Number(1);
      this._prototype = JSValue.undefined;
      this.RequireNewKeywordLevel = RequireNewKeywordLevel.WithoutNewOnly;
    }

    internal override JSValue InternalInvoke(
      JSValue targetObject,
      Expression[] arguments,
      Context initiator,
      bool withSpread,
      bool construct)
    {
      if (construct)
        ExceptionHelper.ThrowTypeError("eval can not be called as constructor");
      return arguments == null || arguments.Length == 0 ? JSValue.NotExists : base.InternalInvoke(targetObject, arguments, initiator, withSpread, construct);
    }

    protected internal override JSValue Invoke(
      bool construct,
      JSValue targetObject,
      Arguments arguments)
    {
      if (arguments == null)
        return JSValue.NotExists;
      JSValue jsValue = arguments[0];
      if (jsValue._valueType != JSValueType.String)
        return jsValue;
      Stack<Context> contextStack = new Stack<Context>();
      try
      {
        Context context = Context.CurrentContext;
        Context rootContext;
        for (rootContext = context.RootContext; context != rootContext && context != null; context = context.Deactivate())
          contextStack.Push(context);
        if (context != null)
          return context.Eval(arguments[0].ToString());
        rootContext.Activate();
        try
        {
          return rootContext.Eval(arguments[0].ToString());
        }
        finally
        {
          rootContext.Deactivate();
        }
      }
      finally
      {
        while (contextStack.Count != 0)
          contextStack.Pop().Activate();
      }
    }

    [Hidden]
    public override string ToString(bool headerOnly)
    {
      string str = "function eval()";
      if (!headerOnly)
        str += " { [native code] }";
      return str;
    }
  }
}
