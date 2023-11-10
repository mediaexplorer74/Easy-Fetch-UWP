// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.JSObject
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core.Interop;
using NiL.JS.Expressions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace NiL.JS.Core
{
  public class JSObject : JSValue
  {
    internal IDictionary<string, JSValue> _fields;
    internal IDictionary<Symbol, JSValue> _symbols;
    internal JSObject _objectPrototype;

    [DoNotDelete]
    [DoNotEnumerate]
    [NotConfigurable]
    [CLSCompliant(false)]
    public override sealed JSObject __proto__
    {
      [Hidden] get
      {
        if (!this.Defined || this.IsNull)
          ExceptionHelper.Throw((Error) new TypeError("Can not get prototype of null or undefined"));
        if (this._valueType >= JSValueType.Object && this._oValue != this && this._oValue is JSValue oValue)
          return oValue.__proto__;
        if (this._objectPrototype == null || this._objectPrototype._valueType < JSValueType.Object)
        {
          this._objectPrototype = this.GetDefaultPrototype();
          if (this._objectPrototype == null)
            this._objectPrototype = JSValue.@null;
        }
        return this._objectPrototype._oValue == null ? JSValue.@null : this._objectPrototype;
      }
      [Hidden] set
      {
        if ((this._attributes & JSValueAttributesInternal.Immutable) != JSValueAttributesInternal.None || this._valueType < JSValueType.Object || value != null && value._valueType < JSValueType.Object)
          return;
        if (this._oValue != this && this._oValue is JSObject)
        {
          (this._oValue as JSObject).__proto__ = value;
          this._objectPrototype = (JSObject) null;
        }
        else if (value == null || value._oValue == null)
        {
          this._objectPrototype = JSValue.@null;
        }
        else
        {
          if (!(value._oValue is JSObject jsObject1))
            jsObject1 = value;
          for (JSObject jsObject2 = jsObject1; jsObject2 != null && jsObject2 != JSValue.@null && jsObject2._valueType > JSValueType.Undefined; jsObject2 = jsObject2.__proto__)
          {
            if (jsObject2 == this || jsObject2._oValue == this)
              ExceptionHelper.Throw(new Error("Try to set cyclic __proto__ value."));
          }
          if (!(value._oValue is JSObject jsObject3))
            jsObject3 = value;
          this._objectPrototype = jsObject3;
        }
      }
    }

    [Hidden]
    protected internal JSObject()
    {
    }

    [Hidden]
    public static JSObject CreateObject() => JSObject.CreateObject(false, JSAttributes.None);

    internal static JSObject CreateObject(bool createFields = false, JSAttributes attributes = JSAttributes.None)
    {
      JSObject jsObject1 = new JSObject();
      jsObject1._valueType = JSValueType.Object;
      JSObject jsObject2 = jsObject1;
      jsObject2._oValue = (object) jsObject2;
      jsObject2._attributes = (JSValueAttributesInternal) attributes;
      if (createFields)
        jsObject2._fields = JSObject.getFieldsContainer();
      return jsObject2;
    }

    protected internal override JSValue GetProperty(
      JSValue key,
      bool forWrite,
      PropertyScope propertyScope)
    {
      JSValue property = (JSValue) null;
      JSObject jsObject = (JSObject) null;
      string str = (string) null;
      if (key._valueType == JSValueType.Symbol)
      {
        property = this.getSymbol(key, forWrite, propertyScope);
      }
      else
      {
        if (forWrite || this._fields != null)
          str = key.ToString();
        bool flag = (propertyScope >= PropertyScope.Super || this._fields == null || !this._fields.TryGetValue(str, out property) || property._valueType < JSValueType.Undefined) && (jsObject = this.__proto__)._oValue != null;
        if (flag)
        {
          property = jsObject.GetProperty(key, false, propertyScope > PropertyScope.Common ? propertyScope - 1 : PropertyScope.Common);
          if (propertyScope == PropertyScope.Own && ((property._attributes & JSValueAttributesInternal.Field) == JSValueAttributesInternal.None || property._valueType != JSValueType.Property) || property._valueType < JSValueType.Undefined)
            property = (JSValue) null;
        }
        if (property == null)
        {
          if (!forWrite || (this._attributes & JSValueAttributesInternal.Immutable) != JSValueAttributesInternal.None)
            return propertyScope != PropertyScope.Own && string.CompareOrdinal(str, "__proto__") == 0 ? (JSValue) jsObject : JSValue.notExists;
          property = new JSValue()
          {
            _valueType = JSValueType.NotExistsInObject
          };
          if (this._fields == null)
            this._fields = JSObject.getFieldsContainer();
          this._fields[str] = property;
        }
        else if (forWrite && (property._attributes & JSValueAttributesInternal.SystemObject) > JSValueAttributesInternal.None | flag && (property._attributes & JSValueAttributesInternal.ReadOnly) == JSValueAttributesInternal.None && (property._valueType != JSValueType.Property || propertyScope == PropertyScope.Own))
        {
          property = property.CloneImpl(false);
          if (this._fields == null)
            this._fields = JSObject.getFieldsContainer();
          this._fields[str] = property;
        }
      }
      property._valueType |= JSValueType.NotExistsInObject;
      return property;
    }

    private JSValue getSymbol(JSValue key, bool forWrite, PropertyScope memberScope)
    {
      JSObject jsObject = (JSObject) null;
      JSValue symbol = (JSValue) null;
      Symbol oValue = key._oValue as Symbol;
      bool flag = (this._symbols == null || !this._symbols.TryGetValue(oValue, out symbol) || symbol._valueType < JSValueType.Undefined) && (jsObject = this.__proto__)._oValue != null;
      if (flag)
      {
        symbol = jsObject.GetProperty(key, false, memberScope);
        if (memberScope == PropertyScope.Own && ((symbol._attributes & JSValueAttributesInternal.Field) == JSValueAttributesInternal.None || symbol._valueType != JSValueType.Property) || symbol._valueType < JSValueType.Undefined)
          symbol = (JSValue) null;
      }
      if (symbol == null)
      {
        if (!forWrite || (this._attributes & JSValueAttributesInternal.Immutable) != JSValueAttributesInternal.None)
          return JSValue.notExists;
        symbol = new JSValue()
        {
          _valueType = JSValueType.NotExistsInObject
        };
        if (this._symbols == null)
          this._symbols = (IDictionary<Symbol, JSValue>) new Dictionary<Symbol, JSValue>();
        this._symbols[oValue] = symbol;
      }
      else if (forWrite && (symbol._attributes & JSValueAttributesInternal.SystemObject) > JSValueAttributesInternal.None | flag && (symbol._attributes & JSValueAttributesInternal.ReadOnly) == JSValueAttributesInternal.None && (symbol._valueType != JSValueType.Property || memberScope == PropertyScope.Own))
      {
        symbol = symbol.CloneImpl(false);
        if (this._symbols == null)
          this._symbols = (IDictionary<Symbol, JSValue>) new Dictionary<Symbol, JSValue>();
        this._symbols[oValue] = symbol;
      }
      return symbol;
    }

    protected internal override void SetProperty(
      JSValue key,
      JSValue value,
      PropertyScope propertyScope,
      bool throwOnError)
    {
      if (this._valueType >= JSValueType.Object && this._oValue != this)
      {
        if (this._oValue == null)
          ExceptionHelper.Throw((Error) new TypeError("Can not get property \"" + key?.ToString() + "\" of \"null\""));
        JSValue oValue = (JSValue) (this._oValue as JSObject);
        if (oValue != null)
        {
          oValue.SetProperty(key, value, propertyScope, throwOnError);
          return;
        }
      }
      JSValue property = this.GetProperty(key, true, PropertyScope.Common);
      if (property._valueType == JSValueType.Property)
      {
        Function setter = (property._oValue as PropertyPair).setter;
        if (setter != null)
        {
          setter.Call((JSValue) this, new Arguments()
          {
            value
          });
        }
        else
        {
          if (!throwOnError)
            return;
          ExceptionHelper.Throw((Error) new TypeError("Can not assign value to readonly property \"" + key?.ToString() + "\""));
        }
      }
      else if ((property._attributes & JSValueAttributesInternal.ReadOnly) != JSValueAttributesInternal.None)
      {
        if (!throwOnError)
          return;
        ExceptionHelper.Throw((Error) new TypeError("Can not assign value to readonly property \"" + key?.ToString() + "\""));
      }
      else
        property.Assign(value);
    }

    protected internal override bool DeleteProperty(JSValue key)
    {
      JSValue oValue;
      if (this._valueType >= JSValueType.Object && this._oValue != this)
      {
        if (this._oValue == null)
          ExceptionHelper.Throw((Error) new TypeError("Can't get property \"" + key?.ToString() + "\" of \"null\""));
        oValue = (JSValue) (this._oValue as JSObject);
        if (oValue != null)
          return oValue.DeleteProperty(key);
      }
      string key1;
      if (this._fields != null && this._fields.TryGetValue(key1 = key.ToString(), out oValue) && (!oValue.Exists || (oValue._attributes & JSValueAttributesInternal.DoNotDelete) == JSValueAttributesInternal.None))
      {
        if ((oValue._attributes & JSValueAttributesInternal.SystemObject) == JSValueAttributesInternal.None)
          oValue._valueType = JSValueType.NotExistsInObject;
        return this._fields.Remove(key1);
      }
      JSValue property = this.GetProperty(key, true, PropertyScope.Own);
      if (!property.Exists)
        return true;
      if ((property._attributes & JSValueAttributesInternal.SystemObject) != JSValueAttributesInternal.None)
        property = this.GetProperty(key, true, PropertyScope.Own);
      if ((property._attributes & JSValueAttributesInternal.DoNotDelete) != JSValueAttributesInternal.None)
        return false;
      property._valueType = JSValueType.NotExistsInObject;
      property._oValue = (object) null;
      return true;
    }

    [Hidden]
    protected internal override IEnumerator<KeyValuePair<string, JSValue>> GetEnumerator(
      bool hideNonEnum,
      EnumerationMode enumeratorMode)
    {
      if (this._fields != null)
      {
        foreach (KeyValuePair<string, JSValue> field in (IEnumerable<KeyValuePair<string, JSValue>>) this._fields)
        {
          if (field.Value.Exists && (!hideNonEnum || (field.Value._attributes & JSValueAttributesInternal.DoNotEnumerate) == JSValueAttributesInternal.None))
            yield return field;
        }
      }
      if (this._objectPrototype != null)
      {
        IEnumerator<KeyValuePair<string, JSValue>> enumerator = this._objectPrototype.GetEnumerator(hideNonEnum, EnumerationMode.RequireValues);
        while (enumerator.MoveNext())
        {
          if (enumerator.Current.Value._valueType >= JSValueType.Undefined && (enumerator.Current.Value._attributes & JSValueAttributesInternal.Field) != JSValueAttributesInternal.None)
            yield return enumerator.Current;
        }
        enumerator = (IEnumerator<KeyValuePair<string, JSValue>>) null;
      }
    }

    [Hidden]
    public override sealed void Assign(JSValue value)
    {
      if ((this._attributes & JSValueAttributesInternal.ReadOnly) == JSValueAttributesInternal.None)
      {
        if (this is GlobalObject)
          ExceptionHelper.Throw((Error) new ReferenceError("Invalid left-hand side"));
        throw new InvalidOperationException("Try to assign to a non-primitive value");
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static IDictionary<string, JSValue> getFieldsContainer() => (IDictionary<string, JSValue>) new StringMap<JSValue>();

    [DoNotEnumerate]
    [ArgumentsCount(2)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static JSValue create(Arguments args)
    {
      if (args[0]._valueType < JSValueType.Object)
        ExceptionHelper.Throw((Error) new TypeError("Prototype may be only Object or null."));
      if (!(args[0]._oValue is JSObject oValue1))
        oValue1 = JSValue.@null;
      JSObject jsObject1 = oValue1;
      if (!(args[1]._oValue is JSObject oValue2))
        oValue2 = JSValue.@null;
      JSObject targetObject = oValue2;
      if (args[1]._valueType >= JSValueType.Object && targetObject._oValue == null)
        ExceptionHelper.Throw((Error) new TypeError("Properties descriptor may be only Object."));
      JSObject jsObject2 = JSObject.CreateObject(true);
      if (jsObject1._valueType >= JSValueType.Object)
        jsObject2._objectPrototype = jsObject1;
      if (targetObject._valueType >= JSValueType.Object)
      {
        foreach (KeyValuePair<string, JSValue> keyValuePair in (JSValue) targetObject)
        {
          JSValue target = keyValuePair.Value;
          if (target._valueType == JSValueType.Property)
          {
            Function getter = (target._oValue as PropertyPair).getter;
            if (getter == null || getter._oValue == null)
              ExceptionHelper.Throw((Error) new TypeError("Invalid property descriptor for property " + keyValuePair.Key + " ."));
            target = (getter._oValue as Function).Call((JSValue) targetObject, (Arguments) null);
          }
          if (target._valueType < JSValueType.Object || target._oValue == null)
            ExceptionHelper.Throw((Error) new TypeError("Invalid property descriptor for property " + keyValuePair.Key + " ."));
          JSValue property1 = target["value"];
          if (property1._valueType == JSValueType.Property)
            property1 = Tools.InvokeGetter(property1, target);
          JSValue property2 = target["configurable"];
          if (property2._valueType == JSValueType.Property)
            property2 = Tools.InvokeGetter(property2, target);
          JSValue property3 = target["enumerable"];
          if (property3._valueType == JSValueType.Property)
            property3 = Tools.InvokeGetter(property3, target);
          JSValue property4 = target["writable"];
          if (property4._valueType == JSValueType.Property)
            property4 = Tools.InvokeGetter(property4, target);
          JSValue property5 = target["get"];
          if (property5._valueType == JSValueType.Property)
            property5 = Tools.InvokeGetter(property5, target);
          JSValue property6 = target["set"];
          if (property6._valueType == JSValueType.Property)
            property6 = Tools.InvokeGetter(property6, target);
          if (property1.Exists && (property5.Exists || property6.Exists))
            ExceptionHelper.Throw((Error) new TypeError("Property can not have getter or setter and default value."));
          if (property4.Exists && (property5.Exists || property6.Exists))
            ExceptionHelper.Throw((Error) new TypeError("Property can not have getter or setter and writable attribute."));
          if (property5.Defined && property5._valueType != JSValueType.Function)
            ExceptionHelper.Throw((Error) new TypeError("Getter mast be a function."));
          if (property6.Defined && property6._valueType != JSValueType.Function)
            ExceptionHelper.Throw((Error) new TypeError("Setter mast be a function."));
          JSValue jsValue = new JSValue()
          {
            _valueType = JSValueType.Undefined
          };
          jsObject2._fields[keyValuePair.Key] = jsValue;
          jsValue._attributes |= JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.NonConfigurable;
          if ((bool) property3)
            jsValue._attributes &= ~JSValueAttributesInternal.DoNotEnumerate;
          if ((bool) property2)
            jsValue._attributes &= ~(JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.NonConfigurable);
          if (property1.Exists)
          {
            JSValueAttributesInternal attributes = jsValue._attributes;
            jsValue._attributes = JSValueAttributesInternal.None;
            jsValue.Assign(property1);
            jsValue._attributes = attributes;
            if ((bool) property4)
              jsValue._attributes &= ~JSValueAttributesInternal.ReadOnly;
          }
          else if (property5.Exists || property6.Exists)
          {
            Function function1 = (Function) null;
            Function function2 = (Function) null;
            if (jsValue._valueType == JSValueType.Property)
            {
              function1 = (jsValue._oValue as PropertyPair).setter;
              function2 = (jsValue._oValue as PropertyPair).getter;
            }
            jsValue._valueType = JSValueType.Property;
            jsValue._oValue = (object) new PropertyPair()
            {
              setter = (property6.Exists ? property6._oValue as Function : function1),
              getter = (property5.Exists ? property5._oValue as Function : function2)
            };
          }
          else if ((bool) property4)
            jsValue._attributes &= ~JSValueAttributesInternal.ReadOnly;
        }
      }
      return (JSValue) jsObject2;
    }

    [DoNotEnumerate]
    [ArgumentsCount(2)]
    public static JSValue defineProperties(Arguments args)
    {
      if (args[0]._valueType < JSValueType.Object)
        ExceptionHelper.Throw((Error) new TypeError("Property define may only for Objects."));
      if (args[0]._oValue == null)
        ExceptionHelper.Throw((Error) new TypeError("Can not define properties of null."));
      if (!(args[0]._oValue is JSObject oValue1))
        oValue1 = JSValue.@null;
      JSObject target = oValue1;
      if (!(args[1]._oValue is JSObject oValue2))
        oValue2 = JSValue.@null;
      JSObject targetObject = oValue2;
      if (!args[1].Defined)
        ExceptionHelper.Throw((Error) new TypeError("Properties descriptor can not be undefined."));
      if (args[1]._valueType < JSValueType.Object)
        return (JSValue) target;
      if (targetObject._oValue == null)
        ExceptionHelper.Throw((Error) new TypeError("Properties descriptor can not be null."));
      if (target._valueType < JSValueType.Object || target._oValue == null)
        return (JSValue) target;
      if (targetObject._valueType > JSValueType.Undefined)
      {
        foreach (KeyValuePair<string, JSValue> keyValuePair in (JSValue) targetObject)
        {
          JSValue jsValue = keyValuePair.Value;
          if (jsValue._valueType == JSValueType.Property)
          {
            Function getter = (jsValue._oValue as PropertyPair).getter;
            if (getter == null || getter._oValue == null)
              ExceptionHelper.Throw((Error) new TypeError("Invalid property descriptor for property " + keyValuePair.Key + " ."));
            jsValue = (getter._oValue as Function).Call((JSValue) targetObject, (Arguments) null);
          }
          if (jsValue._valueType < JSValueType.Object || jsValue._oValue == null)
            ExceptionHelper.Throw((Error) new TypeError("Invalid property descriptor for property " + keyValuePair.Key + " ."));
          JSObject.definePropertyImpl(target, jsValue._oValue as JSObject, keyValuePair.Key);
        }
      }
      return (JSValue) target;
    }

    [DoNotEnumerate]
    [ArgumentsCount(3)]
    [CLSCompliant(false)]
    public static JSValue defineProperty(Arguments args)
    {
      if (args[0]._valueType < JSValueType.Object || args[0]._oValue == null)
        ExceptionHelper.Throw((Error) new TypeError("Object.defineProperty cannot apply to non-object."));
      if (!(args[0]._oValue is JSObject oValue1))
        oValue1 = JSValue.@null;
      JSObject target = oValue1;
      if (!(args[2]._oValue is JSObject oValue2))
        oValue2 = JSValue.@null;
      JSObject desc = oValue2;
      if (desc._valueType < JSValueType.Object || desc._oValue == null)
        ExceptionHelper.Throw((Error) new TypeError("Invalid property descriptor."));
      if (target._valueType < JSValueType.Object || target._oValue == null)
        return (JSValue) target;
      if (target is Proxy)
        target = (target as Proxy).PrototypeInstance ?? target;
      string memberName = args[1].ToString();
      return (JSValue) JSObject.definePropertyImpl(target, desc, memberName);
    }

    private static JSObject definePropertyImpl(JSObject target, JSObject desc, string memberName)
    {
      JSValue jsValue = desc["value"];
      if (jsValue._valueType == JSValueType.Property)
        jsValue = Tools.InvokeGetter(jsValue, (JSValue) desc);
      JSValue property1 = desc["configurable"];
      if (property1._valueType == JSValueType.Property)
        property1 = Tools.InvokeGetter(property1, (JSValue) desc);
      JSValue property2 = desc["enumerable"];
      if (property2._valueType == JSValueType.Property)
        property2 = Tools.InvokeGetter(property2, (JSValue) desc);
      JSValue property3 = desc["writable"];
      if (property3._valueType == JSValueType.Property)
        property3 = Tools.InvokeGetter(property3, (JSValue) desc);
      JSValue property4 = desc["get"];
      if (property4._valueType == JSValueType.Property)
        property4 = Tools.InvokeGetter(property4, (JSValue) desc);
      JSValue property5 = desc["set"];
      if (property5._valueType == JSValueType.Property)
        property5 = Tools.InvokeGetter(property5, (JSValue) desc);
      if (jsValue.Exists && (property4.Exists || property5.Exists))
        ExceptionHelper.Throw((Error) new TypeError("Property can not have getter or setter and default value."));
      if (property3.Exists && (property4.Exists || property5.Exists))
        ExceptionHelper.Throw((Error) new TypeError("Property can not have getter or setter and writable attribute."));
      if (property4.Defined && property4._valueType != JSValueType.Function)
        ExceptionHelper.Throw((Error) new TypeError("Getter mast be a function."));
      if (property5.Defined && property5._valueType != JSValueType.Function)
        ExceptionHelper.Throw((Error) new TypeError("Setter mast be a function."));
      JSValue first = target.DefineProperty(memberName);
      if ((first._attributes & JSValueAttributesInternal.Argument) != JSValueAttributesInternal.None && (property5.Exists || property4.Exists))
      {
        int result = 0;
        if (target is Arguments && int.TryParse(memberName, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture, out result) && result >= 0 && result < 16)
          (target as Arguments)[result] = first = first.CloneImpl(JSValueAttributesInternal.SystemObject);
        else
          target._fields[memberName] = first = first.CloneImpl(JSValueAttributesInternal.SystemObject);
        first._attributes &= ~JSValueAttributesInternal.Argument;
      }
      if ((first._attributes & JSValueAttributesInternal.SystemObject) != JSValueAttributesInternal.None)
        ExceptionHelper.Throw((Error) new TypeError("Can not define property \"" + memberName + "\". Object immutable."));
      if (target is NiL.JS.BaseLibrary.Array)
      {
        if (memberName == "length")
        {
          try
          {
            if (jsValue.Exists)
            {
              double d = Tools.JSObjectToDouble(jsValue);
              uint nlen = (uint) d;
              if (double.IsNaN(d) || double.IsInfinity(d) || (double) nlen != d)
                ExceptionHelper.Throw((Error) new RangeError("Invalid array length"));
              if ((first._attributes & JSValueAttributesInternal.ReadOnly) != JSValueAttributesInternal.None && (first._valueType == JSValueType.Double && d != first._dValue || first._valueType == JSValueType.Integer && (long) nlen != (long) first._iValue))
                ExceptionHelper.Throw((Error) new TypeError("Cannot change length of fixed size array"));
              if (!(target as NiL.JS.BaseLibrary.Array).SetLenght((long) nlen))
                ExceptionHelper.Throw((Error) new TypeError("Unable to reduce length because Exists not configurable elements"));
              jsValue = JSValue.notExists;
            }
          }
          finally
          {
            if (property3.Exists && !(bool) property3)
              first._attributes |= JSValueAttributesInternal.ReadOnly;
          }
        }
      }
      bool flag1 = first._valueType < JSValueType.Undefined;
      bool flag2 = (first._attributes & JSValueAttributesInternal.NonConfigurable) == JSValueAttributesInternal.None | flag1;
      if (!flag2)
      {
        if (property2.Exists && (first._attributes & JSValueAttributesInternal.DoNotEnumerate) > JSValueAttributesInternal.None == (bool) property2)
          ExceptionHelper.Throw((Error) new TypeError("Cannot change enumerable attribute for non configurable property."));
        if (property3.Exists && (first._attributes & JSValueAttributesInternal.ReadOnly) != JSValueAttributesInternal.None && (bool) property3)
          ExceptionHelper.Throw((Error) new TypeError("Cannot change writable attribute for non configurable property."));
        if (property1.Exists && (bool) property1)
          ExceptionHelper.Throw((Error) new TypeError("Cannot set configurable attribute to true."));
        if ((first._valueType != JSValueType.Property || (first._attributes & JSValueAttributesInternal.Field) != JSValueAttributesInternal.None) && (property5.Exists || property4.Exists))
          ExceptionHelper.Throw((Error) new TypeError("Cannot redefine not configurable property from immediate value to accessor property"));
        if (first._valueType == JSValueType.Property && (first._attributes & JSValueAttributesInternal.Field) == JSValueAttributesInternal.None && jsValue.Exists)
          ExceptionHelper.Throw((Error) new TypeError("Cannot redefine not configurable property from accessor property to immediate value"));
        if (first._valueType == JSValueType.Property && (first._attributes & JSValueAttributesInternal.Field) == JSValueAttributesInternal.None && property5.Exists && ((first._oValue as PropertyPair).setter != null && (first._oValue as PropertyPair).setter._oValue != property5._oValue || (first._oValue as PropertyPair).setter == null && property5.Defined))
          ExceptionHelper.Throw((Error) new TypeError("Cannot redefine setter of not configurable property."));
        if (first._valueType == JSValueType.Property && (first._attributes & JSValueAttributesInternal.Field) == JSValueAttributesInternal.None && property4.Exists && ((first._oValue as PropertyPair).getter != null && (first._oValue as PropertyPair).getter._oValue != property4._oValue || (first._oValue as PropertyPair).getter == null && property4.Defined))
          ExceptionHelper.Throw((Error) new TypeError("Cannot redefine getter of not configurable property."));
      }
      if (jsValue.Exists)
      {
        if (!flag2 && (first._attributes & JSValueAttributesInternal.ReadOnly) != JSValueAttributesInternal.None && (!StrictEqual.Check(first, jsValue) || (first._valueType != JSValueType.Undefined || jsValue._valueType != JSValueType.Undefined) && first.IsNumber && jsValue.IsNumber && 1.0 / Tools.JSObjectToDouble(first) != 1.0 / Tools.JSObjectToDouble(jsValue)) && (first._valueType != JSValueType.Double || jsValue._valueType != JSValueType.Double || !double.IsNaN(first._dValue) || !double.IsNaN(jsValue._dValue)))
          ExceptionHelper.Throw((Error) new TypeError("Cannot change value of not configurable not writable peoperty."));
        first._valueType = JSValueType.Undefined;
        JSValueAttributesInternal attributes = first._attributes;
        first._attributes = JSValueAttributesInternal.None;
        first.Assign(jsValue);
        first._attributes = attributes;
      }
      else if (property4.Exists || property5.Exists)
      {
        Function function1 = (Function) null;
        Function function2 = (Function) null;
        if (first._valueType == JSValueType.Property)
        {
          function1 = (first._oValue as PropertyPair).setter;
          function2 = (first._oValue as PropertyPair).getter;
        }
        first._valueType = JSValueType.Property;
        first._oValue = (object) new PropertyPair()
        {
          setter = (property5.Exists ? property5._oValue as Function : function1),
          getter = (property4.Exists ? property4._oValue as Function : function2)
        };
      }
      else if (flag1)
        first._valueType = JSValueType.Undefined;
      if (flag1)
      {
        first._attributes |= JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.NonConfigurable;
      }
      else
      {
        JSValueAttributesInternal attributes = first._attributes;
        if (property1.Exists && (flag2 || !(bool) property1))
          first._attributes |= JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.NonConfigurable;
        if (property2.Exists && (flag2 || !(bool) property2))
          first._attributes |= JSValueAttributesInternal.DoNotEnumerate;
        if (property3.Exists && (flag2 || !(bool) property3))
          first._attributes |= JSValueAttributesInternal.ReadOnly;
        if (first._attributes != attributes && (first._attributes & JSValueAttributesInternal.Argument) != JSValueAttributesInternal.None)
        {
          int result = 0;
          if (target is Arguments && int.TryParse(memberName, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture, out result) && result >= 0 && result < 16)
            (target as Arguments)[result] = first = first.CloneImpl(JSValueAttributesInternal.Argument | JSValueAttributesInternal.SystemObject);
          else
            target._fields[memberName] = first = first.CloneImpl(JSValueAttributesInternal.Argument | JSValueAttributesInternal.SystemObject);
        }
      }
      if (flag2)
      {
        if ((bool) property2)
          first._attributes &= ~JSValueAttributesInternal.DoNotEnumerate;
        if ((bool) property1)
          first._attributes &= ~(JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.NonConfigurable);
        if ((bool) property3)
          first._attributes &= ~JSValueAttributesInternal.ReadOnly;
      }
      return target;
    }

    [DoNotEnumerate]
    [CLSCompliant(false)]
    public void __defineGetter__(Arguments args)
    {
      if (args._iValue < 2)
        ExceptionHelper.Throw((Error) new TypeError("Missed parameters"));
      if (args[1]._valueType != JSValueType.Function)
        ExceptionHelper.Throw((Error) new TypeError("Expecting function as second parameter"));
      JSValue property = this.GetProperty(args[0], true, PropertyScope.Own);
      if ((property._attributes & JSValueAttributesInternal.NonConfigurable) != JSValueAttributesInternal.None)
        ExceptionHelper.Throw((Error) new TypeError("Cannot change value of not configurable peoperty."));
      if ((property._attributes & JSValueAttributesInternal.ReadOnly) != JSValueAttributesInternal.None)
        ExceptionHelper.Throw((Error) new TypeError("Cannot change value of readonly peoperty."));
      if (property._valueType == JSValueType.Property)
      {
        (property._oValue as PropertyPair).getter = args[1].Value as Function;
      }
      else
      {
        property._valueType = JSValueType.Property;
        property._oValue = (object) new PropertyPair()
        {
          getter = (args[1].Value as Function)
        };
      }
    }

    [DoNotEnumerate]
    [CLSCompliant(false)]
    public void __defineSetter__(Arguments args)
    {
      if (args._iValue < 2)
        ExceptionHelper.Throw((Error) new TypeError("Missed parameters"));
      if (args[1]._valueType != JSValueType.Function)
        ExceptionHelper.Throw((Error) new TypeError("Expecting function as second parameter"));
      JSValue property = this.GetProperty(args[0], true, PropertyScope.Own);
      if ((property._attributes & JSValueAttributesInternal.NonConfigurable) != JSValueAttributesInternal.None)
        ExceptionHelper.Throw((Error) new TypeError("Cannot change value of not configurable peoperty."));
      if ((property._attributes & JSValueAttributesInternal.ReadOnly) != JSValueAttributesInternal.None)
        ExceptionHelper.Throw((Error) new TypeError("Cannot change value of readonly peoperty."));
      if (property._valueType == JSValueType.Property)
      {
        (property._oValue as PropertyPair).setter = args[1]._oValue as Function;
      }
      else
      {
        property._valueType = JSValueType.Property;
        property._oValue = (object) new PropertyPair()
        {
          setter = (args[1].Value as Function)
        };
      }
    }

    [DoNotEnumerate]
    [CLSCompliant(false)]
    public JSObject __lookupGetter__(Arguments args)
    {
      JSValue property = this.GetProperty(args[0], false, PropertyScope.Common);
      return property._valueType == JSValueType.Property ? (JSObject) (property._oValue as PropertyPair).getter : (JSObject) null;
    }

    [DoNotEnumerate]
    [CLSCompliant(false)]
    public JSObject __lookupSetter__(Arguments args)
    {
      JSValue property = this.GetProperty(args[0], false, PropertyScope.Common);
      return property._valueType == JSValueType.Property ? (JSObject) (property._oValue as PropertyPair).getter : (JSObject) null;
    }

    [DoNotEnumerate]
    public static JSValue freeze(Arguments args)
    {
      if (args[0]._valueType < JSValueType.Object)
        ExceptionHelper.Throw((Error) new TypeError("Object.freeze called on non-object."));
      if (args[0]._oValue == null)
        ExceptionHelper.Throw((Error) new TypeError("Object.freeze called on null."));
      if (!(args[0].Value is JSObject oValue))
        oValue = args[0]._oValue as JSObject;
      JSObject jsObject = oValue;
      jsObject._attributes |= JSValueAttributesInternal.Immutable;
      IEnumerator<KeyValuePair<string, JSValue>> enumerator = jsObject.GetEnumerator(false, EnumerationMode.RequireValuesForWrite);
      while (enumerator.MoveNext())
      {
        JSValue jsValue = enumerator.Current.Value;
        if ((jsValue._attributes & JSValueAttributesInternal.SystemObject) == JSValueAttributesInternal.None)
          jsValue._attributes |= JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.NonConfigurable;
      }
      return (JSValue) jsObject;
    }

    [DoNotEnumerate]
    public static JSValue preventExtensions(Arguments args)
    {
      if (args[0]._valueType < JSValueType.Object)
        ExceptionHelper.Throw((Error) new TypeError("Prevent the expansion can only for objects"));
      if (args[0]._oValue == null)
        ExceptionHelper.Throw((Error) new TypeError("Can not prevent extensions for null"));
      if (!(args[0].Value is JSObject oValue))
        oValue = args[0]._oValue as JSObject;
      oValue._attributes |= JSValueAttributesInternal.Immutable;
      return (JSValue) oValue;
    }

    [DoNotEnumerate]
    public static JSValue isExtensible(Arguments args)
    {
      if (args[0]._valueType < JSValueType.Object)
        ExceptionHelper.Throw((Error) new TypeError("Object.isExtensible called on non-object."));
      if (args[0]._oValue == null)
        ExceptionHelper.Throw((Error) new TypeError("Object.isExtensible called on null."));
      if (!(args[0].Value is JSObject oValue))
        oValue = args[0]._oValue as JSObject;
      return (JSValue) ((oValue._attributes & JSValueAttributesInternal.Immutable) == JSValueAttributesInternal.None);
    }

    [DoNotEnumerate]
    public static JSValue isSealed(Arguments args)
    {
      if (args[0]._valueType < JSValueType.Object)
        ExceptionHelper.Throw((Error) new TypeError("Object.isSealed called on non-object."));
      if (args[0]._oValue == null)
        ExceptionHelper.Throw((Error) new TypeError("Object.isSealed called on null."));
      if (!(args[0].Value is JSObject oValue))
        oValue = args[0]._oValue as JSObject;
      JSObject jsObject = oValue;
      if ((jsObject._attributes & JSValueAttributesInternal.Immutable) == JSValueAttributesInternal.None)
        return (JSValue) false;
      if (jsObject is Proxy)
        return (JSValue) true;
      if (jsObject is NiL.JS.BaseLibrary.Array array)
      {
        foreach (JSValue jsValue in array._data)
        {
          if (jsValue != null && jsValue.Exists && jsValue._valueType >= JSValueType.Object && jsValue._oValue != null && (jsValue._attributes & JSValueAttributesInternal.NonConfigurable) == JSValueAttributesInternal.None)
            return (JSValue) false;
        }
      }
      if (jsObject._fields != null)
      {
        foreach (KeyValuePair<string, JSValue> field in (IEnumerable<KeyValuePair<string, JSValue>>) jsObject._fields)
        {
          if (field.Value._valueType >= JSValueType.Object && field.Value._oValue != null && (field.Value._attributes & JSValueAttributesInternal.NonConfigurable) == JSValueAttributesInternal.None)
            return (JSValue) false;
        }
      }
      return (JSValue) true;
    }

    [DoNotEnumerate]
    public static JSObject seal(Arguments args)
    {
      if (args[0]._valueType < JSValueType.Object)
        ExceptionHelper.Throw((Error) new TypeError("Object.seal called on non-object."));
      if (args[0]._oValue == null)
        ExceptionHelper.Throw((Error) new TypeError("Object.seal called on null."));
      if (!(args[0].Value is JSObject oValue))
        oValue = args[0]._oValue as JSObject;
      JSObject jsObject = oValue;
      jsObject._attributes |= JSValueAttributesInternal.Immutable;
      IEnumerator<KeyValuePair<string, JSValue>> enumerator = jsObject.GetEnumerator(false, EnumerationMode.RequireValuesForWrite);
      while (enumerator.MoveNext())
      {
        JSValue jsValue = enumerator.Current.Value;
        if ((jsValue._attributes & JSValueAttributesInternal.SystemObject) == JSValueAttributesInternal.None)
          jsValue._attributes |= JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.NonConfigurable;
      }
      return jsObject;
    }

    [DoNotEnumerate]
    public static JSValue isFrozen(Arguments args)
    {
      if (args[0]._valueType < JSValueType.Object)
        ExceptionHelper.Throw((Error) new TypeError("Object.isFrozen called on non-object."));
      if (args[0]._oValue == null)
        ExceptionHelper.Throw((Error) new TypeError("Object.isFrozen called on null."));
      if (!(args[0].Value is JSObject oValue))
        oValue = args[0]._oValue as JSObject;
      JSObject jsObject = oValue;
      if ((jsObject._attributes & JSValueAttributesInternal.Immutable) == JSValueAttributesInternal.None)
        return (JSValue) false;
      switch (jsObject)
      {
        case Proxy _:
          return (JSValue) true;
        case NiL.JS.BaseLibrary.Array array:
          using (IEnumerator<KeyValuePair<int, JSValue>> enumerator = array._data.DirectOrder.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<int, JSValue> current = enumerator.Current;
              if (current.Value != null && current.Value.Exists && ((current.Value._attributes & JSValueAttributesInternal.NonConfigurable) == JSValueAttributesInternal.None || current.Value._valueType != JSValueType.Property && (current.Value._attributes & JSValueAttributesInternal.ReadOnly) == JSValueAttributesInternal.None))
                return (JSValue) false;
            }
            break;
          }
        case Arguments _:
          Arguments arguments = jsObject as Arguments;
          for (int index = 0; index < 16; ++index)
          {
            if ((arguments[index]._attributes & JSValueAttributesInternal.NonConfigurable) == JSValueAttributesInternal.None || arguments[index]._valueType != JSValueType.Property && (arguments[index]._attributes & JSValueAttributesInternal.ReadOnly) == JSValueAttributesInternal.None)
              return (JSValue) false;
          }
          break;
      }
      if (jsObject._fields != null)
      {
        foreach (KeyValuePair<string, JSValue> field in (IEnumerable<KeyValuePair<string, JSValue>>) jsObject._fields)
        {
          if ((field.Value._attributes & JSValueAttributesInternal.NonConfigurable) == JSValueAttributesInternal.None || field.Value._valueType != JSValueType.Property && (field.Value._attributes & JSValueAttributesInternal.ReadOnly) == JSValueAttributesInternal.None)
            return (JSValue) false;
        }
      }
      return (JSValue) true;
    }

    [DoNotEnumerate]
    public static JSObject getPrototypeOf(Arguments args)
    {
      if (args[0]._valueType < JSValueType.Object)
        ExceptionHelper.Throw((Error) new TypeError("Parameter isn't an Object."));
      return args[0].__proto__;
    }

    [DoNotEnumerate]
    [ArgumentsCount(2)]
    public static JSValue getOwnPropertyDescriptor(Arguments args)
    {
      if (args[0]._valueType <= JSValueType.Undefined)
        ExceptionHelper.Throw((Error) new TypeError("Object.getOwnPropertyDescriptor called on undefined."));
      if (args[0]._valueType < JSValueType.Object)
        ExceptionHelper.Throw((Error) new TypeError("Object.getOwnPropertyDescriptor called on non-object."));
      if (!(args[0]._oValue is JSObject oValue))
        oValue = JSValue.@null;
      JSObject targetObject = oValue;
      JSValue property = targetObject.GetProperty(args[1], false, PropertyScope.Own);
      if (property._valueType < JSValueType.Undefined)
        return JSValue.undefined;
      if ((property._attributes & JSValueAttributesInternal.SystemObject) != JSValueAttributesInternal.None)
        property = targetObject.GetProperty(args[1], true, PropertyScope.Own);
      JSObject propertyDescriptor = JSObject.CreateObject();
      if (property._valueType != JSValueType.Property || (property._attributes & JSValueAttributesInternal.Field) != JSValueAttributesInternal.None)
      {
        if (property._valueType == JSValueType.Property)
          propertyDescriptor["value"] = (property._oValue as PropertyPair).getter.Call((JSValue) targetObject, (Arguments) null);
        else
          propertyDescriptor["value"] = property;
        propertyDescriptor["writable"] = (JSValue) (property._valueType < JSValueType.Undefined || (property._attributes & JSValueAttributesInternal.ReadOnly) == JSValueAttributesInternal.None);
      }
      else
      {
        propertyDescriptor["set"] = (JSValue) (property._oValue as PropertyPair).setter;
        propertyDescriptor["get"] = (JSValue) (property._oValue as PropertyPair).getter;
      }
      propertyDescriptor["configurable"] = (JSValue) ((property._attributes & JSValueAttributesInternal.NonConfigurable) == JSValueAttributesInternal.None || (property._attributes & JSValueAttributesInternal.DoNotDelete) == JSValueAttributesInternal.None);
      propertyDescriptor["enumerable"] = (JSValue) ((property._attributes & JSValueAttributesInternal.DoNotEnumerate) == JSValueAttributesInternal.None);
      return (JSValue) propertyDescriptor;
    }

    [DoNotEnumerate]
    public static JSObject getOwnPropertyNames(Arguments args)
    {
      if (args[0]._valueType < JSValueType.Object)
        ExceptionHelper.Throw((Error) new TypeError("Object.getOwnPropertyNames called on non-object value."));
      if (args[0]._oValue == null)
        ExceptionHelper.Throw((Error) new TypeError("Cannot get property names of null"));
      JSObject oValue = args[0]._oValue as JSObject;
      NiL.JS.BaseLibrary.Array ownPropertyNames = new NiL.JS.BaseLibrary.Array();
      IEnumerator<KeyValuePair<string, JSValue>> enumerator = oValue.GetEnumerator(false, EnumerationMode.KeysOnly);
      while (enumerator.MoveNext())
        ownPropertyNames.Add((JSValue) enumerator.Current.Key);
      return (JSObject) ownPropertyNames;
    }

    [DoNotEnumerate]
    public static JSObject keys(Arguments args)
    {
      if (args[0]._valueType < JSValueType.Object)
        ExceptionHelper.Throw((Error) new TypeError("Object.keys called on non-object value."));
      if (args[0]._oValue == null)
        ExceptionHelper.Throw((Error) new TypeError("Cannot get property names of null"));
      JSObject oValue = args[0]._oValue as JSObject;
      NiL.JS.BaseLibrary.Array array = new NiL.JS.BaseLibrary.Array();
      IEnumerator<KeyValuePair<string, JSValue>> enumerator = oValue.GetEnumerator(true, EnumerationMode.KeysOnly);
      while (enumerator.MoveNext())
        array.Add((JSValue) enumerator.Current.Key);
      return (JSObject) array;
    }

    public static bool @is(JSValue value1, JSValue value2)
    {
      if (value1 == value2)
        return true;
      if (value1 != null && value2 == null || value1 == null && value2 != null || (value1._valueType | JSValueType.Undefined) != (value2._valueType | JSValueType.Undefined))
        return false;
      return value1._valueType == JSValueType.Double && double.IsNaN(value1._dValue) && double.IsNaN(value2._dValue) || StrictEqual.Check(value1, value2);
    }

    public static NiL.JS.BaseLibrary.Array getOwnPropertySymbols(JSObject obj)
    {
      object source;
      if (obj == null)
      {
        source = (object) null;
      }
      else
      {
        IDictionary<Symbol, JSValue> symbols = obj._symbols;
        source = symbols != null ? (object) symbols.Keys : (object) null;
      }
      if (source == null)
        source = (object) new Symbol[0];
      return new NiL.JS.BaseLibrary.Array((IEnumerable) source);
    }

    [JavaScriptName("assign")]
    public static JSValue JSAssign(Arguments args)
    {
      if (args._iValue == 0 || !args[0].Defined)
        ExceptionHelper.ThrowTypeError("Cannot convert undefined or null to object");
      JSObject jsObject = args[0].ToObject();
      for (int index = 1; index < args._iValue; ++index)
      {
        IEnumerator<KeyValuePair<string, JSValue>> enumerator = args[index].GetEnumerator(true, EnumerationMode.RequireValues);
        while (enumerator.MoveNext())
          jsObject.SetProperty((JSValue) enumerator.Current.Key, enumerator.Current.Value, PropertyScope.Own, true);
      }
      return (JSValue) jsObject;
    }
  }
}
