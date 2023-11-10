// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.JSValue
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core.Functions;
using NiL.JS.Core.Interop;
using NiL.JS.Expressions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace NiL.JS.Core
{
  [DebuggerTypeProxy(typeof (JSObjectDebugView))]
  [DebuggerDisplay("Value = {debugValue()} ({ValueType})")]
  public class JSValue : 
    IEnumerable<KeyValuePair<string, JSValue>>,
    IEnumerable,
    IComparable<JSValue>,
    IConvertible
  {
    internal const int publicAttributesMask = 33554463;
    internal static readonly JSValue numberString = (JSValue) "number";
    internal static readonly JSValue undefinedString = (JSValue) nameof (undefined);
    internal static readonly JSValue stringString = (JSValue) "string";
    internal static readonly JSValue symbolString = (JSValue) "symbol";
    internal static readonly JSValue booleanString = (JSValue) "boolean";
    internal static readonly JSValue functionString = (JSValue) "function";
    internal static readonly JSValue objectString = (JSValue) "object";
    internal static readonly JSValue undefined = new JSValue()
    {
      _valueType = JSValueType.Undefined,
      _attributes = JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.NonConfigurable | JSValueAttributesInternal.SystemObject
    };
    internal static readonly JSValue notExists = new JSValue()
    {
      _valueType = JSValueType.NotExists,
      _attributes = JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.NonConfigurable | JSValueAttributesInternal.SystemObject
    };
    internal static readonly JSObject @null;
    internal static readonly JSValue nullString;
    internal JSValueAttributesInternal _attributes;
    internal JSValueType _valueType;
    internal int _iValue;
    internal double _dValue;
    internal object _oValue;

    [Hidden]
    public static JSValue Undefined
    {
      [Hidden] get => JSValue.undefined;
    }

    [Hidden]
    public static JSValue NotExists
    {
      [Hidden] get
      {
        JSValue.notExists._valueType = JSValueType.NotExists;
        return JSValue.notExists;
      }
    }

    [Hidden]
    public static JSValue NotExistsInObject
    {
      [Hidden] get
      {
        JSValue.notExists._valueType = JSValueType.NotExistsInObject;
        return JSValue.notExists;
      }
    }

    [Hidden]
    public static JSValue Null
    {
      [Hidden] get => (JSValue) JSValue.@null;
    }

    [Hidden]
    public virtual JSValue this[string name]
    {
      [Hidden] get => this.GetProperty(name);
      [Hidden] set => this.SetProperty((JSValue) name, value ?? JSValue.undefined, true);
    }

    [Hidden]
    public virtual object Value
    {
      [Hidden] get
      {
        switch (this._valueType)
        {
          case JSValueType.Boolean:
            return (object) (this._iValue != 0);
          case JSValueType.Integer:
            return (object) this._iValue;
          case JSValueType.Double:
            return (object) this._dValue;
          case JSValueType.String:
            return (object) this._oValue.ToString();
          case JSValueType.Symbol:
            return this._oValue;
          case JSValueType.Object:
          case JSValueType.Function:
          case JSValueType.Date:
          case JSValueType.Property:
          case JSValueType.SpreadOperatorResult:
            return this._oValue != this && this._oValue is JSObject ? (this._oValue as JSObject).Value : this._oValue;
          default:
            return (object) null;
        }
      }
      protected set
      {
        switch (this._valueType)
        {
          case JSValueType.Boolean:
            this._iValue = (bool) value ? 1 : 0;
            break;
          case JSValueType.Integer:
            this._iValue = (int) value;
            break;
          case JSValueType.Double:
            this._dValue = (double) value;
            break;
          case JSValueType.String:
            this._oValue = (object) (string) value;
            break;
          case JSValueType.Object:
          case JSValueType.Function:
          case JSValueType.Date:
          case JSValueType.Property:
            this._oValue = value;
            break;
          default:
            throw new InvalidOperationException();
        }
      }
    }

    [Hidden]
    public JSValueType ValueType
    {
      [Hidden] get => this._valueType;
      protected set => this._valueType = value;
    }

    [Hidden]
    public JSAttributes Attributes
    {
      [Hidden] get => (JSAttributes) (this._attributes & (JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.Immutable | JSValueAttributesInternal.NonConfigurable | JSValueAttributesInternal.Reassign));
    }

    protected bool Reassign
    {
      get => (this._attributes & JSValueAttributesInternal.Reassign) > JSValueAttributesInternal.None;
      set
      {
        if (value)
          this._attributes |= JSValueAttributesInternal.Reassign;
        else
          this._attributes &= ~JSValueAttributesInternal.Reassign;
      }
    }

    [DoNotDelete]
    [DoNotEnumerate]
    [NotConfigurable]
    [CLSCompliant(false)]
    public virtual JSObject __proto__
    {
      [Hidden] [return: JSValue.ProtoConverter] get
      {
        if (this._valueType >= JSValueType.Object && this._oValue != this && this._oValue is JSObject)
          return (this._oValue as JSObject).__proto__;
        if (!this.Defined || this.IsNull)
          ExceptionHelper.Throw((Error) new TypeError("Can not get prototype of null or undefined"));
        return this.GetDefaultPrototype();
      }
      [Hidden] [param: JSValue.ProtoConverter] set
      {
        if ((this._attributes & JSValueAttributesInternal.Immutable) != JSValueAttributesInternal.None || this._valueType < JSValueType.Object)
          return;
        if (this._oValue == this)
          throw new InvalidOperationException();
        if (this._oValue == null)
          ExceptionHelper.Throw((Error) new ReferenceError("Cannot set __proto__ of null"));
        (this._oValue as JSObject).__proto__ = value;
      }
    }

    [Hidden]
    public bool Exists
    {
      [Hidden, MethodImpl(MethodImplOptions.AggressiveInlining)] get => this._valueType >= JSValueType.Undefined;
    }

    [Hidden]
    public bool Defined
    {
      [Hidden, MethodImpl(MethodImplOptions.AggressiveInlining)] get => this._valueType > JSValueType.Undefined;
    }

    [Hidden]
    public bool IsNull
    {
      [Hidden, MethodImpl(MethodImplOptions.AggressiveInlining)] get => this._valueType >= JSValueType.Object && this._oValue == null;
    }

    [Hidden]
    public bool IsNumber
    {
      [Hidden, MethodImpl(MethodImplOptions.AggressiveInlining)] get => this._valueType == JSValueType.Integer || this._valueType == JSValueType.Double;
    }

    internal bool NeedClone
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => (this._attributes & (JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.SystemObject)) == JSValueAttributesInternal.SystemObject;
    }

    internal bool IsBox
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this._valueType >= JSValueType.Object && this._oValue != null && this._oValue != this;
    }

    internal JSObject GetDefaultPrototype()
    {
      switch (this._valueType)
      {
        case JSValueType.Boolean:
          return Context.CurrentGlobalContext.GetPrototype(typeof (NiL.JS.BaseLibrary.Boolean));
        case JSValueType.Integer:
        case JSValueType.Double:
          return Context.CurrentGlobalContext.GetPrototype(typeof (Number));
        case JSValueType.String:
          return Context.CurrentGlobalContext.GetPrototype(typeof (NiL.JS.BaseLibrary.String));
        default:
          if (this._oValue == null || this._oValue == this)
            return Context.CurrentGlobalContext.GetPrototype(this.GetType());
          return this._oValue is JSValue oValue ? oValue.GetDefaultPrototype() ?? JSValue.@null : Context.CurrentGlobalContext.GetPrototype(this._oValue.GetType());
      }
    }

    [Hidden]
    public JSValue GetProperty(string name) => this.GetProperty((JSValue) name, false, PropertyScope.Common);

    [Hidden]
    public JSValue GetProperty(string name, PropertyScope propertyScope) => this.GetProperty((JSValue) name, false, propertyScope);

    [Hidden]
    public JSValue DefineProperty(string name) => this.GetProperty((JSValue) name, true, PropertyScope.Own);

    [Hidden]
    public void DefineGetSetProperty(string name, Func<object> getter, Action<object> setter) => this.DefineGetSetProperty((Context) Context.CurrentGlobalContext, name, getter, setter);

    [Hidden]
    public void DefineGetSetProperty(
      Context context,
      string name,
      Func<object> getter,
      Action<object> setter)
    {
      JSValue jsValue = this.GetProperty(name).ValueType < JSValueType.Undefined ? this.DefineProperty(name) : throw new ArgumentException();
      if (jsValue.ValueType < JSValueType.Undefined)
        throw new InvalidOperationException();
      jsValue._valueType = JSValueType.Property;
      Function getter1 = (Function) null;
      if (getter != null)
        getter1 = (Function) new MethodProxy(context, (MethodBase) getter.GetMethodInfo(), getter.Target);
      Function setter1 = (Function) null;
      if (setter != null)
        setter1 = (Function) new MethodProxy(context, (MethodBase) setter.GetMethodInfo(), setter.Target);
      jsValue._oValue = (object) new PropertyPair(getter1, setter1);
    }

    [Hidden]
    public bool DeleteProperty(string name) => name != null ? this.DeleteProperty((JSValue) name) : throw new ArgumentNullException("memberName");

    protected internal JSValue GetProperty(string name, bool forWrite, PropertyScope propertyScope) => this.GetProperty((JSValue) name, forWrite, propertyScope);

    protected internal virtual JSValue GetProperty(
      JSValue key,
      bool forWrite,
      PropertyScope propertyScope)
    {
      switch (this._valueType)
      {
        case JSValueType.NotExists:
        case JSValueType.NotExistsInObject:
        case JSValueType.Undefined:
          ExceptionHelper.ThrowTypeError(string.Format(Strings.TryingToGetProperty, (object) key, (object) "undefined"));
          return (JSValue) null;
        case JSValueType.Boolean:
          if (propertyScope == PropertyScope.Own)
            return JSValue.notExists;
          forWrite = false;
          return Context.CurrentGlobalContext.GetPrototype(typeof (NiL.JS.BaseLibrary.Boolean)).GetProperty(key, false, PropertyScope.Common);
        case JSValueType.Integer:
        case JSValueType.Double:
          if (propertyScope == PropertyScope.Own)
            return JSValue.notExists;
          forWrite = false;
          return Context.CurrentGlobalContext.GetPrototype(typeof (Number)).GetProperty(key, false, PropertyScope.Common);
        case JSValueType.String:
          return this.stringGetProperty(key, forWrite, propertyScope);
        default:
          if (this._oValue != this)
          {
            if (this._oValue == null)
              ExceptionHelper.ThrowTypeError(string.Format(Strings.TryingToGetProperty, (object) key, (object) "null"));
            if (this._oValue is JSObject oValue)
              return oValue.GetProperty(key, forWrite, propertyScope);
          }
          ExceptionHelper.Throw((Exception) new InvalidOperationException("Method GetProperty(...) of custom types must be overridden"));
          return (JSValue) null;
      }
    }

    private JSValue stringGetProperty(JSValue name, bool forWrite, PropertyScope propertyScope)
    {
      if ((name._valueType == JSValueType.String || name._valueType >= JSValueType.Object) && string.CompareOrdinal(name._oValue.ToString(), "length") == 0)
        return (object) (this._oValue as RopeString) != null ? (JSValue) (this._oValue as RopeString).Length : (JSValue) this._oValue.ToString().Length;
      double num = Tools.JSObjectToDouble(name);
      int index;
      if (num >= 0.0 && (double) (index = (int) num) == num && this._oValue.ToString().Length > index)
        return (JSValue) this._oValue.ToString()[index];
      return propertyScope == PropertyScope.Own ? JSValue.notExists : Context.CurrentGlobalContext.GetPrototype(typeof (NiL.JS.BaseLibrary.String)).GetProperty(name, false, PropertyScope.Common);
    }

    protected internal void SetProperty(JSValue name, JSValue value, bool throwOnError) => this.SetProperty(name, value, PropertyScope.Common, throwOnError);

    protected internal virtual void SetProperty(
      JSValue name,
      JSValue value,
      PropertyScope propertyScope,
      bool throwOnError)
    {
      if (this._valueType >= JSValueType.Object)
      {
        if (this._oValue == null)
          ExceptionHelper.ThrowTypeError(string.Format(Strings.TryingToSetProperty, (object) name, (object) "null"));
        if (this._oValue == this)
          this.GetProperty(name, true, propertyScope).Assign(value);
        (this._oValue as JSObject)?.SetProperty(name, value, propertyScope, throwOnError);
      }
      else
      {
        if (this._valueType > JSValueType.Undefined)
          return;
        ExceptionHelper.ThrowTypeError(string.Format(Strings.TryingToSetProperty, (object) name, (object) "undefined"));
      }
    }

    protected internal virtual bool DeleteProperty(JSValue name)
    {
      if (this._valueType >= JSValueType.Object)
      {
        if (this._oValue == null)
          ExceptionHelper.ThrowTypeError(string.Format(Strings.TryingToGetProperty, (object) name, (object) "null"));
        if (this._oValue == this)
          throw new InvalidOperationException();
        if (this._oValue is JSObject oValue)
          return oValue.DeleteProperty(name);
      }
      else if (this._valueType <= JSValueType.Undefined)
        ExceptionHelper.ThrowTypeError(string.Format(Strings.TryingToGetProperty, (object) name, (object) "undefined"));
      return true;
    }

    [Hidden]
    public override bool Equals(object obj)
    {
      if (!(obj is JSValue))
        return false;
      return obj == this || StrictEqual.Check(this, obj as JSValue);
    }

    [Hidden]
    public override int GetHashCode() => base.GetHashCode();

    [Hidden]
    public static implicit operator JSValue(char value) => (JSValue) new NiL.JS.BaseLibrary.String(value.ToString());

    [Hidden]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator JSValue(bool value) => (JSValue) (NiL.JS.BaseLibrary.Boolean) value;

    [Hidden]
    public static implicit operator JSValue(int value) => (JSValue) new Number(value);

    [Hidden]
    public static implicit operator JSValue(long value) => (JSValue) new Number(value);

    [Hidden]
    public static implicit operator JSValue(double value) => (JSValue) new Number(value);

    [Hidden]
    public static implicit operator JSValue(string value) => (JSValue) new NiL.JS.BaseLibrary.String(value);

    [Hidden]
    public static explicit operator int(JSValue obj) => Tools.JSObjectToInt32(obj);

    [Hidden]
    public static explicit operator long(JSValue obj) => Tools.JSObjectToInt64(obj);

    [Hidden]
    public static explicit operator double(JSValue obj) => Tools.JSObjectToDouble(obj);

    [Hidden]
    public static explicit operator bool(JSValue obj)
    {
      switch (obj._valueType)
      {
        case JSValueType.Boolean:
        case JSValueType.Integer:
          return obj._iValue != 0;
        case JSValueType.Double:
          return obj._dValue != 0.0 && !double.IsNaN(obj._dValue);
        case JSValueType.String:
          return !string.IsNullOrEmpty(obj._oValue.ToString());
        case JSValueType.Object:
        case JSValueType.Function:
        case JSValueType.Date:
          return obj._oValue != null;
        default:
          return false;
      }
    }

    [Hidden]
    public static implicit operator JSValue(Delegate action) => JSValue.Marshal((object) action);

    [Hidden]
    public object Clone() => (object) this.CloneImpl();

    internal JSValue CloneImpl() => this.CloneImpl(true);

    internal JSValue CloneImpl(bool force) => this.CloneImpl(force, JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.SystemObject | JSValueAttributesInternal.ProxyPrototype | JSValueAttributesInternal.Temporary | JSValueAttributesInternal.Reassign);

    internal virtual JSValue CloneImpl(JSValueAttributesInternal resetMask) => this.CloneImpl(true, resetMask);

    internal virtual JSValue CloneImpl(bool force, JSValueAttributesInternal resetMask)
    {
      if (!force && (this._attributes & JSValueAttributesInternal.Cloned) != JSValueAttributesInternal.None)
      {
        this._attributes &= ~(JSValueAttributesInternal.Cloned | resetMask);
        return this;
      }
      JSValue jsValue = new JSValue();
      jsValue.Assign(this);
      jsValue._attributes = this._attributes & ~resetMask;
      return jsValue;
    }

    private object debugValue()
    {
      if (this._valueType <= JSValueType.Undefined)
        return (object) JSValueType.Undefined;
      if (this._valueType == JSValueType.String)
        return (object) ("\"" + this._oValue?.ToString() + "\"");
      if (this._valueType < JSValueType.Object)
        return this.Value;
      if (!(this._oValue is JSValue jsValue))
        jsValue = this;
      return (object) jsValue._valueType;
    }

    [Hidden]
    public override string ToString() => this.BaseToString();

    protected internal string BaseToString()
    {
      if (this._valueType == JSValueType.String)
        return this._oValue is string oValue ? oValue : this._oValue.ToString();
      if (this._valueType <= JSValueType.Undefined)
        return "undefined";
      if (this._valueType == JSValueType.Property)
      {
        string str = "[";
        if ((this._oValue as PropertyPair).getter != null)
          str += "Getter";
        if ((this._oValue as PropertyPair).setter != null)
          str += str.Length != 1 ? "/Setter" : "Setter";
        return str.Length == 1 ? "[Invalid Property]" : str + "]";
      }
      JSValue jsValue = this._valueType >= JSValueType.Object ? this.ToPrimitiveValue_String_Value() : this;
      switch (jsValue._valueType)
      {
        case JSValueType.Boolean:
          return jsValue._iValue == 0 ? "false" : "true";
        case JSValueType.Integer:
          return Tools.Int32ToString(jsValue._iValue);
        case JSValueType.Double:
          return Tools.DoubleToString(jsValue._dValue);
        case JSValueType.String:
          return jsValue._oValue.ToString();
        default:
          return (jsValue._oValue ?? (object) "null").ToString();
      }
    }

    [Hidden]
    public virtual void Assign(JSValue value)
    {
      if ((this._attributes & (JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.SystemObject)) != JSValueAttributesInternal.None)
        return;
      this._valueType = value._valueType | JSValueType.Undefined;
      this._iValue = value._iValue;
      this._dValue = value._dValue;
      this._oValue = value._oValue;
    }

    internal JSValue ToPrimitiveValue_Value_String() => this.ToPrimitiveValue("valueOf", "toString");

    internal JSValue ToPrimitiveValue_LocaleString_Value() => this.ToPrimitiveValue("toLocaleString", "valueOf");

    internal JSValue ToPrimitiveValue_String_Value() => this.ToPrimitiveValue("toString", "valueOf");

    internal JSValue ToPrimitiveValue(string func0, string func1 = null)
    {
      if (this._valueType >= JSValueType.Object && this._oValue != null)
      {
        if (this._oValue == null)
          return JSValue.nullString;
        JSValue jsValue1 = Tools.InvokeGetter(this.GetProperty(func0), this);
        if (jsValue1._valueType == JSValueType.Function)
        {
          JSValue primitiveValue = (jsValue1._oValue as Function).Call(this, (Arguments) null);
          if (primitiveValue._valueType == JSValueType.Object && primitiveValue._oValue is NiL.JS.BaseLibrary.String)
            primitiveValue = (JSValue) (primitiveValue._oValue as NiL.JS.BaseLibrary.String);
          if (primitiveValue._valueType < JSValueType.Object)
            return primitiveValue;
        }
        if (func1 != null)
        {
          JSValue jsValue2 = Tools.InvokeGetter(this.GetProperty(func1), this);
          if (jsValue2._valueType == JSValueType.Function)
          {
            JSValue primitiveValue = (jsValue2._oValue as Function).Call(this, (Arguments) null);
            if (primitiveValue._valueType == JSValueType.Object && primitiveValue._oValue is NiL.JS.BaseLibrary.String)
              primitiveValue = (JSValue) (primitiveValue._oValue as NiL.JS.BaseLibrary.String);
            if (primitiveValue._valueType < JSValueType.Object)
              return primitiveValue;
          }
        }
        ExceptionHelper.Throw((Error) new TypeError("Can't convert object to primitive value."));
      }
      return this;
    }

    [Hidden]
    public JSObject ToObject()
    {
      if (this._valueType >= JSValueType.Object)
        return this._oValue is JSObject oValue ? oValue : JSValue.@null;
      if (this._valueType >= JSValueType.Undefined)
        return (JSObject) new ObjectWrapper((object) this.ToPrimitiveTypeContainer());
      JSObject jsObject = new JSObject();
      jsObject._valueType = JSValueType.Object;
      return jsObject;
    }

    [Hidden]
    public JSValue ToPrimitiveTypeContainer()
    {
      if (this._valueType >= JSValueType.Object)
        return (JSValue) null;
      switch (this._valueType)
      {
        case JSValueType.Boolean:
          return !(this is NiL.JS.BaseLibrary.Boolean) ? (JSValue) new NiL.JS.BaseLibrary.Boolean(this._iValue != 0) : this;
        case JSValueType.Integer:
          return !(this is Number) ? (JSValue) new Number(this._iValue) : this;
        case JSValueType.Double:
          return !(this is Number) ? (JSValue) new Number(this._dValue) : this;
        case JSValueType.String:
          return !(this is NiL.JS.BaseLibrary.String) ? (JSValue) new NiL.JS.BaseLibrary.String(this._oValue.ToString()) : this;
        case JSValueType.Symbol:
          return (JSValue) (this._oValue as Symbol);
        default:
          return new JSValue()
          {
            _valueType = JSValueType.Undefined
          };
      }
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    [Hidden]
    public IEnumerator<KeyValuePair<string, JSValue>> GetEnumerator() => this.GetEnumerator(true, EnumerationMode.RequireValuesForWrite);

    protected internal virtual IEnumerator<KeyValuePair<string, JSValue>> GetEnumerator(
      bool hideNonEnumerable,
      EnumerationMode enumeratorMode)
    {
      return this._valueType >= JSValueType.Object && this._oValue != this && this._oValue is JSValue oValue ? oValue.GetEnumerator(hideNonEnumerable, enumeratorMode) : this.GetEnumeratorImpl(hideNonEnumerable);
    }

    private IEnumerator<KeyValuePair<string, JSValue>> GetEnumeratorImpl(bool hideNonEnum)
    {
      JSValue jsValue = this;
      if (jsValue._valueType == JSValueType.String)
      {
        string strValue = jsValue._oValue.ToString();
        int len = strValue.Length;
        for (int i = 0; i < len; ++i)
          yield return new KeyValuePair<string, JSValue>(Tools.Int32ToString(i), (JSValue) strValue[i].ToString());
        if (!hideNonEnum)
          yield return new KeyValuePair<string, JSValue>("length", (JSValue) len);
        strValue = (string) null;
      }
      else if (jsValue._valueType == JSValueType.Object && jsValue._oValue == jsValue)
        throw new InvalidOperationException("Internal error. #VaO");
    }

    [CLSCompliant(false)]
    [DoNotEnumerate]
    [ArgumentsCount(0)]
    [AllowNullArguments]
    public virtual JSValue toString(Arguments args)
    {
      if (!(this._oValue is JSValue jsValue))
        jsValue = this;
      JSValue target = jsValue;
      switch (target._valueType)
      {
        case JSValueType.Undefined:
          return (JSValue) "[object Undefined]";
        case JSValueType.Boolean:
          return (JSValue) "[object Boolean]";
        case JSValueType.Integer:
        case JSValueType.Double:
          return (JSValue) "[object Number]";
        case JSValueType.String:
          return (JSValue) "[object String]";
        case JSValueType.Object:
        case JSValueType.Date:
          if (target._oValue == null)
            return (JSValue) "[object Null]";
          if (target._oValue is GlobalObject)
            return (JSValue) target._oValue.ToString();
          JSValue property = target.GetProperty((JSValue) Symbol.toStringTag, false, PropertyScope.Common);
          if (property.Defined)
            return (JSValue) string.Format("[object {0}]", (object) Tools.InvokeGetter(property, target));
          if (target._oValue is Proxy)
          {
            Type hostedType = (target._oValue as Proxy)._hostedType;
            return (object) hostedType == (object) typeof (JSObject) ? (JSValue) "[object Object]" : (JSValue) ("[object " + hostedType.Name + "]");
          }
          return (object) target.Value.GetType() == (object) typeof (JSObject) ? (JSValue) "[object Object]" : (JSValue) ("[object " + target.Value.GetType().Name + "]");
        case JSValueType.Function:
          return (JSValue) "[object Function]";
        default:
          throw new NotImplementedException();
      }
    }

    [DoNotEnumerate]
    public virtual JSValue toLocaleString()
    {
      if (!(this._oValue is JSValue jsValue1))
        jsValue1 = this;
      JSValue jsValue2 = jsValue1;
      if (jsValue2._valueType >= JSValueType.Object && jsValue2._oValue == null)
        ExceptionHelper.Throw((Error) new TypeError("toLocaleString calling on null."));
      if (jsValue2._valueType <= JSValueType.Undefined)
        ExceptionHelper.Throw((Error) new TypeError("toLocaleString calling on undefined value."));
      return jsValue2 == this ? this.ToPrimitiveValue("toString") : jsValue2.toLocaleString();
    }

    [DoNotEnumerate]
    public virtual JSValue valueOf()
    {
      if (this._valueType >= JSValueType.Object && this._oValue == null)
        ExceptionHelper.Throw((Error) new TypeError("valueOf calling on null."));
      if (this._valueType <= JSValueType.Undefined)
        ExceptionHelper.Throw((Error) new TypeError("valueOf calling on undefined value."));
      if (this._valueType >= JSValueType.Object)
        return this;
      JSObject jsObject = new JSObject();
      jsObject._valueType = JSValueType.Object;
      jsObject._oValue = (object) this;
      return (JSValue) jsObject;
    }

    [DoNotEnumerate]
    public virtual JSValue propertyIsEnumerable(Arguments args)
    {
      if (this._valueType >= JSValueType.Object && this._oValue == null)
        ExceptionHelper.Throw((Error) new TypeError("propertyIsEnumerable calling on null."));
      if (this._valueType <= JSValueType.Undefined)
        ExceptionHelper.Throw((Error) new TypeError("propertyIsEnumerable calling on undefined value."));
      JSValue property = this.GetProperty(args[0].ToString(), PropertyScope.Own);
      return (JSValue) (property.Exists && (property._attributes & JSValueAttributesInternal.DoNotEnumerate) == JSValueAttributesInternal.None);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [DoNotEnumerate]
    public virtual JSValue isPrototypeOf(Arguments args)
    {
      if (this._valueType >= JSValueType.Object && this._oValue == null)
        ExceptionHelper.Throw((Error) new TypeError("isPrototypeOf calling on null."));
      if (this._valueType <= JSValueType.Undefined)
        ExceptionHelper.Throw((Error) new TypeError("isPrototypeOf calling on undefined value."));
      if (args.GetProperty("length")._iValue == 0)
        return (JSValue) false;
      JSValue proto = (JSValue) args[0].__proto__;
      if (this._valueType >= JSValueType.Object)
      {
        if (this._oValue != null)
        {
          for (; proto != null && proto._valueType >= JSValueType.Object && proto._oValue != null; proto = (JSValue) proto.__proto__)
          {
            if (proto._oValue == this._oValue)
              return (JSValue) true;
            JSObject prototypeInstance = proto._oValue is StaticProxy oValue ? oValue.PrototypeInstance : (JSObject) null;
            if (prototypeInstance != null && (this == prototypeInstance || this == prototypeInstance._oValue))
              return (JSValue) true;
          }
        }
      }
      else
      {
        if (proto._oValue == this._oValue)
          return (JSValue) true;
        JSObject prototypeInstance = proto._oValue is StaticProxy oValue ? oValue.PrototypeInstance : (JSObject) null;
        if (prototypeInstance != null && (this == prototypeInstance || this == prototypeInstance._oValue))
          return (JSValue) true;
      }
      return (JSValue) false;
    }

    [DoNotEnumerate]
    public virtual JSValue hasOwnProperty(Arguments args) => (JSValue) this.GetProperty(args[0], false, PropertyScope.Own).Exists;

    TypeCode IConvertible.GetTypeCode() => TypeCode.Object;

    bool IConvertible.ToBoolean(IFormatProvider provider) => (bool) this;

    byte IConvertible.ToByte(IFormatProvider provider) => (byte) Tools.JSObjectToInt32(this);

    char IConvertible.ToChar(IFormatProvider provider)
    {
      string str = this.ToString();
      return str.Length <= 0 ? char.MinValue : str[0];
    }

    DateTime IConvertible.ToDateTime(IFormatProvider provider)
    {
      if (this._valueType == JSValueType.Date)
        return (this._oValue as Date).ToDateTime();
      throw new InvalidCastException();
    }

    Decimal IConvertible.ToDecimal(IFormatProvider provider) => (Decimal) Tools.JSObjectToDouble(this);

    double IConvertible.ToDouble(IFormatProvider provider) => Tools.JSObjectToDouble(this);

    short IConvertible.ToInt16(IFormatProvider provider) => (short) Tools.JSObjectToInt32(this);

    int IConvertible.ToInt32(IFormatProvider provider) => this._valueType == JSValueType.Integer ? this._iValue : Tools.JSObjectToInt32(this);

    long IConvertible.ToInt64(IFormatProvider provider) => (long) (byte) Tools.JSObjectToInt64(this);

    sbyte IConvertible.ToSByte(IFormatProvider provider) => (sbyte) Tools.JSObjectToInt32(this);

    float IConvertible.ToSingle(IFormatProvider provider) => (float) Tools.JSObjectToDouble(this);

    string IConvertible.ToString(IFormatProvider provider) => this.ToString();

    object IConvertible.ToType(Type conversionType, IFormatProvider provider) => Tools.convertJStoObj(this, conversionType, true);

    ushort IConvertible.ToUInt16(IFormatProvider provider) => (ushort) Tools.JSObjectToInt32(this);

    uint IConvertible.ToUInt32(IFormatProvider provider) => (uint) Tools.JSObjectToInt32(this);

    ulong IConvertible.ToUInt64(IFormatProvider provider) => (ulong) Tools.JSObjectToInt64(this);

    [Hidden]
    public virtual int CompareTo(JSValue other)
    {
      if (this._valueType != other._valueType)
        throw new InvalidOperationException("Type mismatch");
      switch (this._valueType)
      {
        case JSValueType.NotExists:
        case JSValueType.NotExistsInObject:
        case JSValueType.Undefined:
          return 0;
        case JSValueType.Boolean:
        case JSValueType.Integer:
          return this._iValue - other._iValue;
        case JSValueType.Double:
          return System.Math.Sign(this._dValue - other._dValue);
        case JSValueType.String:
          return string.CompareOrdinal(this._oValue.ToString(), other._oValue.ToString());
        default:
          throw new NotImplementedException("Try to compare two values of " + this._valueType.ToString());
      }
    }

    public static JSValue Marshal(object value) => Context.CurrentGlobalContext.ProxyValue(value);

    public static JSValue Wrap(object value) => value == null ? JSValue.Null : (JSValue) new ObjectWrapper(value);

    public static JSValue GetConstructor(Type type) => (JSValue) Context.CurrentGlobalContext.GetConstructor(type);

    public static Function GetGenericTypeSelector(IList<Type> types) => Context.CurrentGlobalContext.GetGenericTypeSelector(types);

    static JSValue()
    {
      JSObject jsObject = new JSObject();
      jsObject._valueType = JSValueType.Object;
      jsObject._oValue = (object) null;
      jsObject._attributes = JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.SystemObject;
      JSValue.@null = jsObject;
      JSValue.nullString = new JSValue()
      {
        _valueType = JSValueType.String,
        _oValue = (object) nameof (@null),
        _attributes = JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.SystemObject
      };
    }

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = false, Inherited = false)]
    protected sealed class ProtoConverterAttribute : ConvertValueAttribute
    {
      public override object From(object source)
      {
        if (!(source is JSValue jsValue) || jsValue.ValueType >= JSValueType.Object)
          return source;
        JSObject jsObject = new JSObject();
        jsObject._valueType = JSValueType.Undefined;
        return (object) jsObject;
      }

      public override object To(JSValue source)
      {
        if (source.ValueType < JSValueType.Object)
        {
          JSObject jsObject = new JSObject();
          jsObject._valueType = JSValueType.Undefined;
          return (object) jsObject;
        }
        return source._oValue is JSObject oValue ? (object) oValue : (object) source;
      }
    }
  }
}
