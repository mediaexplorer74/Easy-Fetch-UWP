// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Functions.GeneratorIterator
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core.Interop;

namespace NiL.JS.Core.Functions
{
  internal sealed class GeneratorIterator : IIterator, IIterable
  {
    private static readonly Arguments _emptyArguments = new Arguments();
    private Context _initialContext;
    private Context _generatorContext;
    private Arguments _initialArgs;
    private Function _generator;
    private JSValue _targetObject;

    [Hidden]
    public GeneratorIterator(GeneratorFunction generator, JSValue self, Arguments args)
    {
      this._initialContext = Context.CurrentContext;
      this._generator = (Function) generator;
      this._initialArgs = args ?? GeneratorIterator._emptyArguments;
      this._targetObject = self;
    }

    public IIteratorResult next(Arguments args)
    {
      if (this._generatorContext == null)
      {
        this.initContext();
      }
      else
      {
        switch (this._generatorContext._executionMode)
        {
          case ExecutionMode.Suspend:
            this._generatorContext._executionMode = ExecutionMode.Resume;
            goto case ExecutionMode.ResumeThrow;
          case ExecutionMode.ResumeThrow:
            this._generatorContext._executionInfo = args != null ? args[0] : JSValue.undefined;
            break;
          default:
            return (IIteratorResult) new GeneratorResult(JSValue.undefined, true);
        }
      }
      this._generatorContext.Activate();
      JSValue jsValue = (JSValue) null;
      try
      {
        jsValue = this._generator.evaluateBody(this._generatorContext);
      }
      finally
      {
        this._generatorContext.Deactivate();
      }
      return (IIteratorResult) new GeneratorResult(jsValue, this._generatorContext._executionMode != ExecutionMode.Suspend);
    }

    private void initContext()
    {
      this._generatorContext = new Context(this._initialContext, true, this._generator);
      Context generatorContext = this._generatorContext;
      Context currentContext = Context.CurrentContext;
      int num = (currentContext != null ? currentContext._callDepth : 0) + 1;
      generatorContext._callDepth = num;
      this._generatorContext._definedVariables = this._generator._functionDefinition._body._variables;
      this._generator.initParameters(this._initialArgs, true, this._generatorContext);
      this._generator.initContext(this._targetObject, this._initialArgs, true, this._generatorContext);
    }

    public IIteratorResult @return()
    {
      if (this._generatorContext == null)
        this.initContext();
      this._generatorContext._executionMode = ExecutionMode.Return;
      return this.next((Arguments) null);
    }

    public IIteratorResult @throw(Arguments arguments = null)
    {
      if (this._generatorContext == null)
        return (IIteratorResult) new GeneratorResult(JSValue.undefined, true);
      this._generatorContext._executionMode = ExecutionMode.ResumeThrow;
      return this.next(arguments);
    }

    public IIterator iterator() => (IIterator) this;

    [Hidden]
    public override bool Equals(object obj) => base.Equals(obj);

    [Hidden]
    public override int GetHashCode() => base.GetHashCode();

    [Hidden]
    public override string ToString() => base.ToString();
  }
}
