// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Functions.ExternalFunction
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core.Interop;
using System;
using System.Linq;
using System.Reflection;

namespace NiL.JS.Core.Functions
{
  [Prototype(typeof (Function), true)]
  public sealed class ExternalFunction : Function
  {
    private readonly ExternalFunctionDelegate _delegate;

    public override string name => this._delegate.GetMethodInfo().Name;

    public override JSValue prototype
    {
      get => (JSValue) null;
      set
      {
      }
    }

    public ExternalFunctionDelegate Delegate => this._delegate;

    public ExternalFunction(ExternalFunctionDelegate @delegate)
    {
      if (this._length == null)
      {
        Number number = new Number(0);
        number._attributes = JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly;
        this._length = number;
      }
      Attribute[] array = CustomAttributeExtensions.GetCustomAttributes(@delegate.GetMethodInfo(), typeof (ArgumentsCountAttribute), false).ToArray<Attribute>();
      this._length._iValue = array.Length != 0 ? ((ArgumentsCountAttribute) array[0]).Count : 1;
      this._delegate = @delegate != null ? @delegate : throw new ArgumentNullException();
      this.RequireNewKeywordLevel = RequireNewKeywordLevel.WithoutNewOnly;
    }

    protected internal override JSValue Invoke(
      bool construct,
      JSValue targetObject,
      Arguments arguments)
    {
      return this._delegate(targetObject, arguments) ?? JSValue.NotExists;
    }

    public override string ToString(bool headerOnly)
    {
      string str = "function " + this.name + "()";
      if (!headerOnly)
        str += " { [native code] }";
      return str;
    }
  }
}
