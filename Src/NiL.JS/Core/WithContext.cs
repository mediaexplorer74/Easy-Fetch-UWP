// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.WithContext
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using System;

namespace NiL.JS.Core
{
  public sealed class WithContext : Context
  {
    private JSValue @object;

    public WithContext(JSValue obj, Context prototype)
      : base(prototype, false, prototype._owner)
    {
      if (obj == null)
        throw new ArgumentNullException(nameof (obj));
      if (obj._valueType == JSValueType.NotExists)
        ExceptionHelper.Throw((Error) new ReferenceError("Variable is not defined."));
      if (obj._valueType <= JSValueType.Undefined)
        ExceptionHelper.Throw((Error) new TypeError("Can't access to property value of \"undefined\"."));
      if (obj._valueType >= JSValueType.Object && obj._oValue == null)
        ExceptionHelper.Throw((Error) new TypeError("Can't access to property value of \"null\"."));
      if (!(obj._oValue is JSValue jsValue))
        jsValue = obj;
      this.@object = jsValue;
    }

    public override JSValue DefineVariable(string name, bool deletable) => this._parent.DefineVariable(name);

    protected internal override JSValue GetVariable(string name, bool create)
    {
      this._thisBind = this._parent._thisBind;
      JSValue variable = this.@object.GetProperty(name, create, PropertyScope.Common);
      if (variable._valueType < JSValueType.Undefined)
      {
        variable = this._parent.GetVariable(name, create);
        this._objectSource = this._parent._objectSource;
      }
      else
        this._objectSource = this.@object;
      return variable;
    }
  }
}
