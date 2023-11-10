// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.ObjectWrapper
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core.Interop;
using System.Collections.Generic;

namespace NiL.JS.Core
{
  internal sealed class ObjectWrapper : JSObject
  {
    internal object instance;

    [Hidden]
    public override object Value => this.instance ?? base.Value;

    [Hidden]
    public ObjectWrapper(object instance, JSObject proto)
    {
      this.instance = instance;
      if (instance is Date)
        this._valueType = JSValueType.Date;
      else
        this._valueType = JSValueType.Object;
      this._oValue = (object) this;
      this._attributes = JSValueAttributesInternal.SystemObject;
      if (proto == null)
        return;
      this._attributes |= proto._attributes & JSValueAttributesInternal.Immutable;
      this._objectPrototype = proto;
    }

    [Hidden]
    public ObjectWrapper(object instance)
      : this(instance, instance != null ? Context.CurrentGlobalContext.GetPrototype(instance.GetType()) : (JSObject) null)
    {
    }

    protected internal override JSValue GetProperty(
      JSValue name,
      bool forWrite,
      PropertyScope memberScope)
    {
      return this.instance is JSValue instance ? instance.GetProperty(name, forWrite, memberScope) : base.GetProperty(name, forWrite, memberScope);
    }

    protected internal override void SetProperty(
      JSValue name,
      JSValue value,
      PropertyScope memberScope,
      bool strict)
    {
      if (this.instance is JSValue instance)
        instance.SetProperty(name, value, memberScope, strict);
      else
        base.SetProperty(name, value, memberScope, strict);
    }

    protected internal override bool DeleteProperty(JSValue name) => this.instance is JSValue instance ? instance.DeleteProperty(name) : base.DeleteProperty(name);

    protected internal override IEnumerator<KeyValuePair<string, JSValue>> GetEnumerator(
      bool hideNonEnum,
      EnumerationMode enumerationMode)
    {
      return this.instance is JSValue instance ? instance.GetEnumerator(hideNonEnum, enumerationMode) : base.GetEnumerator(hideNonEnum, enumerationMode);
    }
  }
}
