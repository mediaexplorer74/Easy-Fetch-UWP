// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Functions.ConstantFunction
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core.Interop;
using NiL.JS.Expressions;

namespace NiL.JS.Core.Functions
{
  [Prototype(typeof (Function), true)]
  internal sealed class ConstantFunction : Function
  {
    private readonly JSValue _value;

    public ConstantFunction(JSValue value, FunctionDefinition functionDefinition)
      : base((Context) Context.CurrentGlobalContext, functionDefinition)
    {
      this._value = value;
    }

    internal override JSValue InternalInvoke(
      JSValue targetObject,
      Expression[] arguments,
      Context initiator,
      bool withSpread,
      bool construct)
    {
      for (int index = 0; index < arguments.Length; ++index)
        arguments[index].Evaluate(initiator);
      return construct ? this.ConstructObject() : this._value;
    }

    protected internal override JSValue Invoke(
      bool construct,
      JSValue targetObject,
      Arguments arguments)
    {
      return this._value;
    }
  }
}
