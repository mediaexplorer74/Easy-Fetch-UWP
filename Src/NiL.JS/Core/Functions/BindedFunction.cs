// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Functions.BindedFunction
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core.Interop;
using System.Collections.Generic;

namespace NiL.JS.Core.Functions
{
  [Prototype(typeof (Function), true)]
  internal sealed class BindedFunction : Function
  {
    private Function original;
    private JSValue _thisBind;
    private Arguments bindedArguments;

    public override JSValue caller
    {
      get
      {
        Function.ThrowTypeError();
        return (JSValue) null;
      }
      set => Function.ThrowTypeError();
    }

    public override JSValue arguments
    {
      get
      {
        Function.ThrowTypeError();
        return (JSValue) null;
      }
      set => Function.ThrowTypeError();
    }

    public override JSValue prototype
    {
      get => (JSValue) null;
      set
      {
      }
    }

    public BindedFunction(Function proto, Arguments args)
      : base(proto.Context, proto._functionDefinition)
    {
      if (this._length == null)
        this._length = new Number(0);
      this._length._iValue = proto.length._iValue;
      this.original = proto;
      this._thisBind = args[0];
      this.bindedArguments = args;
      if (args._iValue > 0)
      {
        --args._iValue;
        for (int index = 0; index < args._iValue; ++index)
          args[index] = args[index + 1];
        this._length._iValue -= args._iValue;
        if (this._length._iValue < 0)
          this._length._iValue = 0;
        args[args._iValue] = (JSValue) null;
        if (args._iValue == 0)
          this.bindedArguments = (Arguments) null;
      }
      else
        this.bindedArguments = (Arguments) null;
      this.RequireNewKeywordLevel = proto.RequireNewKeywordLevel;
    }

    protected internal override JSValue Invoke(
      bool construct,
      JSValue targetObject,
      Arguments arguments)
    {
      if (this.bindedArguments != null)
      {
        if (arguments == null)
          arguments = new Arguments();
        arguments._iValue += this.bindedArguments._iValue;
        int iValue1 = arguments._iValue;
        while (iValue1-- > this.bindedArguments._iValue)
          arguments[iValue1] = arguments[iValue1 - this.bindedArguments._iValue];
        int iValue2 = this.bindedArguments._iValue;
        while (iValue2-- > 0)
          arguments[iValue2] = this.bindedArguments[iValue2];
      }
      return (construct || this._thisBind == null || this._thisBind.IsNull || !this._thisBind.Defined) && targetObject != null && targetObject.Defined ? this.original.Invoke(construct, targetObject, arguments) : this.original.Call(this._thisBind, arguments);
    }

    protected internal override JSValue ConstructObject() => this.original.ConstructObject();

    protected internal override IEnumerator<KeyValuePair<string, JSValue>> GetEnumerator(
      bool hideNonEnumerable,
      EnumerationMode enumeratorMode)
    {
      return this.original.GetEnumerator(hideNonEnumerable, enumeratorMode);
    }

    protected internal override JSValue GetProperty(
      JSValue key,
      bool forWrite,
      PropertyScope memberScope)
    {
      return this.original.GetProperty(key, forWrite, memberScope);
    }

    public override string ToString(bool headerOnly) => this.original.ToString(headerOnly);
  }
}
