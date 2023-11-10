// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.GlobalContext
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core.Functions;
using NiL.JS.Core.Interop;
using NiL.JS.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NiL.JS.Core
{
  public sealed class GlobalContext : Context
  {
    internal JSObject _globalPrototype;
    private readonly Dictionary<Type, JSObject> _proxies;

    public string Name { get; private set; }

    public IndexersSupport IndexersSupport { get; set; }

    public JsonSerializersRegistry JsonSerializersRegistry { get; set; }

    public TimeZoneInfo CurrentTimeZone { get; set; }

    public GlobalContext()
      : this("")
    {
      this.Name = (string) null;
    }

    public GlobalContext(string name)
      : base((Context) null)
    {
      this.Name = name != null ? name : throw new ArgumentNullException(nameof (name));
      this._proxies = new Dictionary<Type, JSObject>();
      this.JsonSerializersRegistry = new JsonSerializersRegistry();
      this.ResetContext();
    }

    internal void ResetContext()
    {
      if (this._parent != null)
        throw new InvalidOperationException("Try to reset non-global context");
      this.ActivateInCurrentThread();
      try
      {
        if (this._variables != null)
          this._variables.Clear();
        else
          this._variables = JSObject.getFieldsContainer();
        this._proxies.Clear();
        this._globalPrototype = (JSObject) null;
        Function constructor = this.GetConstructor(typeof (JSObject)) as Function;
        this._variables.Add("Object", (JSValue) constructor);
        constructor._attributes |= JSValueAttributesInternal.DoNotDelete;
        this._globalPrototype = constructor.prototype as JSObject;
        this._globalPrototype._objectPrototype = JSValue.@null;
        this.DefineConstructor(typeof (NiL.JS.BaseLibrary.Math));
        this.DefineConstructor(typeof (NiL.JS.BaseLibrary.Array));
        this.DefineConstructor(typeof (JSON));
        this.DefineConstructor(typeof (NiL.JS.BaseLibrary.String));
        this.DefineConstructor(typeof (Function));
        this.DefineConstructor(typeof (Date));
        this.DefineConstructor(typeof (Number));
        this.DefineConstructor(typeof (Symbol));
        this.DefineConstructor(typeof (NiL.JS.BaseLibrary.Boolean));
        this.DefineConstructor(typeof (Error));
        this.DefineConstructor(typeof (TypeError));
        this.DefineConstructor(typeof (ReferenceError));
        this.DefineConstructor(typeof (EvalError));
        this.DefineConstructor(typeof (RangeError));
        this.DefineConstructor(typeof (URIError));
        this.DefineConstructor(typeof (SyntaxError));
        this.DefineConstructor(typeof (RegExp));
        this.DefineConstructor(typeof (ArrayBuffer));
        this.DefineConstructor(typeof (Int8Array));
        this.DefineConstructor(typeof (Uint8Array));
        this.DefineConstructor(typeof (Uint8ClampedArray));
        this.DefineConstructor(typeof (Int16Array));
        this.DefineConstructor(typeof (Uint16Array));
        this.DefineConstructor(typeof (Int32Array));
        this.DefineConstructor(typeof (Uint32Array));
        this.DefineConstructor(typeof (Float32Array));
        this.DefineConstructor(typeof (Float64Array));
        this.DefineConstructor(typeof (Promise));
        this.DefineConstructor(typeof (Map));
        this.DefineConstructor(typeof (Set));
        this.DefineConstructor(typeof (Debug));
        this.DefineVariable("console").Assign(JSValue.Marshal((object) new JSConsole()));
        this.DefineVariable("eval").Assign((JSValue) new EvalFunction());
        this._variables["eval"]._attributes |= JSValueAttributesInternal.Eval;
        this.DefineVariable("isNaN").Assign((JSValue) new ExternalFunction(new ExternalFunctionDelegate(GlobalFunctions.isNaN)));
        this.DefineVariable("unescape").Assign((JSValue) new ExternalFunction(new ExternalFunctionDelegate(GlobalFunctions.unescape)));
        this.DefineVariable("escape").Assign((JSValue) new ExternalFunction(new ExternalFunctionDelegate(GlobalFunctions.escape)));
        this.DefineVariable("encodeURI").Assign((JSValue) new ExternalFunction(new ExternalFunctionDelegate(GlobalFunctions.encodeURI)));
        this.DefineVariable("encodeURIComponent").Assign((JSValue) new ExternalFunction(new ExternalFunctionDelegate(GlobalFunctions.encodeURIComponent)));
        this.DefineVariable("decodeURI").Assign((JSValue) new ExternalFunction(new ExternalFunctionDelegate(GlobalFunctions.decodeURI)));
        this.DefineVariable("decodeURIComponent").Assign((JSValue) new ExternalFunction(new ExternalFunctionDelegate(GlobalFunctions.decodeURIComponent)));
        this.DefineVariable("isFinite").Assign((JSValue) new ExternalFunction(new ExternalFunctionDelegate(GlobalFunctions.isFinite)));
        this.DefineVariable("parseFloat").Assign((JSValue) new ExternalFunction(new ExternalFunctionDelegate(GlobalFunctions.parseFloat)));
        this.DefineVariable("parseInt").Assign((JSValue) new ExternalFunction(new ExternalFunctionDelegate(GlobalFunctions.parseInt)));
        this._variables["undefined"] = JSValue.undefined;
        this._variables["Infinity"] = Number.POSITIVE_INFINITY;
        this._variables["NaN"] = Number.NaN;
        this._variables["null"] = (JSValue) JSValue.@null;
        foreach (JSValue jsValue in (IEnumerable<JSValue>) this._variables.Values)
          jsValue._attributes |= JSValueAttributesInternal.DoNotEnumerate;
      }
      finally
      {
        this.Deactivate();
      }
      this.CurrentTimeZone = TimeZoneInfo.Local;
    }

    public void ActivateInCurrentThread()
    {
      if (Context.CurrentContext != null)
      {
        if (!(Context.CurrentContext is GlobalContext))
          throw new InvalidOperationException();
        Context.CurrentContext.Deactivate();
      }
      if (!this.Activate())
        throw new Exception("Unable to activate base context");
    }

    public void Deactivate()
    {
      if (Context.CurrentContext != this)
        throw new InvalidOperationException();
      if (base.Deactivate() != null)
        throw new InvalidOperationException("Invalid state");
    }

    internal JSObject GetPrototype(Type type) => (this.GetConstructor(type) is Function constructor ? constructor.prototype : (JSValue) null) as JSObject;

    public JSObject GetConstructor(Type type)
    {
      JSObject constructor;
      if (!this._proxies.TryGetValue(type, out constructor))
      {
        lock (this._proxies)
        {
          JSObject prototype = (JSObject) null;
          if (type.GetTypeInfo().ContainsGenericParameters)
          {
            constructor = (JSObject) this.GetGenericTypeSelector((IList<Type>) new Type[1]
            {
              type
            });
          }
          else
          {
            bool indexersSupport = this.IndexersSupport == IndexersSupport.ForceEnable || this.IndexersSupport == IndexersSupport.WithAttributeOnly && CustomAttributeExtensions.IsDefined(type.GetTypeInfo(), typeof (UseIndexersAttribute), false);
            StaticProxy staticProxy = new StaticProxy(this, type, indexersSupport);
            if (type.GetTypeInfo().IsAbstract)
            {
              this._proxies[type] = (JSObject) staticProxy;
              return (JSObject) staticProxy;
            }
            JSObject jsObject = (JSObject) null;
            Attribute[] array = CustomAttributeExtensions.GetCustomAttributes(type.GetTypeInfo(), typeof (PrototypeAttribute), true).ToArray<Attribute>();
            if (array.Length != 0 && (object) (array[0] as PrototypeAttribute).PrototypeType != (object) type)
            {
              Type prototypeType = (array[0] as PrototypeAttribute).PrototypeType;
              jsObject = (this.GetConstructor(prototypeType) as Function).prototype as JSObject;
              if ((array[0] as PrototypeAttribute).Replace && TypeExtensions.IsAssignableFrom(prototypeType, type))
              {
                prototype = jsObject;
              }
              else
              {
                PrototypeProxy prototypeProxy = new PrototypeProxy(this, type, indexersSupport);
                prototypeProxy._objectPrototype = jsObject;
                prototype = (JSObject) prototypeProxy;
              }
            }
            else
              prototype = (JSObject) new PrototypeProxy(this, type, indexersSupport);
            constructor = (object) type != (object) typeof (JSObject) ? (JSObject) new ConstructorProxy((Context) this, staticProxy, prototype) : (JSObject) new ObjectConstructor((Context) this, staticProxy, prototype);
            if (CustomAttributeExtensions.IsDefined(type.GetTypeInfo(), typeof (ImmutableAttribute), false))
              prototype._attributes |= JSValueAttributesInternal.Immutable;
            constructor._attributes = prototype._attributes;
            prototype._attributes |= JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.NonConfigurable;
            if (prototype != jsObject && (object) type != (object) typeof (ConstructorProxy))
              prototype._fields["constructor"] = (JSValue) constructor;
          }
          this._proxies[type] = constructor;
          if (prototype != null)
          {
            if (TypeExtensions.IsAssignableFrom(typeof (JSValue), type))
            {
              if (prototype._objectPrototype == null)
                prototype._objectPrototype = this._globalPrototype ?? JSValue.@null;
              JSObject prototypeInstance = (prototype as PrototypeProxy).PrototypeInstance;
            }
          }
        }
      }
      return constructor;
    }

    public Function GetGenericTypeSelector(IList<Type> types)
    {
      for (int index1 = 0; index1 < types.Count; ++index1)
      {
        for (int index2 = index1 + 1; index2 < types.Count; ++index2)
        {
          if (TypeExtensions.GetGenericArguments(types[index1]).Length == TypeExtensions.GetGenericArguments(types[index2]).Length)
            ExceptionHelper.Throw((Exception) new InvalidOperationException("Types have the same arguments"));
        }
      }
      return (Function) new ExternalFunction((ExternalFunctionDelegate) ((_this, args) =>
      {
        Type type = (Type) null;
        for (int index = 0; index < types.Count; ++index)
        {
          if (TypeExtensions.GetGenericArguments(types[index]).Length == args._iValue)
          {
            type = types[index];
            break;
          }
        }
        if ((object) type == null)
          ExceptionHelper.ThrowTypeError("Invalid arguments count for generic constructor");
        if (args._iValue == 0)
          return (JSValue) this.GetConstructor(type);
        Type[] typeArray = new Type[args._iValue];
        for (int index = 0; index < args._iValue; ++index)
        {
          typeArray[index] = args[index].As<Type>();
          if ((object) typeArray[index] == null)
            ExceptionHelper.ThrowTypeError("Invalid argument #" + index.ToString() + " for generic constructor");
        }
        return (JSValue) this.GetConstructor(type.MakeGenericType(typeArray));
      }));
    }

    public JSValue ProxyValue(object value)
    {
      if (value == null)
        return JSValue.NotExists;
      if (value is JSValue jsValue)
        return jsValue;
      switch (value.GetType().GetTypeCode())
      {
        case TypeCode.Boolean:
          return new JSValue()
          {
            _iValue = (bool) value ? 1 : 0,
            _valueType = JSValueType.Boolean
          };
        case TypeCode.Char:
          return new JSValue()
          {
            _oValue = (object) ((char) value).ToString(),
            _valueType = JSValueType.String
          };
        case TypeCode.SByte:
          return new JSValue()
          {
            _iValue = (int) (sbyte) value,
            _valueType = JSValueType.Integer
          };
        case TypeCode.Byte:
          return new JSValue()
          {
            _iValue = (int) (byte) value,
            _valueType = JSValueType.Integer
          };
        case TypeCode.Int16:
          return new JSValue()
          {
            _iValue = (int) (short) value,
            _valueType = JSValueType.Integer
          };
        case TypeCode.UInt16:
          return new JSValue()
          {
            _iValue = (int) (ushort) value,
            _valueType = JSValueType.Integer
          };
        case TypeCode.Int32:
          return new JSValue()
          {
            _iValue = (int) value,
            _valueType = JSValueType.Integer
          };
        case TypeCode.UInt32:
          uint num1 = (uint) value;
          if ((long) (int) num1 != (long) num1)
            return new JSValue()
            {
              _dValue = (double) num1,
              _valueType = JSValueType.Double
            };
          return new JSValue()
          {
            _iValue = (int) num1,
            _valueType = JSValueType.Integer
          };
        case TypeCode.Int64:
          return new JSValue()
          {
            _dValue = (double) (long) value,
            _valueType = JSValueType.Double
          };
        case TypeCode.UInt64:
          long num2 = (long) value;
          if ((long) (int) num2 != num2)
            return new JSValue()
            {
              _dValue = (double) num2,
              _valueType = JSValueType.Double
            };
          return new JSValue()
          {
            _iValue = (int) num2,
            _valueType = JSValueType.Integer
          };
        case TypeCode.Single:
          return new JSValue()
          {
            _dValue = (double) (float) value,
            _valueType = JSValueType.Double
          };
        case TypeCode.Double:
          return new JSValue()
          {
            _dValue = (double) value,
            _valueType = JSValueType.Double
          };
        case TypeCode.Decimal:
          return new JSValue()
          {
            _dValue = (double) (Decimal) value,
            _valueType = JSValueType.Double
          };
        case TypeCode.DateTime:
          return (JSValue) new ObjectWrapper((object) new Date((DateTime) value));
        case TypeCode.String:
          return new JSValue()
          {
            _oValue = value,
            _valueType = JSValueType.String
          };
        default:
          if ((object) (value as Delegate) != null)
            return value is ExternalFunctionDelegate ? (JSValue) new ExternalFunction(value as ExternalFunctionDelegate) : (JSValue) new MethodProxy((Context) this, (MethodBase) ((Delegate) value).GetMethodInfo(), ((Delegate) value).Target);
          if (value is IList)
            return (JSValue) new NativeList(value as IList);
          if (value is ExpandoObject)
            return (JSValue) new ExpandoObjectWrapper(value as ExpandoObject);
          if (value is Task)
          {
            Task<JSValue> result = !Tools.IsTaskOfT(value.GetType()) ? new Task<JSValue>((Func<JSValue>) (() => JSValue.NotExists)) : new Task<JSValue>((Func<JSValue>) (() => this.ProxyValue(TypeExtensions.GetMethod(value.GetType(), "get_Result", new Type[0]).Invoke(value, (object[]) null))));
            (value as Task).ContinueWith((Action<Task>) (task => result.Start()));
            return (JSValue) new ObjectWrapper((object) new Promise(result));
          }
          return value is IEnumerable && NativeReadOnlyListCtors.IsReadOnlyList(value) ? NativeReadOnlyListCtors.Create(value) : (JSValue) new ObjectWrapper(value);
      }
    }

    public override string ToString()
    {
      string str = "Global Context";
      return string.IsNullOrEmpty(this.Name) ? str : str + " \"" + this.Name + "\"";
    }
  }
}
