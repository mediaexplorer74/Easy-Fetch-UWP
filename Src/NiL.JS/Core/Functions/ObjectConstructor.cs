// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Functions.ObjectConstructor
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core.Interop;
using System.Collections.Generic;

namespace NiL.JS.Core.Functions
{
  [Prototype(typeof (Function), true)]
  internal class ObjectConstructor : ConstructorProxy
  {
    public override string name => "Object";

    public ObjectConstructor(Context context, StaticProxy staticProxy, JSObject prototype)
      : base(context, staticProxy, prototype)
    {
      this._length = new Number(1);
    }

    protected internal override JSValue Invoke(
      bool construct,
      JSValue targetObject,
      Arguments arguments)
    {
      JSValue jsValue = targetObject;
      if (jsValue != null && (jsValue._attributes & JSValueAttributesInternal.ConstructingObject) == JSValueAttributesInternal.None)
        jsValue = (JSValue) null;
      if (arguments != null && arguments._iValue > 0)
        jsValue = arguments[0];
      if (jsValue == null)
        return this.ConstructObject();
      return jsValue._valueType >= JSValueType.Object ? (jsValue._oValue == null ? this.ConstructObject() : jsValue) : (jsValue._valueType <= JSValueType.Undefined ? this.ConstructObject() : (JSValue) jsValue.ToObject());
    }

    protected internal override JSValue ConstructObject() => (JSValue) JSObject.CreateObject();

    protected internal override IEnumerator<KeyValuePair<string, JSValue>> GetEnumerator(
      bool hideNonEnum,
      EnumerationMode enumerationMode)
    {
      ObjectConstructor objectConstructor = this;
      IEnumerator<KeyValuePair<string, JSValue>> pe = objectConstructor._staticProxy.GetEnumerator(hideNonEnum, enumerationMode);
      while (pe.MoveNext())
        yield return pe.Current;
      pe = objectConstructor.__proto__.GetEnumerator(hideNonEnum, enumerationMode);
      while (pe.MoveNext())
        yield return pe.Current;
    }
  }
}
